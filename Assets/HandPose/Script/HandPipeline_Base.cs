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

    ResourceSet _resources;
    (PalmDetector palm, HandLandmarkDetector landmark) _detector;
    (ComputeBuffer crop, ComputeBuffer region, ComputeBuffer filter) _buffer;

    #endregion

    #region Object allocation/deallocation

    void AllocateObjects(ResourceSet resources)
    {
        _resources = resources;

        _detector = (new PalmDetector(_resources.blazePalm),
                     new HandLandmarkDetector(_resources.handLandmark));

        // Size of the crop buffer
        var cropBufferLength = 3 * CropSize * CropSize;

        // Size of HandRegion struct defined in HandRegion.hlsl
        var regionBufferSize = sizeof(float) * 24;

        // Key point count
        var keyCount = HandLandmarkDetector.VertexCount;

        _buffer = (new ComputeBuffer(cropBufferLength, sizeof(float)),
                   new ComputeBuffer(1, regionBufferSize),
                   new ComputeBuffer(keyCount * 2, sizeof(float) * 4));
    }

    void DeallocateObjects()
    {
        _detector.palm.Dispose();
        _detector.landmark.Dispose();
        _buffer.crop.Dispose();
        _buffer.region.Dispose();
        _buffer.filter.Dispose();
    }

    #endregion
}

} // namespace MediaPipe.HandPose
