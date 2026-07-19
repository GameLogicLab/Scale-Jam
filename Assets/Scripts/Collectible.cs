using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Visual (optional)")]
    public float rotateSpeed = 90f; // degrees per second, for a nice spinning key effect
    public float bobHeight = 0.2f;
    public float bobSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Simple spin + bob animation so the key feels alive (no animation clip needed)
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        float newY = startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        // Detect player via ScaleController component (works regardless of tag)
        if (other.GetComponent<ScaleController>() != null)
        {
            KeyManager.Instance.CollectKey();
            Destroy(gameObject);
        }
    }
}