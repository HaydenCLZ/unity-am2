
using UnityEngine;


public class SpiderController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _turnSpeed = 150;
    [SerializeField] private Transform raycaster;
    [SerializeField] private float heightFromGround;

    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }


    void FixedUpdate()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        ForwardInput = vertical;
        TurnInput = horizontal;
        if (TurnInput != 0f)
        {
            float angle = TurnInput * _turnSpeed;
            transform.Rotate(0, angle, 0, Space.Self);  
        }
        transform.position += (_moveSpeed * ForwardInput * Time.deltaTime * transform.forward);
        float dist = GetDistanceToGround();
        if (dist < heightFromGround || dist > heightFromGround)
            transform.position += transform.up * (heightFromGround - dist);

    }

    public float GetDistanceToGround()
    {
        RaycastHit hit;
        float distanceToGround = 0f;

        // Cr�er un rayon vers le bas depuis la position actuelle de l'objet
        if (Physics.Raycast(raycaster.position, -transform.up, out hit))
        {
            distanceToGround = hit.distance;
        }

        return distanceToGround;
    }
}