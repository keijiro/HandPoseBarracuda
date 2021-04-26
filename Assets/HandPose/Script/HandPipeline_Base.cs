using MediaPipe.BlazePalm;
using MediaPipe.HandLandmark;
using UnityEngine;

namespace MediaPipe.HandPose {

//
// Basic implementation of the hand pipeline class
//

sealed partial class HandPipeline : System.IDisposable
{
    #region Private objects

    ResourceSet _resources;

    (PalmDetector palm, HandLandmarkDetector landmark) _detector;

    Material _preprocess;

    RenderTexture _cropRT;

    ComputeBuffer _postBuffer;

    #endregion

    #region Object allocation/deallocation

    void AllocateObjects(ResourceSet resources)
    {
        _resources = resources;

        _detector = (new PalmDetector(_resources.blazePalm),
                     new HandLandmarkDetector(_resources.handLandmark));

        _preprocess = new Material(_resources.preprocessShader);

        _cropRT = new RenderTexture(224, 224, 0);

        var vcount = HandLandmarkDetector.VertexCount;
        _postBuffer = new ComputeBuffer(vcount * 2, sizeof(float) * 4);
    }

    void DeallocateObjects()
    {
        _detector.palm.Dispose();
        _detector.landmark.Dispose();

        Object.Destroy(_preprocess);

        Object.Destroy(_cropRT);

        _postBuffer.Dispose();
    }

    #endregion
}

} // namespace MediaPipe.HandPose
