using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public GameObject[] imageToToggle; // Reference to the UI image

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            Show();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        { 
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject g in imageToToggle)
        {
            if (g != null)
            {
                g.SetActive(true); // Show the UI image
            }
        }
    }

    private void Hide()
    {
        foreach (GameObject g in imageToToggle)
        {
            if (g != null)
            {
                g.SetActive(false); // Hide the UI image
            }
        }
    }
}