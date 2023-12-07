using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int bonusHealth = 20;
    private Main_Character_Health player;

    void Start()
    {
        // find the Player script in the scene
        player = FindObjectOfType<Main_Character_Health>();
        if (player == null)
        {
            Debug.LogError("no player :("); //debugging
        }
    }

    // OnTriggerEnter is called when a trigger collider enters the item's collider
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && player != null) //make sure to put properly the tag "player" to the character we use.
        {
            if (player.currentHealth < player.maxHealth)
            {

                Destroy(gameObject); // destroy the healing item
                player.Heal(bonusHealth);
            }
        }
    }
}
