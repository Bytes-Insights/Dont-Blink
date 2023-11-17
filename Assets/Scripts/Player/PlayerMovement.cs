using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    public float rotationFactorPerFrame = 5.0f;
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

        rotation = playerControls.Player.Look;
        rotation.Enable();
    }
    
    private void OnDisable()
    {
        move.Disable();
        shift.Disable();
    }

    private void Shift(InputAction.CallbackContext ctx)
    {
        isShifted = !isShifted;

        if(isShifted)
            playerSpeed = standardSpeed / 3;
        else
            playerSpeed = standardSpeed;
    }

    void HandleRotation()
    {
        /*
        Vector2 mousePosition = playerControls.Player.Look.ReadValue<Vector2>();
        mousePosition /= new Vector2(Screen.width, Screen.height);
        mousePosition -= new Vector2(mousePosition.x - 0.5f, mousePosition.y - 0.5f);
        var mousePositionZ = _camera.farClipPlane * 0.5f;
        Vector3 mouseViewportPosition = _camera.ViewportToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, _camera.transform.position.z));

        if(mousePosition.x > 1.5 || mousePosition.x < -0.5)
            mousePosition.x = 0.5f;
        if(mousePosition.y > 1.5 || mousePosition.y < -0.5)
            mousePosition.y = 0.5f;

        Debug.Log("MousePos: " + mousePosition);

        Vector3 positionToLookAt;
        positionToLookAt.x = transform.position.x + mousePosition.x;
        positionToLookAt.y = transform.position.y + mousePosition.y;
        //positionToLookAt.z = currentMovement.z;
        positionToLookAt.z = 10.0f;

        transform.Rotate(mousePosition.y, mousePosition.x, 0.0f);
        */
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
        characterController.Move(new Vector3(currentMovement.x, 0, currentMovement.z));
    }
}
