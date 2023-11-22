using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
            if (hit.gameObject.CompareTag("NPC"))

            {
                Debug.Log("Joakim's cool");
            }
        }
}