using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class epaule : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        transform.Rotate(Vector3.forward, mouseInput.y * rotationSpeed);
    }
}
