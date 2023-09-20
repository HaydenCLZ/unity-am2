using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyController : MonoBehaviour
{

    public GameObject[] legTargets;
    public GameObject[] legCubes;
    Vector3[] legPositions;
    Vector3[] legOriginalPositions;

    void Start()
    {
        legPositions = new Vector3[legTargets.Length];
        legOriginalPositions = new Vector3[legTargets.Length];
        for (int i = 0 ; i < legTargets.Length; i++)
        {
            legPositions[i] = legTargets[i].transform.position;
            legOriginalPositions[i] = legPositions[i];
        }
    }

    void FixedUpdate()
    {
        
    }

    void moveLegs()
    {
        for (int i = 0; i < legTargets.Length; i++)
        {
            legTargets[i].transform.position = legOriginalPositions[i];
        }
    }
}
