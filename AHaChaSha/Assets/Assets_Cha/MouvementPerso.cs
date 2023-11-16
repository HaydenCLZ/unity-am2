using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementPerso : MonoBehaviour
{
    //Vitesse du joueur

    [Header("Movement")]
    public float moveSpeed;

    //Variables de verification que le joueur est sur le sol

    [Header("Ground Check")]
    public float groundDrag;
    /* public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    private float distToGround; */


    //Variable d'inputs et d'orientation
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    private void Start()
    {
        //Génération du rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //Calcule la distance entre le centre du perso et le sol.
        //distToGround = GetComponent<Collider>().bounds.extents.y; ;


    }

    private void Update()
    {
        //GroundCheck();
        Inputs();
        SpeedControl();
        rb.drag = groundDrag;
        /*if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;*/

    }
        private void FixedUpdate()
    {
        MouvPerso();
    }
  
    /*private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.2f);
    }*/

    private void Inputs() //R�cup�re les touches directionnelles 
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
        
    
    private void MouvPerso()
    {
        //Calcule la direction du mouvement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Donne une impulsion de mouvement
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(horizontalInput == 0 && verticalInput == 0)
        {
            rb.velocity = new Vector3(0f,0f,0f);
        }

       // Limite la vitesse
       if(Input.GetKey("left shift") && flatVel.magnitude > moveSpeed*1.5f)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed * 1.5f;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        else if (!Input.GetKey("left shift") && flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    /* private void GroundCheck()
    {
        //Verifie si le raycast atteint le sol
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }
*/
    /* private void Drag()
    {
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
    */
}
