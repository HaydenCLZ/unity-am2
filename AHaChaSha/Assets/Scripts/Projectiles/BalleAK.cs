using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalleAK : MonoBehaviour
{
    public float vitesseBalle = 10f;

    private Rigidbody rb;
    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.detectCollisions = true;
        rb.velocity = transform.up * vitesseBalle;
    }
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 2f);
    }
}
