Shader "Custom/RevealUnlitStencil_XR"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (1,1,1,1)
        _AlphaCutoff ("Alpha Cutoff (floor)", Range(0,1)) = 0.01

        [Header(Fake Lighting)]
        _Ambient ("Ambient", Range(0,1)) = 0.35
        _FlashBoost ("Flash Boost", Range(0,5)) = 1.0
        _Specular ("Specular", Range(0,2)) = 0.25
        _SpecPower ("Spec Power", Range(1,128)) = 32
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Name "UnlitStencilGated"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            // Only draw where stencil == 1 (written by the mask)
            Stencil
            {
                Ref 1
                Comp Equal
                Pass Keep
                Fail Keep
                ZFail Keep
            }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            // XR + instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _AlphaCutoff;

                float _Ambient;
                float _FlashBoost;
                float _Specular;
                float _SpecPower;
            CBUFFER_END

            // Globals set by FlashlightRevealController
            float4 _FlashlightPosWS;        // xyz position
            float4 _FlashlightDirWS;        // xyz forward
            float  _FlashlightCosHalfAngle; // not strictly needed here, but kept for consistency
            float  _FlashlightRange;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                float3 normalWS    : TEXCOORD2;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs pos = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nor = GetVertexNormalInputs(IN.normalOS, float4(1,0,0,1)); // tangent not needed

                OUT.positionHCS = pos.positionCS;
                OUT.positionWS  = pos.positionWS;
                OUT.normalWS    = nor.normalWS;
                OUT.uv          = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                float4 baseSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // Discard only fully/near-transparent pixels; semi-transparent ones blend normally
                clip(baseSample.a - _AlphaCutoff);

                float3 albedo = baseSample.rgb;

                // Fake flashlight lighting
                float3 N = SafeNormalize(IN.normalWS);

                float3 lightPos = _FlashlightPosWS.xyz;
                float3 Ldir = SafeNormalize(lightPos - IN.positionWS);     // point -> light
                float  dist = length(lightPos - IN.positionWS);

                // Distance falloff (smooth)
                float range = max(_FlashlightRange, 0.0001);
                float att = saturate(1.0 - dist / range);
                att = att * att; // quadratic-ish falloff

                // Diffuse
                float ndotl = saturate(dot(N, Ldir));
                float diffuse = ndotl * att * _FlashBoost;

                // Simple specular (Blinn-Phong-ish)
                float3 V = SafeNormalize(GetCameraPositionWS() - IN.positionWS);
                float3 H = SafeNormalize(Ldir + V);
                float spec = pow(saturate(dot(N, H)), _SpecPower) * _Specular * att;

                float3 lit = albedo * (_Ambient + diffuse) + spec;

                // Pass texture/vertex alpha through for blending
                return half4(lit, baseSample.a);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
