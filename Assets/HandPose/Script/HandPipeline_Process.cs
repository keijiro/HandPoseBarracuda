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

        // Letterboxing scale factor
        var scale = new Vector2
          (Mathf.Max((float)input.height / input.width, 1),
           Mathf.Max(1, (float)input.width / input.height));

        // Image scaling and padding
        cs.SetInt("_spad_width", InputWidth);
        cs.SetVector("_spad_scale", scale);
        cs.SetTexture(0, "_spad_input", input);
        cs.SetBuffer(0, "_spad_output", _buffer.input);
        cs.Dispatch(0, InputWidth / 8, InputWidth / 8, 1);

        // Palm detection
        _detector.palm.ProcessImage(_buffer.input);

        // Hand region bounding box update
        cs.SetFloat("_bbox_dt", Time.deltaTime);
        cs.SetBuffer(1, "_bbox_count", _detector.palm.CountBuffer);
        cs.SetBuffer(1, "_bbox_palm", _detector.palm.DetectionBuffer);
        cs.SetBuffer(1, "_bbox_region", _buffer.region);
        cs.Dispatch(1, 1, 1, 1);

        // Hand region cropping
        cs.SetTexture(2, "_crop_input", input);
        cs.SetBuffer(2, "_crop_region", _buffer.region);
        cs.SetBuffer(2, "_crop_output", _buffer.crop);
        cs.Dispatch(2, CropSize / 8, CropSize / 8, 1);

        // Hand landmark detection
        _detector.landmark.ProcessImage(_buffer.crop);

        // Key point postprocess
        cs.SetFloat("_post_dt", Time.deltaTime);
        cs.SetFloat("_post_scale", scale.y);
        cs.SetBuffer(3, "_post_input", _detector.landmark.OutputBuffer);
        cs.SetBuffer(3, "_post_region", _buffer.region);
        cs.SetBuffer(3, "_post_output", _buffer.filter);
        cs.Dispatch(3, 1, 1, 1);

        // Read cache invalidation
        InvalidateReadCache();
    }
}

} // namespace MediaPipe.HandPose
