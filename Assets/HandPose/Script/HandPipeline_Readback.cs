using UnityEngine;
using UnityEngine.Rendering;

namespace MediaPipe.HandPose {

//
// GPU to CPU readback implementation of the hand pipeline class
//

sealed partial class HandPipeline
{
    #region Read cache operations

    Vector4[] _readCache = new Vector4[KeyPointCount];
    bool _readFlag;

    Vector4[] ReadCache
      => (_readFlag || UseAsyncReadback) ? _readCache : UpdateReadCache();

    Vector4[] UpdateReadCache()
    {
        _buffer.filter.GetData(_readCache, 0, 0, KeyPointCount);
        _readFlag = true;
        return _readCache;
    }

    void InvalidateReadCache()
    {
        if (UseAsyncReadback)
            AsyncGPUReadback.Request
              (_buffer.filter, ReadbackBytes, 0, ReadbackCompleteAction);
        else
            _readFlag = false;
    }

    #endregion

    #region GPU async operation callback

    const int ReadbackBytes = KeyPointCount * sizeof(float) * 4;

    System.Action<AsyncGPUReadbackRequest> ReadbackCompleteAction
      => OnReadbackComplete;

    void OnReadbackComplete(AsyncGPUReadbackRequest req)
      => req.GetData<Vector4>().CopyTo(_readCache);

    #endregion
}

} // namespace MediaPipe.HandPose
