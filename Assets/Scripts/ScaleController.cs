using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class ScaleController : MonoBehaviour
{
    [Header("Scale Settings")]
    public float minScale = 0.4f;      // Tiny size
    public float maxScale = 3.0f;      // Giant size
    public float scaleSpeed = 1.0f;    // How fast grow/shrink happens

    [Header("Movement Settings (scaled with size)")]
    public float baseMoveSpeed = 6f;
    public float baseJumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Push Force (Giant only)")]
    public float basePushForce = 5f;

    [Header("Input Actions")]
    public InputActionReference moveAction;      // Vector2 (WASD / Left Stick)
    public InputActionReference jumpAction;      // Button (Space)
    public InputActionReference growAction;      // Button (Left Shift, hold)
    public InputActionReference shrinkAction;    // Button (Left Ctrl, hold)

    private CharacterController controller;
    private float currentScale = 1.0f;
    private Vector3 velocity;
    private bool isGrounded;
    private Vector2 moveInput;

    void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        growAction.action.Enable();
        shrinkAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
        growAction.action.Disable();
        shrinkAction.action.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentScale = 1.0f;
        transform.localScale = Vector3.one * currentScale;
    }

    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        HandleScaling();
        HandleMovement();
    }

    void HandleScaling()
    {
        if (growAction.action.IsPressed())
        {
            currentScale += scaleSpeed * Time.deltaTime;
        }
        else if (shrinkAction.action.IsPressed())
        {
            currentScale -= scaleSpeed * Time.deltaTime;
        }

        currentScale = Mathf.Clamp(currentScale, minScale, maxScale);

        Vector3 targetScale = Vector3.one * currentScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 8f);
    }

    void HandleMovement()
    {
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

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (jumpAction.action.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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