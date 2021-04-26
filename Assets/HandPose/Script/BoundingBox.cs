using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// Axis aligned bounding box structure used to track the hand region
//
readonly struct BoundingBox
{
    #region Storage member

    public float2 Min { get; }
    public float2 Max { get; }

    #endregion

    #region Accessor with calculation

    public float2 Center => (Min + Max) * 0.5f;
    public float2 Extent => (Max - Min) * 0.5f;
    public float4 AsFloat4 => math.float4(Min, Max);

    public bool IsZero => math.any(Min == 0) && math.any(Max == 0);

    public float4x4 CropMatrix
      => math.mul(float4x4.Translate(math.float3(Min, 0)),
                  float4x4.Scale(math.float3(Max - Min, 1)));

    public BoundingBox Squarified
      => BoundingBox.CenterExtent(Center, math.cmax(Extent));

    #endregion

    #region Constructors and factory methods

    public BoundingBox(float2 min, float2 max)
      => (Min, Max) = (min, max);

    public BoundingBox(float4 v)
      => (Min, Max) = (v.xy, v.zw);

    public BoundingBox(in MediaPipe.BlazePalm.PalmDetector.Detection d)
      => (Min, Max) = (d.center - d.extent * 0.5f, d.center + d.extent * 0.5f);

    public static BoundingBox CenterExtent(float2 center, float2 extent)
      => new BoundingBox(center - extent, center + extent);

    #endregion

    #region Operator overloading

    public static BoundingBox operator * (BoundingBox b, float scale)
      => CenterExtent(b.Center, b.Extent * scale);

    #endregion

    #region Math operations

    public static float CalculateIOU(BoundingBox b1, BoundingBox b2)
    {
        var area0 = (b1.Max.x - b1.Min.x) * (b1.Max.y - b1.Min.y);
        var area1 = (b2.Max.x - b2.Min.x) * (b2.Max.y - b2.Min.y);

        var p0 = math.max(b1.Min, b2.Min);
        var p1 = math.min(b1.Max, b2.Max);
        float areaInner = math.max(0, p1.x - p0.x) * math.max(0, p1.y - p0.y);

        return areaInner / (area0 + area1 - areaInner);
    }

    #endregion
}

} // namespace MediaPipe.HandPose
