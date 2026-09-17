Shader "CleanableBlit"
{
    Properties
    {
//        _TargetUV ("Target UV", Vector) = (0, 0, 0, 0)
        _TargetUVsCount ("Target UVs Count", Integer) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        Blend One One
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
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
            
            uniform float4 _TargetUVs[512];
            int _TargetUVsCount;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_TARGET
            {
                for (int i = 0; i < _TargetUVsCount; i++)
                {
                    float2 uv = _TargetUVs[i].xy;
                    float dist = distance(IN.uv, uv);
                    
                    if (dist < 0.004)
                    {
                        return 1;
                    }
                }
                
                return 0;
            }
            ENDHLSL
        }
    }
}