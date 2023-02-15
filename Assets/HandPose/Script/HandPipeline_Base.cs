using MediaPipe.BlazePalm;
using MediaPipe.HandLandmark;
using UnityEngine;
using UnityEngine.Rendering;

namespace MediaPipe.HandPose {

//
// Basic implementation of the hand pipeline class
//

sealed partial class HandPipeline : System.IDisposable
{
    #region Private objects

    const int CropSize = HandLandmarkDetector.ImageSize;
    int InputWidth => _detector.palm.ImageSize;

    ResourceSet _resources;
    (PalmDetector palm, HandLandmarkDetector landmark) _detector;
    (ComputeBuffer region, ComputeBuffer filter) _buffer;
    GlobalKeyword _keywordNchw;

    #endregion

    #region Object allocation/deallocation

    void AllocateObjects(ResourceSet resources)
    {
        _resources = resources;

        _detector = (new PalmDetector(_resources.blazePalm),
                     new HandLandmarkDetector(_resources.handLandmark));

        var regionStructSize = sizeof(float) * 24;
        var filterBufferLength = HandLandmarkDetector.VertexCount * 2;

        _buffer = (new ComputeBuffer(1, regionStructSize),
                   new ComputeBuffer(filterBufferLength, sizeof(float) * 4));

        _keywordNchw = GlobalKeyword.Create("NCHW_INPUT");
        Shader.SetKeyword(_keywordNchw, _detector.palm.InputIsNCHW);
    }

    void DeallocateObjects()
    {
        _detector.palm.Dispose();
        _detector.landmark.Dispose();
        _buffer.region.Dispose();
        _buffer.filter.Dispose();
    }

    #endregion
}

} // namespace MediaPipe.HandPose
