Shader "CleanableBlit"
{
    Properties
    {
        _CollisionCount ("Collision Count", Integer) = 0
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
            
            uniform float4 _CollisionUVs[512];
            int _CollisionCount;
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_TARGET
            {
                for (int i = 0; i < _CollisionCount; i++)
                {
                    float2 uv = _CollisionUVs[i].xy;
                    float dist = distance(IN.uv, uv);
                    
                    if (dist < 0.005)
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