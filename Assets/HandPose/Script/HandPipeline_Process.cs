using UnityEngine;
using Unity.Mathematics;

namespace MediaPipe.HandPose {

//
// Image processing part of the hand pipeline class
//

partial class HandPipeline
{
    void RunPipeline(Texture input)
    {
        var post = _resources.postprocessCompute;

        // Palm detection
        _detector.palm.ProcessImage(input);

        // Hand region update
        post.SetFloat("_bbox_dt", Time.deltaTime);
        post.SetBuffer(0, "_bbox_count", _detector.palm.CountBuffer);
        post.SetBuffer(0, "_bbox_palm", _detector.palm.DetectionBuffer);
        post.SetBuffer(0, "_bbox_region", _computeBuffer.region);
        post.Dispatch(0, 1, 1, 1);

        // Hand region cropping
        _preprocess.SetBuffer("_HandRegion", _computeBuffer.region);
        Graphics.Blit(input, _cropRT, _preprocess, 0);

        // Hand landmark detection
        _detector.landmark.ProcessImage(_cropRT);

        // Postprocess for hand mesh construction
        post.SetFloat("_mesh_dt", Time.deltaTime);
        post.SetBuffer(1, "_mesh_input", _detector.landmark.OutputBuffer);
        post.SetBuffer(1, "_mesh_region", _computeBuffer.region);
        post.SetBuffer(1, "_mesh_output", _computeBuffer.post);
        post.Dispatch(1, 1, 1, 1);
    }
}

} // namespace MediaPipe.HandPose
