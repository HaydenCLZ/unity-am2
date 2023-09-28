using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class LegStepper : MonoBehaviour
{
    public float maxDistance;
    public float minDistance;

    public Transform target;
    public bool follow = false;
    public float speed = 3f;


    void Update()
    {
        if (follow)
        {
            float distFromHome = Vector3.Distance(transform.position, target.position);
            Vector3 startPoint = transform.position;
            Vector3 endPoint = target.position;
            // If we are too far off in position
            if (distFromHome > maxDistance && distFromHome > minDistance)
            {
                Vector3 newpos = Vector3.MoveTowards(startPoint, endPoint, speed * Time.deltaTime);
                transform.position = new Vector3(newpos.x, startPoint.y, newpos.z);
            }
        }
    }
}

