using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PersoMouvement : MonoBehaviour
{
    PersoInput persoInput; //Référence la classe PersoInput initialisé par le InputManager
    CharacterController characterController;

    //Variables 
    Vector2 movementInput; //Récupère les données du InputManager
    Vector3 movement; //ajout de la variable movement pour pouvoir gérer la hauteur à terme.
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
        
        //Récupère les inputs pour marcher
        persoInput.PlayerControler.Move.started += onMovementInput;
        persoInput.PlayerControler.Move.performed += onMovementInput;
        persoInput.PlayerControler.Move.canceled += onMovementInput;
        //Récupère les inputs pour courir
        persoInput.PlayerControler.Run.started += onRun;
        persoInput.PlayerControler.Run.canceled += onRun;

    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        //Déplacement en marchant
        movement.x = -movementInput.x;
        movement.z = movementInput.y;

        //Vérifie que le perso bouge.
        isMoving = movementInput.x != 0 || movementInput.y != 0;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton(); // On ne presse que le shift pour courir, pas besoin de vecteur.
    }

    //Contrôle la rotation du personnage à l'aide de quaternion.
    void rotation()
    {
        //Initialisation du vecteur direction
        Vector3 directionToLookAt;

        directionToLookAt.x = cameraMovement.x;
        directionToLookAt.y = 0.0f; //On ne regarde pas en hauteur
        directionToLookAt.z = cameraMovement.z;

        //Rotation actuelle du perso : 
        Quaternion currentRotation = transform.rotation;


        if (isMoving) //Crée la rotation à partir de la direction dans laquelle va le perso
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

    Vector3 ConvertToCamera(Vector3 vectorToRotate) //Converti des coordonées mondes au coordonées de la caméra.
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        float currentYValue = vectorToRotate.y;

        //Enlève les y pour ignorer les angles de la caméra 
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

    //Vérifie que le InputManager est bien initialisé
    void OnEnable()
    {
        persoInput.PlayerControler.Enable();
    }

    //Vérifie que le InputManager est bien éteint
    void OnDisable()
    {
        persoInput.PlayerControler.Disable();
    }
    
}