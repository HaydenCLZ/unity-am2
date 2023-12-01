using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IASpider : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _turnSpeed = 150;
    [System.NonSerialized] public Transform target;
    [SerializeField] public Vector3 spawn;
    [SerializeField] private float heightFromGround;
    [SerializeField] private Transform raycaster;

    private Vector3 goTo;
    private float timer;
    private float cdattack;
    private bool attacked;

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
        StartCoroutine(lookAt());
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
                    target.GetComponent<Main_Character_Health>().TakeDamage(20);//met des degats au joueur
                    GetComponent<Spider_Health>().TakeDamage(50);//met des degats à l'arraignée
                }
            }
            else if (cdattack < 0.1 && cdattack > 0.07)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, -0.4f);
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
            Vector3 lookAtPosition = target.position;
            lookAtPosition.y = transform.position.y;
            transform.LookAt(lookAtPosition);
            attacked = false;
            cdattack = 0.3f;
        }
        else
        {
            if (Vector3.Distance(transform.position, goTo) < 0.2)
            {
                System.Random rand = new System.Random();
                goTo = spawn + new Vector3(2f * (float)rand.NextDouble() - 1f, 0f, 2f * (float)rand.NextDouble() - 1f);
                timer = 0;
            }
            else
            {
                if (timer > 1.5)
                {
                    transform.position = Vector3.MoveTowards(transform.position, goTo, 0.03f);
                    Vector3 lookAtPosition = goTo;
                    lookAtPosition.y = transform.position.y;
                    transform.LookAt(lookAtPosition);
                }
            }
        }
        reajustHeight();

    }

    public void reajustHeight()
    {
        float distToGround = GetDistanceToGround();
        if (distToGround < heightFromGround || distToGround > heightFromGround)
            transform.position += transform.up * (heightFromGround - distToGround);
    }

    public float GetDistanceToGround()
    {
        RaycastHit hit;
        float distanceToGround = 0f;

        // Créer un rayon vers le bas depuis la position actuelle de l'objet
        if (Physics.Raycast(raycaster.position, -transform.up, out hit))
        {
            distanceToGround = hit.distance;
        }

        return distanceToGround;
    }

    private IEnumerator lookAt()
    {
        Quaternion LookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, time);

            time += Time.deltaTime * _turnSpeed;

            yield return null;
        }
    }
}