using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float fuseTime;
    [SerializeField] private float speed = 5f;  // Vitesse du missile
    ParticleSystem exp;

    private Rigidbody rb;
    void Start()
    {
        exp = GetComponent<ParticleSystem>();
        exp.transform.position = transform.position;
        rb = transform.GetComponent<Rigidbody>();

        //

        Invoke("Explode", fuseTime);
        

    }



    void Explode()
    {
        if (!exp.isPlaying)
            exp.Play();
        gameObject.GetComponent<Renderer>().enabled = false;
        Destroy(rb);
        Invoke("DelayedDestroy", exp.main.duration);
    }

    void DelayedDestroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if (rb != null)
        {
            rb.velocity = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)) * speed;
        }
        //Vector3.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)), 0);
    }
}
