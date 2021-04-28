Shader "Hidden/MediaPipe/HandPose/Preprocess"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "HandRegion.hlsl"

    sampler2D _MainTex;

    StructuredBuffer<HandRegion> _HandRegion;

    float4 Fragment(float4 vertex : SV_Position,
                    float2 uv : TEXCOORD0) : SV_Target
    {
        uv = mul(_HandRegion[0].cropMatrix, float4(uv, 0, 1)).xy;
        return tex2D(_MainTex, uv);
    }

    ENDCG

    SubShader
    {
        Cull Off ZTest Always ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment Fragment
            ENDCG
        }
    }
}
