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

    public float moveDuration;
    float timeElapsed = 0;
    bool moving = false;

    IEnumerator MoveToHome()
    {
        moving = true;
        Vector3 startPoint = transform.position;
        Vector3 endPoint = target.position;
        do
        {
            // Add time since last frame to the time elapsed
            timeElapsed += Time.deltaTime;

            float normalizedTime = timeElapsed / moveDuration;
            transform.position = Vector3.Lerp(startPoint, endPoint, normalizedTime);

            // Wait for one frame
            yield return null;
        }
        while (timeElapsed < moveDuration);
        moving = false;
    }

    void Update()
    {
        // If we are already moving, don't start another move
        if (moving) return;

        float distFromHome = Vector3.Distance(transform.position, target.position);

        // If we are too far off in position or rotation
        if (distFromHome > maxDistance)
        {
            // Start the step coroutine
            StartCoroutine(MoveToHome());
        }
    }
}

