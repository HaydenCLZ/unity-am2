using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleControler : MonoBehaviour
{
    public float speed = 50.0f;
    public float rotationSpeed = 100.0f;
    int collide = 0;
    bool moving = false;
    bool jumping = false;
    Vector3 inertie = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        collide++;
    }

    // Gets called when the object exits the collision
    void OnCollisionExit(Collision collision)
    {
        collide--;
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 side = transform.TransformDirection(Vector3.right);
        inertie *= (0.95f - Time.deltaTime) * (0.9f-Mathf.Min(0.1f, collide));
        Vector3 movement = Vector3.zero;
        moving = false;
        jumping = false;
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X") * rotationSpeed, 0);
        Vector2 keyInput = Vector2.zero;
        if (collide != 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                keyInput.x++;
                moving = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                keyInput.x--;
                moving = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                keyInput.y++;
                moving = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                keyInput.y--;
                moving = true;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jumping = true;
                moving = true;
            }
        }
        forward *= speed * Time.deltaTime * keyInput.x * -4;
        side *= speed * Time.deltaTime * keyInput.y * -1;
        movement = forward + side;  
        if(jumping)
        {
            movement = movement + Vector3.up*10.0f;
        }
        if (moving)
        {
            inertie = movement;
        }
        else
        {
            movement = inertie;
        }
        transform.position += movement;
        transform.Rotate(Vector3.up, mouseInput.x * rotationSpeed);
    }
}
