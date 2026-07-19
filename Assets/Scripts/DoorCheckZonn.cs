using UnityEngine;

public class DoorCheckZone : MonoBehaviour
{
    // Place this on a Box Collider (Is Trigger = true) right in front of the door.
    // When the player enters, it checks with KeyManager if all keys are collected.

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
            Debug.Log("Door Open! All keys collected (" + collected + "/" + needed + ")");
        }
        else
        {
            Debug.Log("Door Locked. Keys collected: " + collected + "/" + needed);
        }
    }
}