Shader "Custom/Sprite2DRimLight"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Rim Light Settings)]
        [HDR] _RimColor ("Rim Color", Color) = (1,1,0,1) // HDR 컬러 (Glow 효과)
        _RimWidth ("Rim Width (Pixel Offset)", Range(0, 10)) = 1.0
        _RimPower ("Rim Power (Sharpness)", Range(0.1, 10)) = 3.0
        _RimThreshold ("Rim Threshold", Range(0, 1)) = 0.5
        
        // 스프라이트 마스킹을 위한 스텐실 설정 (필요시 사용)
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "RenderPipeline" = "UniversalPipeline"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "SpriteRimLight"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
            };

            // 텍스처와 샘플러 선언
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            // 텍스처 사이즈 정보 (Unity가 자동으로 채워줌: x=1/width, y=1/height, z=width, w=height)
            float4 _MainTex_TexelSize;

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _RimColor;
                float _RimWidth;
                float _RimPower;
                float _RimThreshold;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = vertexInput.positionCS;
                OUT.uv = IN.uv;
                OUT.color = IN.color * _Color;

                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                // 1. 기본 텍스처 색상 샘플링
                float4 mainColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                // 알파가 없는 픽셀은 계산 제외 (최적화 및 아티팩트 방지)
                if (mainColor.a <= 0.01) discard;

                // 2. Rim Light 계산을 위한 주변 알파 샘플링
                // _MainTex_TexelSize.xy는 픽셀 하나의 UV 크기입니다.
                float2 offset = _MainTex_TexelSize.xy * _RimWidth;

                float alphaUp    = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(0, offset.y)).a;
                float alphaDown  = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(0, offset.y)).a;
                float alphaLeft  = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv - float2(offset.x, 0)).a;
                float alphaRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + float2(offset.x, 0)).a;

                // 3. 주변 투명도의 평균 계산 (주변이 투명할수록 값이 0에 가까워짐)
                float neighborAlpha = (alphaUp + alphaDown + alphaLeft + alphaRight) * 0.25;

                // 4. 림 팩터 계산
                // 내부는 불투명(1)하고 주변은 투명(0)한 곳이 가장자리(Rim)가 됨
                // mainColor.a를 곱하는 이유는 이미지가 투명한 곳에는 림라이트가 생기지 않게 하기 위함
                float rim = (1.0 - neighborAlpha) * mainColor.a;

                // 5. 림 효과 다듬기 (Threshold 및 Power 적용)
                // 부드러운 그라데이션 혹은 날카로운 선을 조절
                rim = smoothstep(_RimThreshold, 1.0, rim);
                rim = pow(rim, _RimPower);

                // 6. 최종 색상 합성
                // 기존 색상 + (림 컬러 * 림 강도)
                // SpriteRenderer의 Color 값도 영향을 받도록 IN.color 적용
                float4 finalColor = mainColor * IN.color;
                
                // Rim을 Additive(더하기) 방식으로 적용하여 빛나는 느낌 강조
                finalColor.rgb += _RimColor.rgb * rim * _RimColor.a;

                return finalColor;
            }
            ENDHLSL
        }
    }
}
