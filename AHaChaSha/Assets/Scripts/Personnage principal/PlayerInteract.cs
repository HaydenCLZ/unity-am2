using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Image imageToToggle; // Reference to the UI image
    private bool isNPCInRange = false; // Flag to track if NPC is in range

    void Update()
    {
        if (isNPCInRange)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("NPC"))
        {
            isNPCInRange = true; // Set flag to true when NPC is hit
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isNPCInRange = false; // Set flag to false when exiting NPC's collider
            Debug.Log("Player exited NPC range");
        }
    }

    private void Show()
    {
        if (imageToToggle != null)
        {
            imageToToggle.gameObject.SetActive(true); // Show the UI image
        }
    }

    private void Hide()
    {
        if (imageToToggle != null)
        {
            imageToToggle.gameObject.SetActive(false); // Hide the UI image
        }
    }
}