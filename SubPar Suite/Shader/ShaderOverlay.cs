using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using SubPar_Suite.Rendering;

namespace SubPar_Suite.Views;

public partial class ShaderOverlay : UserControl
{
	private readonly D3DOverlayRenderer Renderer;
	private readonly DispatcherTimer Timer;

	public ShaderOverlay()
	{
		InitializeComponent();

		Renderer = new D3DOverlayRenderer();

		Timer = new DispatcherTimer
		{
			Interval = TimeSpan.FromMilliseconds(100)
		};

		Timer.Tick += (_, _) => UpdateOverlay();

		Loaded += (_, _) => Timer.Start();
		Unloaded += (_, _) =>
		{
			Timer.Stop();
			Renderer.Dispose();
		};
	}

	public UIElement Input
	{
		get => (UIElement)InputPresenter.Content;
		set => InputPresenter.Content = value;
	}

	private void UpdateOverlay()
	{
		if (ActualWidth <= 0 || ActualHeight <= 0)
			return;

		var dpi = VisualTreeHelper.GetDpi(this);

		var bitmap = new RenderTargetBitmap(
			(int)ActualWidth,
			(int)ActualHeight,
			dpi.PixelsPerInchX,
			dpi.PixelsPerInchY,
			PixelFormats.Pbgra32);

		bitmap.Render(this);

		OverlayImage.SetCurrentValue(
			Image.SourceProperty,
			Renderer.Render(bitmap));
	}
}
