using UnityEngine;

public class TinyGapBlocker : MonoBehaviour
{
    [Header("Settings")]
    public float maxAllowedScale = 0.8f; // player must be at or below this scale to pass
    public ScaleController playerScaleController;

    private Collider blockerCollider;

    void Start()
    {
        blockerCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (playerScaleController == null) return;

        float currentScale = playerScaleController.GetCurrentScale();

        // If player is too big, this trigger acts as a solid wall (disable trigger = solid)
        // If player is small enough, disable the collider entirely so they pass through freely
        blockerCollider.enabled = currentScale > maxAllowedScale;
        blockerCollider.isTrigger = false; // solid when active, blocks the giant player
    }
}