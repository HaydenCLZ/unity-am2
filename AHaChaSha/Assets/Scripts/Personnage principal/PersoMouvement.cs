using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PersoMouvement : MonoBehaviour
{
    PersoInput persoInput; //R�f�rence la classe PersoInput initialis� par le InputManager
    CharacterController characterController;

    //Variables 
    Vector2 movementInput; //R�cup�re les donn�es du InputManager
    Vector3 movement; //ajout de la variable movement pour pouvoir g�rer la hauteur � terme.
    bool isMoving;
    bool isRunning;
    Vector3 cameraMovement;

    [Header("Vitesse")]
    public float movingSpeed = 10.0f;
    public float runningSpeed = 3.0f;
    public float rotationSpeed = 7.0f;

    // Permet d'instancier avant Start()
    void Awake()
    {
        persoInput = new PersoInput();
        characterController = GetComponent<CharacterController>();
        
        //R�cup�re les inputs pour marcher
        persoInput.PlayerControler.Move.started += onMovementInput;
        persoInput.PlayerControler.Move.performed += onMovementInput;
        persoInput.PlayerControler.Move.canceled += onMovementInput;
        //R�cup�re les inputs pour courir
        persoInput.PlayerControler.Run.started += onRun;
        persoInput.PlayerControler.Run.canceled += onRun;

    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        //D�placement en marchant
        movement.x = -movementInput.x;
        movement.z = movementInput.y;

        //V�rifie que le perso bouge.
        isMoving = movementInput.x != 0 || movementInput.y != 0;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton(); // On ne presse que le shift pour courir, pas besoin de vecteur.
    }

    //Contr�le la rotation du personnage � l'aide de quaternion.
    void rotation()
    {
        //Initialisation du vecteur direction
        Vector3 directionToLookAt;

        directionToLookAt.x = cameraMovement.x;
        directionToLookAt.y = 0.0f; //On ne regarde pas en hauteur
        directionToLookAt.z = cameraMovement.z;

        //Rotation actuelle du perso : 
        Quaternion currentRotation = transform.rotation;


        if (isMoving) //Cr�e la rotation � partir de la direction dans laquelle va le perso
        {
            Quaternion rotation = Quaternion.LookRotation(directionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation.normalized, rotation.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    void gravity()
    {
        if(characterController.isGrounded)
        {
            float groundGravity = -0.05f / movingSpeed;
            movement.y = groundGravity;
        } else
        {
            float gravity = -9.8f / movingSpeed;
            movement.y = gravity;
        }
    }

    Vector3 ConvertToCamera(Vector3 vectorToRotate) //Converti des coordon�es mondes au coordon�es de la cam�ra.
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        float currentYValue = vectorToRotate.y;

        //Enl�ve les y pour ignorer les angles de la cam�ra 
        cameraForward.y = 0;
        cameraRight.y = 0;

        //Normalise les vecteurs 
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardProductZ = vectorToRotate.z * cameraForward;
        Vector3 cameraRightProductX = vectorToRotate.x * cameraRight;

        Vector3 rotatedVector = cameraForwardProductZ + cameraRightProductX;
        rotatedVector.y = currentYValue;
        return rotatedVector;
    }

    // Update is called once per frame
    void Update()
    {
        gravity();
        rotation();

        cameraMovement = ConvertToCamera(movement);
        if (isRunning)
        {
            characterController.Move(cameraMovement * Time.deltaTime * movingSpeed * runningSpeed);
        } else
        {
            characterController.Move(cameraMovement * Time.deltaTime * movingSpeed);
        }
    }

    //V�rifie que le InputManager est bien initialis�
    void OnEnable()
    {
        persoInput.PlayerControler.Enable();
    }

    //V�rifie que le InputManager est bien �teint
    void OnDisable()
    {
        persoInput.PlayerControler.Disable();
    }
    
}