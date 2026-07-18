using UnityEngine;
using Unity.Cinemachine;

public class CameraScaleFollower : MonoBehaviour
{
    [Header("References")]
    public ScaleController scaleController;
    public CinemachineThirdPersonFollow thirdPersonFollow;

    [Header("Distance Settings")]
    public float baseCameraDistance = 3f;
    public float distanceMultiplier = 1.2f;
    public float smoothSpeed = 3f;

    void LateUpdate()
    {
        if (scaleController == null || thirdPersonFollow == null) return;

        float currentScale = scaleController.GetCurrentScale();

        float targetDistance = Mathf.Max(baseCameraDistance + (currentScale - 1f) * distanceMultiplier, 1.5f);
        thirdPersonFollow.CameraDistance = Mathf.Lerp(
            thirdPersonFollow.CameraDistance,
            targetDistance,
            Time.deltaTime * smoothSpeed
        );
    }
}