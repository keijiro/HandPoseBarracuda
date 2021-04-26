using MediaPipe.BlazePalm;
using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// Hand region tracker class
//

sealed class HandRegion
{
    #region Exposed properties

    public float4x4 RotationMatrix { get; private set; }
    public float4x4 CropMatrix { get; private set; }

    #endregion

    #region Public methods

    public void Update(in PalmDetector.Detection palm)
    {
        var box = new BoundingBox(palm) * 3;
        var angle = MathUtil.Angle(palm.middle - palm.wrist) - math.PI / 2;
        var offset = math.float3(0, 0.15f, 0);
        RotationMatrix = MathUtil.ZRotateAtCenter(angle);
        CropMatrix = MathUtil.Mul(box.CropMatrix,
                                  RotationMatrix,
                                  float4x4.Translate(offset));
    }

    #endregion
}

} // namespace MediaPipe.HandPose
