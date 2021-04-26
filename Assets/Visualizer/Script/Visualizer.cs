using UnityEngine;
using Unity.Mathematics;
using UI = UnityEngine.UI;

namespace MediaPipe.HandPose {

public sealed class Visualizer : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] WebcamInput _webcam = null;
    [Space]
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] Shader _shader = null;
    [Space]
    [SerializeField] UI.RawImage _mainUI = null;
    [SerializeField] UI.RawImage _cropUI = null;

    #endregion

    #region Private members

    HandPipeline _pipeline;
    Material _material;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _pipeline = new HandPipeline(_resources);
        _material = new Material(_shader);
    }

    void OnDestroy()
    {
        _pipeline.Dispose();
        Destroy(_material);
    }

    void LateUpdate()
    {
        // Processing on the hand pipeline
        _pipeline.ProcessImage(_webcam.Texture);

        // UI update
        _mainUI.texture = _webcam.Texture;
        _cropUI.texture = _pipeline.CroppedTexture;
    }

    void OnRenderObject()
    {
        var mv = float4x4.Translate(math.float3(-0.25f, 0, 0));
        _material.SetMatrix("_Xform", mv);
        _material.SetBuffer("_Vertices", _pipeline.RefinedVertexBuffer);

        // Key point circles
        _material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Triangles, 96, 21);

        // Bone lines
        _material.SetPass(1);
        Graphics.DrawProceduralNow(MeshTopology.Lines, 2, 4 * 5 + 1);
    }

    #endregion
}

} // namespace MediaPipe.HandPose
