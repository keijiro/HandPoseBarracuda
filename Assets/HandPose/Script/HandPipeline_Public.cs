using UnityEngine;

namespace MediaPipe.HandPose {

//
// Public part of the hand pipeline class
//

partial class HandPipeline
{
    #region Public accessors

    public ComputeBuffer KeyPointBuffer
      => _buffer.filter;

    public ComputeBuffer HandRegionBuffer
      => _buffer.region;

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
