using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float fuseTime;
    [SerializeField] private float speed = 5f;  // Vitesse du missile
    [SerializeField] private float explosionRadius = 2f;
    ParticleSystem exp;

    private Rigidbody rb;
    void Start()
    {
        exp = GetComponent<ParticleSystem>();
        exp.transform.position = transform.position;
        rb = transform.GetComponent<Rigidbody>();
        rb.detectCollisions = true;
        rb.velocity = transform.up * speed;

        Invoke("Explode", fuseTime);
    }



    void Explode()
    {
        if (gameObject.GetComponent<Collider>().enabled)
        {
            if (!exp.isPlaying)
                exp.Play();
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            rb.velocity = new Vector3(0f, 0f, 0f) ;
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    Health H = hit.gameObject.GetComponent<Health>();
                    H.TakeDamage(50);
                }
            }
            Invoke("DelayedDestroy", exp.main.duration);
        }
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

}
