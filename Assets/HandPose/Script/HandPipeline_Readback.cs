using UnityEngine;

namespace MediaPipe.HandPose {

//
// GPU to CPU readback implementation of the hand pipeline class
//

sealed partial class HandPipeline
{
    Vector4[] _readCache = new Vector4[KeyPointCount];
    bool _readFlag;

    Vector4[] ReadCache
      => _readFlag ? _readCache : UpdateReadCache();

    Vector4[] UpdateReadCache()
    {
        _buffer.filter.GetData(_readCache, 0, 0, KeyPointCount);
        _readFlag = true;
        return _readCache;
    }

    void InvalidateReadCache()
      => _readFlag = false;
}

} // namespace MediaPipe.HandPose
