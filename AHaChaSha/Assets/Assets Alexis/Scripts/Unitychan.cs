using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unitychan : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interact");
    }
}
