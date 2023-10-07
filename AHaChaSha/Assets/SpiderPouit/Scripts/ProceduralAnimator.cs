using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    class ProceduralLeg
    {
        public Transform IKTarget;
        public Vector3 defaultPosition;
        public Vector3 lastPosition;
        public bool moving;
    }

    [Header("Global")]
    [SerializeField] private LayerMask _groundLayerMask = default;

    [Header("Steps")]
    [SerializeField] private Transform[] _legsTargets;
    [SerializeField] private float _stepSize = 1;
    [SerializeField] private float _stepHeight = 1;
    [SerializeField] private int _smoothness = 1;
    [SerializeField] private float _raycastRange = 2;
    [SerializeField] private float _feetOffset = 0;

    private int _nbLegs;
    private ProceduralLeg[] _legs;

    private Vector3 _lastBodyPosition;
    private Vector3 _velocity;
    private bool _allLimbsResting;

    void Start()
    {
        _nbLegs = _legsTargets.Length;
        _legs = new ProceduralLeg[_nbLegs];
        Transform target;
        for (int i = 0; i < _nbLegs; ++i)
        {
            target = _legsTargets[i];
            _legs[i] = new ProceduralLeg()
            {
                IKTarget = target,
                defaultPosition = target.localPosition,
                lastPosition = target.position,
                moving = false
            };
        }

        _lastBodyPosition = transform.position;
        _allLimbsResting = true;
    }

    void FixedUpdate()
    {
        _velocity = transform.position - _lastBodyPosition;

        if (_velocity.magnitude > Mathf.Epsilon)
            _HandleMovement();
        /*if (!_allLimbsResting)
            _BackToRestPosition();*/
    }

    private void _HandleMovement()
    {
        _lastBodyPosition = transform.position;

        Vector3[] targetPositions = new Vector3[_nbLegs];
        float tempStepSize = _stepSize;
        int limbToMove = -1;

        for (int i = 0; i < _nbLegs; ++i)
        {
            if (_legs[i].moving) continue; // limb already moving: can't move again!

            targetPositions[i] = transform.TransformPoint(_legs[i].defaultPosition);
            float dist = (targetPositions[i] + _velocity - _legs[i].lastPosition).magnitude;
            if (dist > tempStepSize)
            {
                tempStepSize = dist;
                limbToMove = i;
            }
        }

        for (int i = 0; i < _nbLegs; ++i)
            if (i != limbToMove)
                _legs[i].IKTarget.position = _legs[i].lastPosition;

        if (limbToMove != -1)
        {
            Vector3 targetPoint = targetPositions[limbToMove];
            
            targetPoint = _RaycastToGround(targetPoint, transform.up);
            _limbs[limbToMove].IKTarget.position = targetPoint;
            _limbs[limbToMove].lastPosition = targetPoint;

            _allLimbsResting = false;
            StartCoroutine(_Stepping(limbToMove, targetPoint));
        }
    }

    private void _BackToRestPosition()
    {
        Vector3 targetPoint; float dist;
        for (int i = 0; i < _nbLegs; ++i)
        {
            if (_legs[i].moving) continue; // limb already moving: can't move again!

            targetPoint = _RaycastToGround(
                transform.TransformPoint(_legs[i].defaultPosition), transform.up) + transform.up * _feetOffset;
            dist = (targetPoint - _legs[i].lastPosition).magnitude;
            if (dist > 0.005f)
            {
                StartCoroutine(_Stepping(i, targetPoint));
                return;
            }
        }
        _allLimbsResting = true;
    }

    private Vector3 _RaycastToGround(Vector3 pos, Vector3 up)
    {
        Vector3 point = pos;

        Ray ray = new Ray(pos + _raycastRange * up, -up);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayerMask))
            point = hit.point;
        return point;
    }

    private IEnumerator _Stepping(int legIdx, Vector3 targetPosition)
    {
        _legs[legIdx].moving = true;
        Vector3 startPosition = _legs[legIdx].lastPosition;
        float t;
        for (int i = 1; i <= _smoothness; ++i)
        {
            t = i / (_smoothness + 1f);
            _legs[legIdx].IKTarget.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        _legs[legIdx].IKTarget.position = targetPosition;
        _legs[legIdx].lastPosition = targetPosition;
        _legs[legIdx].moving = false;
    }
}