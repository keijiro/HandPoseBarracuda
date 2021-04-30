using UnityEditor;
using UnityEngine;

namespace MediaPipe.HandPose {

[CustomEditor(typeof(WebcamInput))]
sealed class WebcamInputEditor : Editor
{
    static readonly GUIContent SelectLabel = new GUIContent("Select");

    SerializedProperty _deviceName;
    SerializedProperty _resolution;
    SerializedProperty _frameRate;
    SerializedProperty _dummyImage;

    void OnEnable()
    {
        _deviceName = serializedObject.FindProperty("_deviceName");
        _resolution = serializedObject.FindProperty("_resolution");
        _frameRate = serializedObject.FindProperty("_frameRate");
        _dummyImage = serializedObject.FindProperty("_dummyImage");
    }

    void ShowDeviceSelector(Rect rect)
    {
        var menu = new GenericMenu();

        foreach (var device in WebCamTexture.devices)
            menu.AddItem(new GUIContent(device.name), false,
                         () => { serializedObject.Update();
                                 _deviceName.stringValue = device.name;
                                 serializedObject.ApplyModifiedProperties(); });

        menu.DropDown(rect);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(_deviceName);

        var rect = EditorGUILayout.GetControlRect(false, GUILayout.Width(60));
        if (EditorGUI.DropdownButton(rect, SelectLabel, FocusType.Keyboard))
            ShowDeviceSelector(rect);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_resolution);
        EditorGUILayout.PropertyField(_frameRate);
        EditorGUILayout.PropertyField(_dummyImage);

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace MediaPipe.HandPose
