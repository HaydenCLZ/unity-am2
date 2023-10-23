using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

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
    
    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float velocityMultiplier = 1f;

    public bool ArcCast(Vector3 center, Quaternion rotation, float angle, float radius, int precision, LayerMask layer, out RaycastHit hit)
    {
        rotation *= Quaternion.Euler(-angle / 2, 0, 0);
        Vector3 forwardRadius = Vector3.forward * radius;
        float dAngle = angle / precision;
        Vector3 A, B, AB;
        A = forwardRadius;
        B = Quaternion.Euler(dAngle, 0, 0) * forwardRadius;
        AB = B - A;
        float AB_magnitude = AB.magnitude * 1.001f;

        for (int i = 0; i < precision; i++)
        {
            A = center + rotation * forwardRadius;
            rotation *= Quaternion.Euler(dAngle, 0, 0);
            B = center + rotation * forwardRadius;
            AB = B - A;
            Debug.DrawRay(A, AB, UnityEngine.Color.red);
            if (Physics.Raycast(A, AB, out hit, AB_magnitude))
            {
                return true;
            }
        }
        hit = new RaycastHit();
        return false;
    }
    Vector3[] MatchToSurfaceFromAbove(Vector3 point, float halfRange, Vector3 up)
    {
        Vector3[] res = new Vector3[2];
        res[1] = Vector3.zero;
        RaycastHit hit;
        Ray ray = new Ray(point + halfRange * up / 2f, - up);
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
    }

    IEnumerator PerformStep(int index, Vector3 targetPoint)
    {
        legMoving[index] = true;
        Vector3 startPos = lastLegPositions[index];
        for(int i = 1; i <= smoothness; ++i)
        {
            legTargets[index].position = Vector3.Lerp(startPos, targetPoint, i / (float)(smoothness + 1f));
            legTargets[index].position += transform.up * Mathf.Sin(i / (float)(smoothness + 1f) * Mathf.PI) * stepHeight;
            yield return new WaitForFixedUpdate();
        }
        legTargets[index].position = targetPoint;
        lastLegPositions[index] = targetPoint;
        legMoving[index] = false;
    }


    void FixedUpdate()
    {
        
        velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;

        move();
        rotateBody();
        bodyPositionToGround();
        lastBodyPos = transform.position;
    }
    public void rotateBody()
    {
        RaycastHit hit;

        if (ArcCast(raycaster.position, transform.rotation, 270, 1.75f, 6, 0, out hit))
        {
            Vector3 up = Vector3.Lerp(transform.up, hit.normal, 1f / (float)(smoothness + 1));
            transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
        }
        /*
        Vector3 v1 = legTargets[1].position - legTargets[0].position;
        Vector3 v2 = legTargets[3].position - legTargets[2].position;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / (float)(smoothness + 1));
        transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
        lastBodyUp = up;*/
    }

    public void bodyPositionToGround()
    {
        RaycastHit hitGround;
        Debug.DrawRay(raycaster.position, -transform.up);
        if (Physics.Raycast(raycaster.position, -transform.up, out hitGround, 0.45f))
        {
            Vector3 pos = hitGround.point; //get the position where the ray hit the ground
            Debug.Log("hit!");
            //shoot a raycast up from that position towards the object
            Ray upRay = new Ray(pos, transform.position - pos);

            //get a point (vector3) in that ray 8 units from its origin
            Vector3 upDist = upRay.GetPoint(0.40f);

            //smoothly interpolate its position
            transform.position = Vector3.Lerp(transform.position, upDist, 0.2f);
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
