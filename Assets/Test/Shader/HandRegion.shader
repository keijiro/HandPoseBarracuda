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

    #define IMAGE_WIDTH 224

    sampler2D _MainTex;
    StructuredBuffer<float> _Image;

    void Vertex(float4 position : POSITION,
                float2 texCoord : TEXCOORD0,
                float4 color : COLOR,
                out float4 outPosition : SV_Position,
                out float2 outTexCoord : TEXCOORD0,
                out float4 outColor : COLOR)
    {
        outPosition = UnityObjectToClipPos(position);
        outTexCoord = float2(texCoord.x, 1 - texCoord.y);
        outColor = color;
    }

    float4 Fragment(float4 position : SV_Position,
                    float2 texCoord : TEXCOORD0,
                    float4 color : COLOR) : SV_Target
    {
        uint2 p = texCoord * IMAGE_WIDTH;
#ifdef NCHW_INPUT
        uint offs = p.y * IMAGE_WIDTH + p.x;
        uint stride = IMAGE_WIDTH * IMAGE_WIDTH;
#else
        uint offs = (p.y * IMAGE_WIDTH + p.x) * 3;
        uint stride = 1;
#endif
        float r = _Image[offs + 0 * stride];
        float g = _Image[offs + 1 * stride];
        float b = _Image[offs + 2 * stride];
        return float4(r, g, b, 1)* color;
    }

    ENDCG

    SubShader
    {
        Tags { "Queue"="Transparent" }
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma multi_compile _ NCHW_INPUT
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
