using UnityEngine;
using UnityEngine.Events;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [Header("Settings")]
    public int totalKeysNeeded = 1; // set this to however many keys are in the level

    [Header("Events")]
    public UnityEvent onAllKeysCollected; // hook up the door-open animation/script here in Inspector

    private int keysCollected = 0;

    void Awake()
    {
        // Simple singleton so any key in the scene can call KeyManager.Instance.CollectKey()
        Instance = this;
    }

    public void CollectKey()
    {
        keysCollected++;
        Debug.Log($"Key collected: {keysCollected}/{totalKeysNeeded}");

        if (keysCollected >= totalKeysNeeded)
        {
            Debug.Log("All keys collected! Opening door...");
            onAllKeysCollected?.Invoke();
        }
    }

    public int GetKeysCollected() => keysCollected;
}