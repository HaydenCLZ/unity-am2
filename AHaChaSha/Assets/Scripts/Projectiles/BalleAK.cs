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
    void FixedUpdate()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.gameObject.GetComponent<Spider_Health>().TakeDamage(5);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.gameObject.CompareTag("Player") || !collision.collider.gameObject.CompareTag("NPC"))
            Destroy(gameObject);
    }
}
