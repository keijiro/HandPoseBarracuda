using UnityEngine;
using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// Public part of the hand pipeline class
//

partial class HandPipeline
{
    #region Accessors for vertex buffers

    public ComputeBuffer RawVertexBuffer
      => _detector.landmark.OutputBuffer;

    public ComputeBuffer RefinedVertexBuffer
      => _postBuffer;

    #endregion

    #region Accessors for cropped textures

    public Texture CroppedTexture
      => _cropRT;

    #endregion

    #region Accessors for crop region matrices

    public float4x4 CropMatrix
      => _handRegion.CropMatrix;

    #endregion

    #region Public methods

    public HandPipeline(ResourceSet resources)
      => AllocateObjects(resources);

    public void Dispose()
      => DeallocateObjects();

    public void ProcessImage(Texture image)
      => RunPipeline(image);

    #endregion
}

} // namespace MediaPipe.HandPose
