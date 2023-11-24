using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light Spotlight;

    void Start()
    {

        Spotlight = GetComponent<Light>();
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            Spotlight.enabled = !Spotlight.enabled;
        }
    }


}
