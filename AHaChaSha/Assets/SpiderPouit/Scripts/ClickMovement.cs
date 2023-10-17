using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ClickMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _speed = 10f;

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
    }

    void Start()
    {
        _targetPosition = transform.position;
        _targetRot = transform.rotation;
        rotationSmoothness = GetComponent<canClimbAnywhere>().rotationSmoothness;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit _hit;
            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, _groundLayerMask))
            {
                _targetRot = Quaternion.LookRotation(_hit.point - transform.position, transform.up);
                _targetPosition = _hit.point;
                Debug.Log(_hit.point);
            }
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRot, rotationSmoothness);
        if (Quaternion.Angle(transform.rotation, _targetRot) < 1)
            if ((_targetPosition - transform.position).sqrMagnitude < 0.02f)
                transform.position = _targetPosition;
            else
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        /*if (!(isGrounded()))
        {
            rb.AddForce(-transform.up * 0.981f);
        }*/
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

    private float distToGround()
    {
        RaycastHit hitG;
        if (Physics.Raycast(transform.position, -transform.up, out hitG, Mathf.Infinity))
            return Vector3.Distance(transform.TransformPoint(boxCollider.center), hitG.point) - boxCollider.size.y/2;
        return Mathf.Infinity;
    }

}