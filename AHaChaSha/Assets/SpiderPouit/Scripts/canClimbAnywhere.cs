﻿using System.Collections;
using UnityEngine;


public class canClimbAnywhere : MonoBehaviour
{
    public Transform[] legTargets;
    public float stepSize = 0.15f;
    public int smoothness = 8;
    public float rotationSmoothness = 5f;
    public float stepHeight = 0.15f;
    public float sphereCastRadius = 0.125f;
    public bool bodyOrientation = true;

    public float raycastRange = 1.5f;
    private Vector3[] defaultLegPositions;
    private Vector3[] lastLegPositions;
    private Vector3 lastBodyUp;
    private bool[] legMoving;
    private int nbLegs;

    [SerializeField] private Transform raycaster;

    private float controllerInput;
    
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float velocityMultiplier = 1f;

    private ClickMovement input;

    Vector3[] MatchToSurfaceFromAbove(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        res[1] = Vector3.zero;
        RaycastHit hit;
        Ray ray = new (point + halfRange * up / 2f, - up);
        if (Physics.SphereCast(ray, sphereCastRadius, out hit, 2f * halfRange))
        {
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            res[0] = point;
        }
        return res;
    }
    
    void Start()
    {
        lastBodyUp = transform.up;
        nbLegs = legTargets.Length;
        defaultLegPositions = new Vector3[nbLegs];
        lastLegPositions = new Vector3[nbLegs];
        legMoving = new bool[nbLegs];
        for (int i = 0; i < nbLegs; ++i)
        {
            defaultLegPositions[i] = legTargets[i].localPosition;
            lastLegPositions[i] = legTargets[i].position;
            legMoving[i] = false;
        }
        lastBodyPos = transform.position;
        if (!TryGetComponent(out input)) { Debug.Log("Where clickmovement ?"); }
        
    }

    IEnumerator PerformStep(int index, Vector3 targetPoint)
    {
        legMoving[index] = true;
        Vector3 startPos = lastLegPositions[index];
        for(int i = 1; i <= smoothness; ++i)
        {
            legTargets[index].position = Vector3.Lerp(startPos, targetPoint, i / (float)(smoothness + 1f));
            legTargets[index].position += Mathf.Sin(i / (float)(smoothness + 1f) * Mathf.PI) * stepHeight * transform.up;
            yield return new WaitForFixedUpdate();
        }
        legTargets[index].position = targetPoint;
        lastLegPositions[index] = targetPoint;
        legMoving[index] = false;
    }


    void FixedUpdate()
    {
        controllerInput = input.ForwardInput;
        velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;

        move();
        rotateBody();
        lastBodyPos = transform.position;
    }
    public void rotateBody()
    {
        int sign = 1;
        if (controllerInput < 0)
            sign = -1;
       
        if (CastExtension.ArcCast(raycaster.position, transform.rotation, 270*sign, 1.90f, 8, sign, out RaycastHit hit))
        {
            Vector3 up = Vector3.Lerp(transform.up, hit.normal, 1f / (float)(smoothness + 1));
            transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;

        }
    }

    public void move()
    {
        Vector3[] desiredPositions = new Vector3[nbLegs];
        int indexToMove = -1;
        float maxDistance = stepSize;
        for (int i = 0; i < nbLegs; ++i)
        {
            desiredPositions[i] = transform.TransformPoint(defaultLegPositions[i]);

            float distance = Vector3.ProjectOnPlane(desiredPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up).magnitude;
            if (distance > maxDistance)
            {
                maxDistance = distance;
                indexToMove = i;
            }
        }
        for (int i = 0; i < nbLegs; ++i)
            if (i != indexToMove)
                legTargets[i].position = lastLegPositions[i];

        if (indexToMove != -1)
        {
            Vector3 targetPoint = desiredPositions[indexToMove] + Mathf.Clamp(velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * (desiredPositions[indexToMove] - legTargets[indexToMove].position) + velocity * velocityMultiplier;

            Vector3[] positionAndNormalFwd = MatchToSurfaceFromAbove(targetPoint + velocity * velocityMultiplier, raycastRange, (transform.up - velocity * 100).normalized);
            Vector3[] positionAndNormalBwd = MatchToSurfaceFromAbove(targetPoint + velocity * velocityMultiplier, raycastRange * (1f + velocity.magnitude), (transform.up + velocity * 75).normalized);

            if (positionAndNormalFwd[1] == Vector3.zero)
            {
                StartCoroutine(PerformStep(indexToMove, positionAndNormalBwd[0]));
            }
            else
            {
                StartCoroutine(PerformStep(indexToMove, positionAndNormalFwd[0]));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < nbLegs; ++i)
        {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawWireSphere(legTargets[i].position, 0.5f);
            Gizmos.color = UnityEngine.Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(defaultLegPositions[i]), stepSize);
        }
    }
}
