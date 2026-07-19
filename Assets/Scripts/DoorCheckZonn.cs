using UnityEngine;

public class DoorCheckZone : MonoBehaviour
{
    [Header("UI")]
    public GameObject levelCompletePanel; // Drag your Level Complete UI Panel here (should be inactive by default)

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ScaleController>() == null) return; // only react to the player

        if (KeyManager.Instance == null)
        {
            Debug.LogWarning("DoorCheckZone: No KeyManager found in scene.");
            return;
        }

        int collected = KeyManager.Instance.GetKeysCollected();
        int needed = KeyManager.Instance.totalKeysNeeded;

        if (collected >= needed)
        {
            Debug.Log("Door Open! Level Complete (" + collected + "/" + needed + ")");
            ShowLevelCompleteUI();
        }
        else
        {
            Debug.Log("Door Locked. Keys collected: " + collected + "/" + needed);
        }
    }

    void ShowLevelCompleteUI()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);

            // Optional: pause the game while the panel is up
            Time.timeScale = 0f;

            // Unlock cursor so player can click UI buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}