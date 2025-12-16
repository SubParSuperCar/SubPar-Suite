using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.D3DCompiler;
using Vortice.DXGI;
using static Vortice.Direct3D11.D3D11;

namespace SubPar_Suite.Rendering;

internal sealed class D3DOverlayRenderer : IDisposable
{
	private readonly ID3D11Device Device;
	private readonly ID3D11DeviceContext Context;
	private readonly ID3D11PixelShader PixelShader;
	private readonly D3DImage D3DImage;

	public D3DOverlayRenderer()
	{
		D3D11CreateDevice(
			null,
			DriverType.Hardware,
			DeviceCreationFlags.BgraSupport,
			null,
			out Device,
			out Context);

		var shaderPath = Path.Combine(
			AppContext.BaseDirectory,
			"Shaders",
			"GrayscaleNibblet.ps.hlsl");

		PixelShader = Device.CreatePixelShader(
			CompilePixelShader(shaderPath));

		D3DImage = new D3DImage();
	}

	public BitmapSource Render(BitmapSource source)
	{
		int width = source.PixelWidth;
		int height = source.PixelHeight;

		byte[] pixels = new byte[width * height * 4];
		source.CopyPixels(pixels, width * 4, 0);

		var desc = new Texture2DDescription
		{
			Width = width,
			Height = height,
			Format = Format.B8G8R8A8_UNorm,
			MipLevels = 1,
			ArraySize = 1,
			BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
			Usage = ResourceUsage.Default,
			SampleDescription = new SampleDescription(1, 0)
		};

		using var texture = Device.CreateTexture2D(
			desc,
			new SubresourceData(pixels, width * 4));

		using var surface = texture.QueryInterface<IDXGISurface>();

		D3DImage.Lock();
		D3DImage.SetBackBuffer(
			D3DResourceType.IDirect3DSurface9,
			surface.NativePointer);
		D3DImage.AddDirtyRect(new Int32Rect(0, 0, width, height));
		D3DImage.Unlock();

		return D3DImage;
	}

	private static byte[] CompilePixelShader(string path)
	{
		var result = Compiler.CompileFromFile(
			path,
			"main",
			"ps_5_0");

		return result.ToArray();
	}

	public void Dispose()
	{
		PixelShader.Dispose();
		Context.Dispose();
		Device.Dispose();
	}
}
