Shader "BI/BongUVW_UI_URP"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _G_tex ("G_tex", 2D) = "white" {}
        _R_tex ("R_tex", 2D) = "white" {}
        _B_tex ("B_tex", 2D) = "white" {}
        _value ("value", Float) = 2
        _A_Speed ("A_Speed", Float) = 0.3
        _B_Speed ("B_Speed", Float) = 0
        _C_Speed ("C_Speed", Float) = 0
        _D_Speed ("D_Speed", Float) = 0.3
        _Color ("Color", Color) = (0.5, 0.5, 0.5, 1)
        _stvalue ("st value", Range(0, 5)) = 0.4358974
        _RotateSpeed ("Rotate Speed", Range(0, 1)) = 0.1667272

        _Stencil ("Stencil ID", Float) = 0
        _StencilComp ("Stencil Comp", Float) = 0
        _StencilOp ("Stencil Op", Float) = 0
        _StencilWriteMask ("Stencil Write", Float) = 0
        _StencilReadMask ("Stencil Read", Float) = 0
        _ColorMask ("ColorMask", Float) = 15
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

        Pass
        {
            Name "UIAdditivePass"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            Stencil
            {
                Ref [_Stencil]
                Comp [_StencilComp]
                Pass [_StencilOp]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }

            ColorMask [_ColorMask]

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Texture declarations
            TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex);
            TEXTURE2D(_G_tex);          SAMPLER(sampler_G_tex);
            TEXTURE2D(_R_tex);          SAMPLER(sampler_R_tex);
            TEXTURE2D(_B_tex);          SAMPLER(sampler_B_tex);

            float4 _Color;
            float _value;
            float _A_Speed, _B_Speed, _C_Speed, _D_Speed;
            float _stvalue;
            float _RotateSpeed;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return mainColor * _Color;
            }

            ENDHLSL
        }
    }
}
