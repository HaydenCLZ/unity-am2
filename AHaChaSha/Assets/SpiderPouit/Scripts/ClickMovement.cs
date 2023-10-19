using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClickMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _turnSpeed = 150;

    private bool _isGrounded;
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }

    private Camera _cam;
    private Ray _ray;
    private RaycastHit _hit;

    private Vector3 _targetPosition;
    private Quaternion _targetRot;
    private float rotationSmoothness;
    private Rigidbody rb;

    private BoxCollider boxCollider;

    void Awake()
    {
        _cam = Camera.main;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        CheckGrounded();
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        ForwardInput = vertical;
        TurnInput = horizontal;
        if (TurnInput != 0f)
        {
            float angle = TurnInput * _turnSpeed;
            transform.Rotate(0, angle, 0, Space.Self);  
        }
        if (_isGrounded)
        {
            transform.position += (transform.forward * ForwardInput * Time.deltaTime * _moveSpeed);
        }
        
    }

    private void CheckGrounded()
    {
        _isGrounded = true;
        RaycastHit hit;
        if (Physics.Raycast(transform.GetChild(transform.childCount-1).position, -transform.up, out hit, Mathf.Infinity))
        {
            if (hit.distance <= 0.5f)
               _isGrounded = true;
        }
    }

    private float distToGround()
    {
        RaycastHit hitG;
        if (Physics.Raycast(transform.position, -transform.up, out hitG, Mathf.Infinity))
            return Vector3.Distance(transform.TransformPoint(boxCollider.center), hitG.point) - boxCollider.size.y/2;
        return Mathf.Infinity;
    }

}