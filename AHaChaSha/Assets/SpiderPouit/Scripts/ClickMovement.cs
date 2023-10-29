
using UnityEngine;


public class ClickMovement : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _turnSpeed = 150;

    private bool _isGrounded;
    public float ForwardInput { get; set; }
    public float TurnInput { get; set; }


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
            transform.position += (_moveSpeed * ForwardInput * Time.deltaTime * transform.forward);
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

}