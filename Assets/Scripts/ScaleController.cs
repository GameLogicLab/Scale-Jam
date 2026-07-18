using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    public float minScale = 0.4f;
    public float maxScale = 3.0f;
    public float scaleSpeed = 1.0f;

    [Header("Movement Settings (scaled with size)")]
    public float baseMoveSpeed = 6f;
    public float baseJumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Push Force (Giant only)")]
    public float basePushForce = 5f;

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "Speed";
    public string groundedParam = "IsGrounded";

    [Header("Strafe Turning")]
    public float turnSpeed = 15f; // how fast player rotates to face A/D/W/S direction

    private CharacterController controller;
    private float currentScale = 1.0f;
    private Vector3 velocity;
    private bool isGrounded;
    private float lastMoveMagnitude = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentScale = 1.0f;
        transform.localScale = Vector3.one * currentScale;

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update()
    {
        HandleScaling();
        HandleMovement();
        HandleAnimation();
    }

    void HandleScaling()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.leftShiftKey.isPressed)
        {
            currentScale += scaleSpeed * Time.deltaTime;
        }
        else if (kb.leftCtrlKey.isPressed)
        {
            currentScale -= scaleSpeed * Time.deltaTime;
        }

        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        Vector3 targetScale = Vector3.one * currentScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 8f);
    }

    void HandleMovement()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float t = (currentScale - minScale) / (maxScale - minScale);
        float speedMultiplier = Mathf.Lerp(1.5f, 0.6f, t);
        float jumpMultiplier = Mathf.Lerp(0.6f, 1.8f, t);

        float moveSpeed = baseMoveSpeed * speedMultiplier;
        float jumpHeight = baseJumpHeight * jumpMultiplier;

        float h = 0f;
        float v = 0f;
        if (kb.aKey.isPressed) h -= 1f;
        if (kb.dKey.isPressed) h += 1f;
        if (kb.sKey.isPressed) v -= 1f;
        if (kb.wKey.isPressed) v += 1f;

        Vector3 inputDir = new Vector3(h, 0, v);
        lastMoveMagnitude = inputDir.magnitude;

        if (inputDir.magnitude > 0.01f)
        {
            // Build world-space move direction from the player's OWN current facing
            // (not the camera) — this is separate from mouse-look rotation.
            Vector3 moveDir = (transform.right * h + transform.forward * v).normalized;

            // Rotate the player body to face whichever direction is being pressed
            // (W = face forward, A = face left, D = face right, S = face backward)
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        if (kb.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleAnimation()
    {
        if (animator == null) return;
        animator.SetFloat(speedParam, lastMoveMagnitude);
        animator.SetBool(groundedParam, isGrounded);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            float pushMultiplier = Mathf.InverseLerp(1f, maxScale, currentScale);
            if (pushMultiplier > 0.1f)
            {
                Vector3 pushDir = hit.moveDirection;
                pushDir.y = 0;
                rb.AddForce(pushDir * basePushForce * pushMultiplier, ForceMode.Impulse);
            }
        }
    }

    public float GetCurrentScale() => currentScale;
}