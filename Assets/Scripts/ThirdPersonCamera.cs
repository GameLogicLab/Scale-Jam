using UnityEngine;
using UnityEngine.InputSystem;

// STANDARD SETUP (do this in the Unity Editor hierarchy first):
//
// Ranger                      <- player root, this script goes here
//   └── CameraPivot           <- empty GameObject, positioned at chest height (e.g. local pos 0, 1.6, 0)
//         └── Main Camera     <- positioned BEHIND the pivot (e.g. local pos 0.5, 0, -4)
//                                 (0.5 = shoulder offset, -4 = distance behind)
//
// This script rotates "Ranger" (yaw, left/right) and "CameraPivot" (pitch, up/down).
// Because the Camera is a child of CameraPivot, it automatically orbits correctly —
// no manual position math needed. This is the standard, reliable pattern.

public class ThirdPersonCameraRig : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPivot; // Drag the CameraPivot child object here

    [Header("Look Sensitivity")]
    public float mouseSensitivity = 2f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    private float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }

    void LateUpdate()
    {
        HandleCursorToggle();
        HandleLook();
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

        // Yaw: rotate the PLAYER body left/right.
        // This automatically makes the camera orbit too (since it's a child),
        // AND makes the player face the direction they're looking.
        transform.Rotate(Vector3.up * delta.x * mouseSensitivity * Time.deltaTime * 10f);

        // Pitch: rotate only the pivot up/down (camera looks up/down, player body stays upright).
        pitch -= delta.y * mouseSensitivity * Time.deltaTime * 10f;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}