using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class IASpider : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _turnSpeed = 150;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 spawn;

    private Vector3 goTo;
    private float timer;

    private Ray _ray;
    private RaycastHit _hit;

    private Vector3 _targetPosition;
    private Quaternion _targetRot;
    private float rotationSmoothness;
    private Rigidbody rb;

    private BoxCollider boxCollider;


    void Start()
    {
        goTo = spawn;
        timer = 0;
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(Vector3.Distance(transform.position, target.position) < 20)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.05f);
            transform.LookAt(target); 
        }
        else
        {
            if(Vector3.Distance(transform.position, goTo) < 0.2)
            {
                System.Random rand = new System.Random();
                goTo = spawn + new Vector3(2f*(float)rand.NextDouble()-1f,0f,2f*(float)rand.NextDouble()-1f);
                timer = 0;
            }
            else
            {
                if(timer>1.5)
                {
                    transform.position = Vector3.MoveTowards(transform.position, goTo, 0.02f);
                    transform.LookAt(goTo);
                }
            }
        }
    }
    private float distToGround()
    {
        RaycastHit hitG;
        if (Physics.Raycast(transform.position, -transform.up, out hitG, Mathf.Infinity))
            return Vector3.Distance(transform.TransformPoint(boxCollider.center), hitG.point) - boxCollider.size.y / 2;
        return Mathf.Infinity;
    }

}