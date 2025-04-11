using UnityEngine;

public class EditorCameraFixer : MonoBehaviour
{
    [Header("要调整的对象（TrackingSpace）")]
    public Transform trackingSpace;

    [Header("Editor 模拟摄像头偏移")]
    public float editorHeightY = 1.6f;

    void Start()
    {
#if UNITY_EDITOR
        if (trackingSpace != null)
        {
            Vector3 original = trackingSpace.localPosition;
            trackingSpace.localPosition = new Vector3(original.x, editorHeightY, original.z);
        }
#endif
    }
}
