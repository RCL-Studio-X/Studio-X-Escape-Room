Shader "Custom/URPToon"
{
    Properties
    {
        [Header(Base Color)]
        _BaseColor          ("Base Color", Color) = (1, 1, 1, 1)
        _BaseMap            ("Base Texture", 2D) = "white" {}

        [Header(Cel Shading)]
        _ShadowColor        ("Shadow Color", Color) = (0.3, 0.3, 0.5, 1)
        _ShadowThreshold    ("Shadow Threshold", Range(-1, 1)) = 0.0
        _ShadowSmoothness   ("Shadow Smoothness", Range(0, 0.5)) = 0.02
        _ShadowBands        ("Shadow Bands", Range(1, 8)) = 2

        [Header(Specular)]
        _SpecularColor      ("Specular Color", Color) = (1, 1, 1, 1)
        _SpecularThreshold  ("Specular Threshold", Range(0, 1)) = 0.8
        _SpecularSmoothness ("Specular Smoothness", Range(0, 0.1)) = 0.01
        _Glossiness         ("Glossiness", Range(1, 256)) = 64

        [Header(Rim Light)]
        _RimColor           ("Rim Color", Color) = (1, 1, 1, 1)
        _RimThreshold       ("Rim Threshold", Range(0, 1)) = 0.7
        _RimSmoothness      ("Rim Smoothness", Range(0, 0.2)) = 0.05
        _RimAmount          ("Rim Amount", Range(0, 1)) = 0.5

        [Header(Outline)]
        [Toggle] _UseOutline ("Use Outline", Float) = 1
        _OutlineColor       ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineWidth       ("Outline Width", Range(0, 0.1)) = 0.005
    }

    SubShader
    {
        Tags
        {
            "RenderType"       = "Opaque"
            "RenderPipeline"   = "UniversalPipeline"
            "Queue"            = "Geometry"
        }

        // ─────────────────────────────────────────────
        // PASS 1 – Outline (back-face hull)
        // ─────────────────────────────────────────────
        Pass
        {
            Name "Outline"
            // UniversalForwardOnly is picked up by URP's forward renderer
            // AND works correctly with single-pass instanced stereo VR.
            // SRPDefaultUnlit is NOT rendered in the second eye in many VR setups.
            Tags { "LightMode" = "UniversalForwardOnly" }
            Cull Front

            HLSLPROGRAM
            #pragma vertex   OutlineVert
            #pragma fragment OutlineFrag

            // ── Critical VR pragmas ───────────────────
            // Enables Single-Pass Instanced (SPI) stereo rendering.
            // Without this the vertex shader only outputs for eye index 0.
            #pragma multi_compile_instancing
            #pragma instancing_options renderingLayer
            // For XR / single-pass instanced you also need this URP keyword:
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON

            #pragma multi_compile_fog
            #pragma shader_feature_local _USEOUTLINE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float  _OutlineWidth;
                float  _UseOutline;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                // Required for GPU instancing / stereo instancing
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float  fogFactor   : TEXCOORD0;
                // Required to write the eye index to SV_RenderTargetArrayIndex
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings OutlineVert(Attributes IN)
            {
                Varyings OUT;
                // Set up instance ID and stereo eye index
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                #ifdef _USEOUTLINE_ON
                    float3 normalWS  = TransformObjectToWorldNormal(IN.normalOS);
                    float3 posWS     = TransformObjectToWorld(IN.positionOS.xyz);
                    posWS           += normalWS * _OutlineWidth;
                    OUT.positionHCS  = TransformWorldToHClip(posWS);
                #else
                    OUT.positionHCS  = float4(0, 0, 0, 0);
                #endif

                OUT.fogFactor = ComputeFogFactor(OUT.positionHCS.z);
                return OUT;
            }

            half4 OutlineFrag(Varyings IN) : SV_Target
            {
                // Required in fragment for stereo instancing
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                half3 col = MixFog(_OutlineColor.rgb, IN.fogFactor);
                return half4(col, 1);
            }
            ENDHLSL
        }

        // ─────────────────────────────────────────────
        // PASS 2 – Main toon lighting (ForwardLit)
        // ─────────────────────────────────────────────
        Pass
        {
            Name "ToonForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Cull Back

            HLSLPROGRAM
            #pragma vertex   ToonVert
            #pragma fragment ToonFrag

            // ── Critical VR pragmas ───────────────────
            #pragma multi_compile_instancing
            #pragma instancing_options renderingLayer
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO STEREO_INSTANCING_ON STEREO_MULTIVIEW_ON

            // URP shadow keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;

                float4 _ShadowColor;
                float  _ShadowThreshold;
                float  _ShadowSmoothness;
                float  _ShadowBands;

                float4 _SpecularColor;
                float  _SpecularThreshold;
                float  _SpecularSmoothness;
                float  _Glossiness;

                float4 _RimColor;
                float  _RimThreshold;
                float  _RimSmoothness;
                float  _RimAmount;

                float4 _OutlineColor;
                float  _OutlineWidth;
                float  _UseOutline;
            CBUFFER_END

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
                float3 positionWS  : TEXCOORD0;
                float3 normalWS    : TEXCOORD1;
                float2 uv          : TEXCOORD2;
                float4 shadowCoord : TEXCOORD3;
                float  fogFactor   : TEXCOORD4;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // ── Helpers ──────────────────────────────

            float CelBand(float value, float bands)
            {
                return floor(value * bands) / bands;
            }

            float ToonStep(float threshold, float smoothness, float value)
            {
                return smoothstep(threshold - smoothness, threshold + smoothness, value);
            }

            // ── Vertex ───────────────────────────────
            Varyings ToonVert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs posInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                VertexNormalInputs   nrmInputs = GetVertexNormalInputs(IN.normalOS);

                OUT.positionHCS = posInputs.positionCS;
                OUT.positionWS  = posInputs.positionWS;
                OUT.normalWS    = nrmInputs.normalWS;
                OUT.uv          = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.shadowCoord = GetShadowCoord(posInputs);
                OUT.fogFactor   = ComputeFogFactor(posInputs.positionCS.z);
                return OUT;
            }

            // ── Fragment ─────────────────────────────
            half4 ToonFrag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                float3 N = normalize(IN.normalWS);
                float3 V = normalize(GetCameraPositionWS() - IN.positionWS);

                Light mainLight = GetMainLight(IN.shadowCoord);
                float3 L        = normalize(mainLight.direction);
                float  shadow   = mainLight.shadowAttenuation;

                // ── Diffuse / cel bands ───────────────
                float NdotL     = dot(N, L) * 0.5 + 0.5;
                float bandedNdL = CelBand(NdotL, _ShadowBands);
                float diffuse   = ToonStep(_ShadowThreshold + 0.5, _ShadowSmoothness, bandedNdL * shadow);

                half3 albedo    = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv).rgb * _BaseColor.rgb;
                half3 litColor  = lerp(_ShadowColor.rgb * albedo, albedo, diffuse);
                litColor       *= mainLight.color;

                // ── Specular ─────────────────────────
                float3 H     = normalize(L + V);
                float  NdotH = pow(max(dot(N, H), 0.0), _Glossiness);
                float  spec  = ToonStep(_SpecularThreshold, _SpecularSmoothness, NdotH) * diffuse;
                litColor    += _SpecularColor.rgb * spec * mainLight.color;

                // ── Rim ──────────────────────────────
                float NdotV = 1.0 - dot(N, V);
                float rim   = ToonStep(_RimThreshold, _RimSmoothness, NdotV) * _RimAmount * diffuse;
                litColor   += _RimColor.rgb * rim * mainLight.color;

                // ── Fog ──────────────────────────────
                litColor = MixFog(litColor, IN.fogFactor);

                return half4(litColor, 1.0);
            }
            ENDHLSL
        }

        // Shadow caster pass (so this object casts shadows)
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
