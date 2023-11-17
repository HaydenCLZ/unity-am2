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
    private Transform target;
    [SerializeField] private Vector3 spawn;

    private Vector3 goTo;
    private float timer;
    private float cdattack;
    private bool attacked;


    private Ray _ray;
    private RaycastHit _hit;

    private Vector3 _targetPosition;
    private Quaternion _targetRot;
    private float rotationSmoothness;
    private Rigidbody rb;

    private BoxCollider boxCollider;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        goTo = spawn;
        timer = 0;
        cdattack = 0;
    }

    void FixedUpdate()
    {
        float dist = Vector3.Distance(transform.position, target.position);
        timer += Time.fixedDeltaTime;
        if (dist < 3)
        {
            cdattack += Time.fixedDeltaTime;
            if (cdattack < 0.03)
            {
                transform.LookAt(target);
                transform.position = Vector3.MoveTowards(transform.position, target.position, 0.4f);
                if (attacked == false)
                {
                    attacked = true;
                    SpiderHealth hp = target.GetComponent<SpiderHealth>();
                    hp.TakeDamage(20);
                    XP xp = target.GetComponent<XP>();
                    xp.addXP(1);
                }
            }
            else if (cdattack < 0.1 && cdattack > 0.07)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, -0.4f);
                transform.LookAt(target);
            }
            else if (cdattack > 1.3)
            {
                attacked = false;
                cdattack = 0;
            }
        }
        else if (dist < 15)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.06f);
            transform.LookAt(target);
            attacked = false;
            cdattack = 0.3f;
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
                    transform.position = Vector3.MoveTowards(transform.position, goTo, 0.03f);
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