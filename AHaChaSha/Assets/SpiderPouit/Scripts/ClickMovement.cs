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
        _targetPosition = transform.position;
        _targetRot = transform.rotation;
    }

    void FixedUpdate()
    {
        CheckGrounded();
        int vertical = Mathf.RoundToInt(Input.GetAxisRaw("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxisRaw("Horizontal"));
        ForwardInput = vertical;
        TurnInput = horizontal;
        if (TurnInput != 0f)
        {
            float angle = Mathf.Clamp(TurnInput, -1f, 1f) * _turnSpeed;
            transform.Rotate(transform.up, Time.fixedDeltaTime * angle);
        }
        if (_isGrounded)
        {
            // Reset the velocity
            rb.velocity = Vector3.zero;

            // Apply a forward or backward velocity based on player input
            rb.velocity += transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * _moveSpeed;
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = false;
        Vector3 boxBottom = transform.TransformPoint(boxCollider.center - Vector3.up * boxCollider.size.y/2f);
        float size = transform.TransformVector(boxCollider.size.y/2f, 0f, 0f).magnitude;
        Ray ray = new Ray(boxBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, size * 5f))
        {
            if (hit.distance < 0.2f)
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