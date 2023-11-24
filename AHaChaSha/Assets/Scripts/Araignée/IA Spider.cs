using System.Collections;
using UnityEngine;


public class IASpider : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _turnSpeed = 150;
    private Transform target;
    [SerializeField] private Vector3 spawn;
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
                    SpiderHealth hp = target.GetComponent<SpiderHealth>();
                    hp.TakeDamage(20);
                    XP xp = target.GetComponent<XP>();
                    xp.addXP(1);
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
        float distToG = GetDistanceToGround();
        if (distToG < heightFromGround || distToG > heightFromGround)
            transform.position += transform.up * (heightFromGround - dist);
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