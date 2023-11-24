using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    public bool useController = false;
    public float rotationFactorPerFrame = 0.25f;
    public float standardSpeed = 5.0f;
    public PlayerInputs playerControls;
    
    Camera _camera;
    private CharacterController characterController;
    private Vector3 currentMovement;
    private Vector2 moveDirection;
    private InputAction move;
    private InputAction shift;
    private InputAction rotation;
    private bool isShifted = false;
    private float playerSpeed;
    

    //Mandatory functions for InputAction to work flawless
    private void Awake()
    {
        playerControls = new PlayerInputs();
        characterController = gameObject.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        shift = playerControls.Player.Shift;
        shift.Enable();
        shift.performed += Shift;

        if(!useController)
            rotation = playerControls.Player.Look;
        else
            rotation = playerControls.Player.LookGamepad;
        rotation.Enable();
    }
    
    private void OnDisable()
    {
        move.Disable();
        rotation.Disable();
        shift.Disable();
    }

    private void Shift(InputAction.CallbackContext ctx)
    {
        isShifted = !isShifted;

        if(isShifted)
            playerSpeed = standardSpeed / 3.0f;
        else
            playerSpeed = standardSpeed;
    }

    void HandleRotation()
    {
        
        Vector2 mousePosition = rotation.ReadValue<Vector2>();
        
        if(!useController){
        if(mousePosition.x < 0.0f || mousePosition.x > Screen.width)
            mousePosition.x = 0.0f;
        else
            mousePosition.x = (mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f);
        }
        transform.eulerAngles += rotationFactorPerFrame * new Vector3(0, mousePosition.x, 0);
        
    }

    void Start()
    {
        playerSpeed = standardSpeed;
        _camera = Camera.main;
    }

    // Move player depending on WASD state
    void Update()
    {
        HandleRotation();
        moveDirection = move.ReadValue<Vector2>();

        currentMovement.x = moveDirection.x * playerSpeed * Time.deltaTime;
        currentMovement.z = moveDirection.y * playerSpeed * Time.deltaTime;

        Vector3 movement = new Vector3(currentMovement.x, 0, currentMovement.z);
        movement = transform.TransformDirection(movement);
        characterController.Move(movement);
    }
}
