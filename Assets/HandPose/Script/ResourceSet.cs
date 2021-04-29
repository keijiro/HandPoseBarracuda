using UnityEngine;
using Unity.Barracuda;

namespace MediaPipe.HandPose {

//
// ScriptableObject class used to hold references to internal assets
//
[CreateAssetMenu(fileName = "ResourceSet",
                 menuName = "ScriptableObjects/MediaPipe/HandPose Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public MediaPipe.BlazePalm.ResourceSet blazePalm;
    public MediaPipe.HandLandmark.ResourceSet handLandmark;
    public ComputeShader compute;
}

} // namespace MediaPipe.HandPose
