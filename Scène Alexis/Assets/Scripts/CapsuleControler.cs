using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleControler : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward_world = transform.TransformDirection(Vector3.forward);
        Vector3 side_world = transform.TransformDirection(Vector3.right);
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector2 keyInput = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            keyInput.x++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            keyInput.x--;
        }
        if (Input.GetKey(KeyCode.D))
        {
            keyInput.y++;
        }
        if (Input.GetKey(KeyCode.A))
        {
            keyInput.y--;
        }
        transform.position += forward_world * speed * Time.deltaTime * keyInput.x * -1;
        transform.position += side_world * speed * Time.deltaTime * keyInput.y * -1;
        transform.Rotate(Vector3.up, mouseInput.x * rotationSpeed);
    }
}
