Shader "Custom/StencilText"
{
    Properties
    {
        _MainTex ("Font Atlas", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 1
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            
            Stencil
            {
                Ref [_StencilID]
                Comp Equal
                Pass Keep
                Fail Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Texture2D _MainTex;
            SamplerState sampler_MainTex;
            float4 _Color;
            float _Cutoff;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half alpha = _MainTex.Sample(sampler_MainTex, i.uv).a;
                
                half smoothAlpha = smoothstep(_Cutoff - 0.05, _Cutoff + 0.05, alpha);
                
                half4 finalCol = i.color * _Color;
                finalCol.a *= smoothAlpha;
                
                return finalCol;
            }
            ENDHLSL
        }
    }
}