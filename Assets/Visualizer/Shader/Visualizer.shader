Shader "Hidden/MediaPipe/HandPose/Visualizer"
{
    CGINCLUDE

    #include "UnityCG.cginc"

    StructuredBuffer<float4> _Vertices;
    float4x4 _Xform;

    float4 Vertex(uint vid : SV_VertexID,
                  uint iid : SV_InstanceID) : SV_Position
    {
        float2 p = _Vertices[iid + 1] - 0.5;

        const float size = 0.02;
        p.x += size * lerp(-1, 1, vid == 1) * (vid < 2);
        p.y += size * lerp(-1, 1, vid == 3) * (vid > 1);

        return UnityObjectToClipPos(mul(_Xform, float4(p, 0, 1)));
    }

    float4 Fragment(float4 position : SV_Position,
                    float4 color : COLOR) : SV_Target
    {
        return float4(1, 1, 1, 0.9);
    }

    ENDCG

    SubShader
    {
        ZWrite Off ZTest Always Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
