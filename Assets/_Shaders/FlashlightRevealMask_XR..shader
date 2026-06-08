Shader "Custom/FlashlightRevealMask_XR"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Geometry-1"
            "RenderType"="Opaque"
        }

        Pass
        {
            Name "RevealMask"
            Tags { "LightMode"="UniversalForward" }

            ColorMask 0
            ZWrite On
            ZTest LEqual
            Cull Back

            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ UNITY_SINGLE_PASS_STEREO

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _FlashlightPosWS;
            float4 _FlashlightDirWS;
            float  _FlashlightCosHalfAngle;
            float  _FlashlightRange;

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS  : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs pos = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = pos.positionCS;
                OUT.positionWS = pos.positionWS;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float3 L2P = IN.positionWS - _FlashlightPosWS.xyz;
                float dist = length(L2P);
                float3 dir = normalize(L2P);

                float angle = dot(dir, normalize(_FlashlightDirWS.xyz));
                if (angle < _FlashlightCosHalfAngle) discard;
                if (dist > _FlashlightRange) discard;

                return 0;
            }
            ENDHLSL
        }
    }
}
