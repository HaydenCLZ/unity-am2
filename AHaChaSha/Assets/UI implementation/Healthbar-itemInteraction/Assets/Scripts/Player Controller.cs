using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float rotationspeed = 100f;

    Vector3 move;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * rotationspeed * Time.deltaTime;

        // Move the player forward or backward
        transform.Translate(0, 0, translation);
        //rotate the player
        transform.Rotate(0, rotation, 0);
    }
}
