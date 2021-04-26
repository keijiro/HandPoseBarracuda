using MediaPipe.BlazePalm;
using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// Hand region tracker class
//

sealed class HandRegion
{
    #region Exposed properties

    public float4x4 CropMatrix { get; private set; }

    #endregion

    #region Internal state

    Float4Filter _box = new Float4Filter(2, 1.5f);
    FloatFilter _angle = new FloatFilter(1.3f, 1.5f);

    #endregion

    #region Public methods

    public void Step(in PalmDetector.Detection palm)
    {
        // Bounding box of the detected palm
        var box_min = palm.center - palm.extent * 0.5f;
        var box_max = palm.center + palm.extent * 0.5f;
        var box = new BoundingBox(box_min, box_max).Squarified * 3;

        // Palm angle
        var angle = MathUtil.Angle(palm.middle - palm.wrist) - math.PI / 2;

        // Low pass filter
        var t_e = UnityEngine.Time.deltaTime;
        _box = _box.Next(box.AsFloat4, t_e);
        _angle = _angle.Next(angle, t_e);

        // Filtered values
        box = new BoundingBox(_box.Value);
        angle = _angle.Value;

        // Region crop matrix update
        var offs = float4x4.Translate(math.float3(0, 0.15f, 0));
        var rot = MathUtil.ZRotateAtCenter(angle);
        CropMatrix = MathUtil.Mul(box.CropMatrix, rot, offs);
    }

    #endregion
}

} // namespace MediaPipe.HandPose
