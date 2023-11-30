using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Update()
    {
        // Check if the player is in range of the NPC and "E" key is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
