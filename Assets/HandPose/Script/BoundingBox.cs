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

    public static BoundingBox CenterExtent(float2 center, float2 extent)
      => new BoundingBox(center - extent, center + extent);

    #endregion

    #region Operator overloading

    public static BoundingBox operator * (BoundingBox b, float scale)
      => CenterExtent(b.Center, b.Extent * scale);

    #endregion
}

} // namespace MediaPipe.HandPose
