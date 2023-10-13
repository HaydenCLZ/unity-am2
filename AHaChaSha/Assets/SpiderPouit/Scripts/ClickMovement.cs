using System.Collections;
using System.Collections.Generic;
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
    private Quaternion _current;
    private Quaternion _targetRot;
    private float rotationSmoothness;

    void Awake()
    {
        _cam = Camera.main;
    }

    void Start()
    {
        _targetPosition = transform.position;
        _current = transform.rotation;
        _targetRot = transform.rotation;
        rotationSmoothness = GetComponent<canClimbAnywhere>().rotationSmoothness;
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
            }
        }
        Move();
    }


    void Move()
    {
        Quaternion.RotateTowards(_current, _targetRot, rotationSmoothness);
        _current = transform.rotation;

        if(Quaternion.Angle(_current, _targetRot) < 1)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }
}