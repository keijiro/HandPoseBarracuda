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

    const int CropSize = HandLandmarkDetector.ImageSize;

    int InputWidth => _detector.palm.ImageSize;

    ResourceSet _resources;

    (PalmDetector palm, HandLandmarkDetector landmark) _detector;

    (ComputeBuffer input, ComputeBuffer crop,
     ComputeBuffer region, ComputeBuffer filter) _buffer;

    #endregion

    #region Object allocation/deallocation

    void AllocateObjects(ResourceSet resources)
    {
        _resources = resources;

        _detector = (new PalmDetector(_resources.blazePalm),
                     new HandLandmarkDetector(_resources.handLandmark));

        var inputBufferLength = 3 * InputWidth * InputWidth;
        var cropBufferLength = 3 * CropSize * CropSize;
        var regionStructSize = sizeof(float) * 24;
        var filterBufferLength = HandLandmarkDetector.VertexCount * 2;

        _buffer = (new ComputeBuffer(inputBufferLength, sizeof(float)),
                   new ComputeBuffer(cropBufferLength, sizeof(float)),
                   new ComputeBuffer(1, regionStructSize),
                   new ComputeBuffer(filterBufferLength, sizeof(float) * 4));
    }

    void DeallocateObjects()
    {
        _detector.palm.Dispose();
        _detector.landmark.Dispose();
        _buffer.input.Dispose();
        _buffer.crop.Dispose();
        _buffer.region.Dispose();
        _buffer.filter.Dispose();
    }

    #endregion
}

} // namespace MediaPipe.HandPose
