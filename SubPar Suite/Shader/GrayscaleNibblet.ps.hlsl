Texture2D InputTex : register(t0);
SamplerState Samp : register(s0);

float4 main(float2 uv : TEXCOORD) : SV_TARGET
{
	float4 c = InputTex.Sample(Samp, uv);

	float g = dot(c.rgb, float3(0.299, 0.587, 0.114));
	g = floor(g * 15.0) / 15.0;

	return float4(g, g, g, c.a);
}
