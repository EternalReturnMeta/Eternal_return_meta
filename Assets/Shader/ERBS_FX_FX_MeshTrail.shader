Shader "ERBS_FX/FX_MeshTrail" {
	Properties {
		_Glow ("Glow", Float) = 0
		_Noise ("Noise Strength", Float) = 0
		_Noise01 ("Noise01", 2D) = "white" {}
		_Noise01UV_TilingSpeed ("Noise01 UV_Tiling & Speed", Vector) = (0,0,0,0)
		_Noise02 ("Noise02", 2D) = "white" {}
		_Noise02UV_TilingSpeed ("Noise02 UV_Tiling & Speed", Vector) = (0,0,0,0)
		_MainTex ("MainTex", 2D) = "white" {}
		_MainTexUV_TilingSpeed ("MainTex UV_Tiling & Speed", Vector) = (0,0,0,0)
		_MaskTex ("MaskTex", 2D) = "white" {}
		[Space(20)] [Enum(LESS,0,GREATER,1,LEQUAL,2,GEQUAL,3,EQUAL,4,NOTEQUAL,5,ALWAYS,6)] _ZTestMode ("ZTest Mode", Float) = 2
		[Enum(UnityEngine.Rendering.CullMode)] _Cull ("Cull Mode", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_MatrixMVP;

			struct Vertex_Stage_Input
			{
				float3 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixMVP, float4(input.pos, 1.0));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState _MainTex_sampler;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(_MainTex_sampler, float2(input.uv.x, input.uv.y));
			}

			ENDHLSL
		}
	}
}