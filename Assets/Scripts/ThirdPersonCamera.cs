using UnityEngine;
using UnityEngine.InputSystem;

// SETUP (Unity hierarchy):
//
// CameraPivot                 <- STANDALONE object, NOT a child of the player
//   └── Main Camera           <- child of CameraPivot, LOCAL position handled by this script now
//
// This script goes on "CameraPivot". It follows the player's POSITION only,
// and rotates purely from mouse input — independent of player body rotation.
// Both height offset AND camera distance scale with the player's current size.

public class ThirdPersonCameraRig : MonoBehaviour
{
    [Header("References")]
    public Transform target;                // Drag Ranger (player) here
    public ScaleController scaleController; // Drag Ranger here too (reads current scale)
    public Transform cameraTransform;       // Drag the child "Main Camera" here

    [Header("Height Offset (scales with player size)")]
    public float baseHeightOffset = 1.6f;
    public float heightOffsetMultiplier = 1f;

    [Header("Distance (scales with player size)")]
    public float baseDistance = 4f;
    public float shoulderOffsetX = 0.5f; // right-shoulder offset
    public float distanceMultiplier = 1f;
    public float cameraSmoothSpeed = 15f;

    [Header("Look Sensitivity")]
    public float mouseSensitivity = 2f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Position Smoothing")]
    public float positionSmoothSpeed = 15f;

    private float yaw;
    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        HandleCursorToggle();
        HandleLook();
        FollowPosition();
        UpdateCameraDistance();
    }

    void HandleCursorToggle()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isLocked = Cursor.lockState == CursorLockMode.Locked;
            Cursor.lockState = isLocked ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isLocked;
        }
    }

    void HandleLook()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 delta = mouse.delta.ReadValue();

        yaw += delta.x * mouseSensitivity * Time.deltaTime * 10f;
        pitch -= delta.y * mouseSensitivity * Time.deltaTime * 10f;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    float GetCurrentScale()
    {
        return scaleController != null ? scaleController.GetCurrentScale() : 1f;
    }

    void FollowPosition()
    {
        float currentScale = GetCurrentScale();
        float heightOffset = baseHeightOffset * Mathf.Lerp(1f, currentScale, heightOffsetMultiplier);

        Vector3 desiredPosition = target.position + new Vector3(0f, heightOffset, 0f);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * positionSmoothSpeed);
    }

    void UpdateCameraDistance()
    {
        if (cameraTransform == null) return;

        float currentScale = GetCurrentScale();
        float distance = baseDistance * Mathf.Lerp(1f, currentScale, distanceMultiplier);

        // Local position: shoulder offset on X, pulled back on Z
        Vector3 targetLocalPos = new Vector3(shoulderOffsetX, 0f, -distance);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetLocalPos, Time.deltaTime * cameraSmoothSpeed);
    }
}