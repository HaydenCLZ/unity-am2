using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public Transform orientation;
    public Transform persoprincipal;
    public Transform persoprincipalObj;
    public Rigidbody rb;

    public float rotationSpeed;
    
    private void Start()
    {

    }

    private void Update()
    {
        //Rotation Orientation :
        Vector3 viewDir = persoprincipal.position - new Vector3(transform.position.x, persoprincipal.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //Rotation de l'objet persoprincipal
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            persoprincipalObj.forward = Vector3.Slerp(persoprincipalObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

    }
}
