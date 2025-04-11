using UnityEngine;

public class PlayerZFollower : MonoBehaviour
{
    [Header("模型骨骼绑定")]
    public Transform headBone;
    public Transform leftHandBone;
    public Transform rightHandBone;

    [Header("手柄模式 - OVR 控制器位置源")]
    public Transform ovrHead;
    public Transform ovrLeftHand;
    public Transform ovrRightHand;

    [Header("手势模式 - OVR 手势追踪位置源")]
    public Transform handTrackingHead;
    public Transform handTrackingLeft;
    public Transform handTrackingRight;

    [Header("左手手指骨骼")]
    public Transform[] leftIndex;
    public Transform[] leftMiddle;
    public Transform[] leftRing;
    public Transform[] leftPinky;
    public Transform[] leftThumb;

    [Header("右手手指骨骼")]
    public Transform[] rightIndex;
    public Transform[] rightMiddle;
    public Transform[] rightRing;
    public Transform[] rightPinky;
    public Transform[] rightThumb;

    [Header("手指卷曲角度 (Local Euler)")]
    public Vector3 fingerCurlEuler = new Vector3(60f, 0f, 0f);

    private bool useHandTracking = false;
    private float leftGripValue = 0f;
    private float rightGripValue = 0f;

    void Update()
    {
        // ✅ 自动识别当前模式（手势 or 控制器）
        useHandTracking = OVRInput.IsControllerConnected(OVRInput.Controller.Hands);

        // ✅ 根据模式切换输入源
        Transform headSrc = useHandTracking ? handTrackingHead : ovrHead;
        Transform leftSrc = useHandTracking ? handTrackingLeft : ovrLeftHand;
        Transform rightSrc = useHandTracking ? handTrackingRight : ovrRightHand;

        // ✅ 同步头部
        if (headBone && headSrc)
        {
            headBone.position = headSrc.position;
            headBone.rotation = headSrc.rotation;
        }

        // ✅ 同步左手
        if (leftHandBone && leftSrc)
        {
            leftHandBone.position = leftSrc.position;
            leftHandBone.rotation = leftSrc.rotation;
        }

        // ✅ 同步右手
        if (rightHandBone && rightSrc)
        {
            rightHandBone.position = rightSrc.position;
            rightHandBone.rotation = rightSrc.rotation;
        }

        // ✅ 读取手柄 Grip 值（用于卷手指）
        if (!useHandTracking)
        {
            leftGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            rightGripValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        }
        else
        {
            // 暂不控制手势手指（Meta Hands 可接入实际追踪）
            leftGripValue = 0f;
            rightGripValue = 0f;
        }

        // ✅ 应用卷曲动作
        ApplyFingerPose(leftIndex, leftGripValue);
        ApplyFingerPose(leftMiddle, leftGripValue);
        ApplyFingerPose(leftRing, leftGripValue);
        ApplyFingerPose(leftPinky, leftGripValue);
        ApplyFingerPose(leftThumb, leftGripValue);

        ApplyFingerPose(rightIndex, rightGripValue);
        ApplyFingerPose(rightMiddle, rightGripValue);
        ApplyFingerPose(rightRing, rightGripValue);
        ApplyFingerPose(rightPinky, rightGripValue);
        ApplyFingerPose(rightThumb, rightGripValue);
    }

    void ApplyFingerPose(Transform[] joints, float grip)
    {
        Quaternion target = Quaternion.Euler(fingerCurlEuler);
        foreach (var joint in joints)
        {
            if (joint != null)
                joint.localRotation = Quaternion.Lerp(Quaternion.identity, target, grip);
        }
    }
}
