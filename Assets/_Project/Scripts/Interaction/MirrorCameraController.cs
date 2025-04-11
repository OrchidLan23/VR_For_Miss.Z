using UnityEngine;

public class MirrorCameraController : MonoBehaviour
{
    public Transform mirrorPlane;      // 镜子的位置和朝向
    public Transform playerCamera;     // XR Rig 中的 Main Camera 或 HMD

    void LateUpdate()
    {
        if (mirrorPlane == null || playerCamera == null) return;

        // 从玩家摄像机位置相对于镜子做对称转换
        Vector3 mirrorNormal = mirrorPlane.forward;
        Vector3 toCamera = playerCamera.position - mirrorPlane.position;
        Vector3 reflectedPos = playerCamera.position - 2 * Vector3.Dot(toCamera, mirrorNormal) * mirrorNormal;

        transform.position = reflectedPos;

        // 计算旋转（朝向对称）
        Vector3 forward = Vector3.Reflect(playerCamera.forward, mirrorNormal);
        Vector3 up = Vector3.Reflect(playerCamera.up, mirrorNormal);
        transform.rotation = Quaternion.LookRotation(forward, up);
    }
}
