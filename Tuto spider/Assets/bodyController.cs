using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{

    public GameObject[] legTargets;
    public GameObject[] legCubes;
    public GameObject spider;
    Vector3[] legPositions;
    Vector3[] legOriginalPositions;

    Vector3 velocity;
    Vector3 lastSpiderPosition;
    Vector3 lastVelocity;

    public float moveDistance = 0.7f;
    public float legMovementSmoothness = 8f;
    public float bodySmoothness = 8f;
    public float overStepMultiplier = 4f;
    public int waitTimeBetweenSteps = 0;
    public float spiderJitterCutOff = 0f;
    public float stepHeight = 0.15f;


    bool currentLeg = true;
    Vector3 lastBodyUp; 

    List<int> oppositeLegIndex = new List<int>();
    List<int> nextIndexToMove = new List<int>();
    List<int> indexMoving = new List<int>();


    void Start()
    {
        lastBodyUp = transform.up;
        legPositions = new Vector3[legTargets.Length];
        legOriginalPositions = new Vector3[legTargets.Length];
        for (int i = 0; i < legTargets.Length; i++)
        {
            legPositions[i] = legTargets[i].transform.position;
            legOriginalPositions[i] = legPositions[i];

            if (currentLeg)
            {
                oppositeLegIndex.Add(i + 1);
                currentLeg = false; 
            }
            else
            {
                oppositeLegIndex.Add(i - 1);
                currentLeg = true;
            }
        }
        lastSpiderPosition = spider.transform.position;
        rotateBody();
    }

    void FixedUpdate()
    {
        velocity = spider.transform.position - lastSpiderPosition;
        velocity = (velocity + bodySmoothness * lastVelocity) / (bodySmoothness + 1f);


        moveLegs();
        rotateBody();

        lastSpiderPosition = spider.transform.position;
        lastVelocity = velocity;
    }

    void moveLegs()
    {
        for (int i = 0; i < legTargets.Length; i++)
        {
            if (Vector3.Distance(legTargets[i].transform.position, legCubes[i].transform.position) >= moveDistance)
            {
                if(!nextIndexToMove.Contains(i) && !indexMoving.Contains(i)) nextIndexToMove.Add(i);
            }
            else if (!indexMoving.Contains((int)i)) 
            {
                legTargets[i].transform.position = legOriginalPositions[i];
            }
        }

        if (nextIndexToMove.Count == 0 || indexMoving.Count != 0) return;
        Vector3 targetPosition = legCubes[nextIndexToMove[0]].transform.position + Mathf.Clamp(velocity.magnitude * overStepMultiplier, 0f, 1.5f) * (legCubes[nextIndexToMove[0]].transform.position - legTargets[nextIndexToMove[0]].transform.position) + velocity * overStepMultiplier; 
        StartCoroutine(step(nextIndexToMove[0], targetPosition, false));
    }

    IEnumerator step(int index, Vector3 moveTo, bool isOpposite)
    {
        if (!isOpposite) moveOppositeLeg(oppositeLegIndex[index]);

        if (nextIndexToMove.Contains((int)index)) nextIndexToMove.Remove(index);
        if (!indexMoving.Contains(index)) indexMoving.Add(index);

        Vector3 startingPosition = legOriginalPositions[index];

        for (int i = 1; i <= legMovementSmoothness; i++)
        {
            legTargets[index].transform.position = Vector3.Lerp(startingPosition, moveTo + new Vector3(0, Mathf.Sin(i / (legMovementSmoothness + spiderJitterCutOff) * Mathf.PI) * stepHeight, 0), (i / legMovementSmoothness + spiderJitterCutOff));
            yield return new WaitForFixedUpdate();
        }

        legOriginalPositions[index] = moveTo;

        for (int i = 0; i < waitTimeBetweenSteps; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (indexMoving.Contains(index)) indexMoving.Remove(index);
    }

    void rotateBody()
    {
        Vector3 v1 = legTargets[0].transform.position - legTargets[1].transform.position;
        Vector3 v2 = legTargets[2].transform.position - legTargets[3].transform.position;
        Vector3 normal = Vector3.Cross(v1, v2).normalized;
        Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / bodySmoothness);
        transform.up = up;
        lastBodyUp = transform.up;
    }

    void moveOppositeLeg(int index)
    {
        Vector3 targetPosition = legCubes[index].transform.position + Mathf.Clamp(velocity.magnitude * overStepMultiplier, 0f, 1.5f) * (legCubes[index].transform.position - legTargets[index].transform.position) + velocity * overStepMultiplier;
        StartCoroutine(step(index, targetPosition, true));
    }
}
