using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject buttonToToggle;

    void Update()
    {
        // Check if the player is in range of the NPC and "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E) && buttonToToggle.activeSelf)
        {
            TriggerDialogue();
            buttonToToggle.SetActive(false);
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
