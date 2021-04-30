using UnityEngine;

namespace MediaPipe.HandPose {

//
// Image processing part of the hand pipeline class
//

partial class HandPipeline
{
    void RunPipeline(Texture input)
    {
        var cs = _resources.compute;

        // Palm detection
        _detector.palm.ProcessImage(input);

        // Hand region bounding box update
        cs.SetFloat("_bbox_dt", Time.deltaTime);
        cs.SetBuffer(0, "_bbox_count", _detector.palm.CountBuffer);
        cs.SetBuffer(0, "_bbox_palm", _detector.palm.DetectionBuffer);
        cs.SetBuffer(0, "_bbox_region", _buffer.region);
        cs.Dispatch(0, 1, 1, 1);

        // Hand region cropping
        cs.SetTexture(1, "_crop_input", input);
        cs.SetBuffer(1, "_crop_region", _buffer.region);
        cs.SetBuffer(1, "_crop_output", _buffer.crop);
        cs.Dispatch(1, CropSize / 8, CropSize / 8, 1);

        // Hand landmark detection
        _detector.landmark.ProcessImage(_buffer.crop);

        // Key point postprocess
        cs.SetFloat("_post_dt", Time.deltaTime);
        cs.SetBuffer(2, "_post_input", _detector.landmark.OutputBuffer);
        cs.SetBuffer(2, "_post_region", _buffer.region);
        cs.SetBuffer(2, "_post_output", _buffer.filter);
        cs.Dispatch(2, 1, 1, 1);

        // Read cache invalidation
        InvalidateReadCache();
    }
}

} // namespace MediaPipe.HandPose
