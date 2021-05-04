Shader "Hidden/MediaPipe/HandPose/Visualizer/KeyPoint"
{
    CGINCLUDE

    //
    // Material for visualizing hand key points and their skeletal structure
    //

    #include "UnityCG.cginc"

    StructuredBuffer<float4> _KeyPoints;

    // Coloring function
    float3 DepthToColor(float z)
    {
        float3 c = lerp(1, float3(0, 0, 1), saturate(z * 2));
        return lerp(c, float3(1, 0, 0), saturate(z * -2));
    }

    //
    // Vertex shader for key points (circles)
    //
    void VertexKeys(uint vid : SV_VertexID,
                    uint iid : SV_InstanceID,
                    out float4 position : SV_Position,
                    out float4 color : COLOR)
    {
        float3 p = _KeyPoints[iid].xyz;

        uint fan = vid / 3;
        uint segment = vid % 3;

        float theta = (fan + segment - 1) * UNITY_PI / 16;
        float radius = (segment > 0) * 0.08 * (max(0, -p.z) + 0.1);
        p.xy += float2(cos(theta), sin(theta)) * radius;

        position = UnityObjectToClipPos(float4(p, 1));
        color = float4(DepthToColor(p.z), 0.8);
    }

    //
    // Vertex shader for bones (line segments)
    //
    void VertexBones(uint vid : SV_VertexID,
                     uint iid : SV_InstanceID,
                     out float4 position : SV_Position,
                     out float4 color : COLOR)
    {
        uint finger = iid / 4;
        uint segment = iid % 4;

        uint i = min(4, finger) * 4 + segment + vid;
        uint root = finger > 1 && finger < 5 ? i - 3 : 0;

        i = max(segment, vid) == 0 ? root : i;

        float3 p = _KeyPoints[i].xyz;

        position = UnityObjectToClipPos(float4(p, 1));
        color = float4(DepthToColor(p.z), 0.8);
    }

    //
    // Common fragment shader (simple fill)
    //
    float4 Fragment(float4 position : SV_Position,
                    float4 color : COLOR) : SV_Target
    {
        return color;
    }

    ENDCG

    SubShader
    {
        ZWrite Off ZTest Always Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex VertexKeys
            #pragma fragment Fragment
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex VertexBones
            #pragma fragment Fragment
            ENDCG
        }
    }
}
