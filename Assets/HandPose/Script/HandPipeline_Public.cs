using UnityEngine;

namespace MediaPipe.HandPose {

//
// Public part of the hand pipeline class
//

partial class HandPipeline
{
    #region Accessors for detection results

    public ComputeBuffer VertexBuffer
      => _computeBuffer.post;

    #endregion

    #region Accessors for internal data

    public ComputeBuffer RawVertexBuffer
      => _detector.landmark.OutputBuffer;

    public Texture CroppedTexture
      => _cropRT;

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
