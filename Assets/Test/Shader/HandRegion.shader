Shader "Hidden/MediaPipe/HandPose/Visualizer/HandRegion"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

    //
    // UI material for displaying a hand cropping region
    //

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "../../HandPose/Shader/HandRegion.hlsl"

    sampler2D _MainTex;
    StructuredBuffer<HandRegion> _HandRegion;

    void Vertex(float4 position : POSITION,
                float2 texCoord : TEXCOORD0,
                float4 color : COLOR,
                out float4 outPosition : SV_Position,
                out float2 outTexCoord : TEXCOORD0,
                out float4 outColor : COLOR)
    {
        float4x4 xform = _HandRegion[0].cropMatrix;
        outPosition = UnityObjectToClipPos(position);
        outTexCoord = mul(xform, float4(texCoord, 0, 1)).xy;
        outColor = color;
    }

    float4 Fragment(float4 position : SV_Position,
                    float2 texCoord : TEXCOORD0,
                    float4 color : COLOR) : SV_Target
    {
        return tex2D(_MainTex, texCoord) * color;
    }

    ENDCG

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
