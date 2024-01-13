using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControls : MonoBehaviour
{
    public PlayerInputActions inputActions;
    public Rigidbody rigidbody;
    public Camera camera;
    
    public WordsUIController wordsController;
    public float speed = 2F;
    public float lookSensitivity = 25F;

    private InputAction wordMenu;
    private bool wordMenuShown = false;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
        }

        inputActions.Camera.Enable();
        inputActions.Movement.Enable();

        wordMenu = inputActions.Actions.WordsMenu;
        wordMenu.Enable();
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        
        inputActions.Camera.Disable();
        inputActions.Movement.Disable();
    }

    void UpdateMovement()
    {
        float forward = inputActions.Movement.Forward.ReadValue<float>();
        float right = inputActions.Movement.Right.ReadValue<float>();

        // TODO: Instead of using rigidbody.velocity we might want to use the movement logic from the old script
        //       But then we should make sure that the rigidbody itself has the X and Z position locked.
        //       We should keep it for the Y direction, as that will ensure our character is properly grounded.
        Vector3 movement = (transform.right * right + transform.forward * forward).normalized * speed;
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);
    }

    private float xRot = 0;

    void UpdateCamera()
    {
        float lookSpeed = lookSensitivity;
        float horizontal = Mathf.Min(inputActions.Camera.Horizontal.ReadValue<float>() * lookSpeed);
        float vertical = inputActions.Camera.Vertical.ReadValue<float>() * lookSpeed;
        Debug.Log(horizontal);
        xRot -= vertical;
        xRot = Mathf.Clamp(xRot, -90F, 90F);

        camera.transform.localRotation = Quaternion.Euler(xRot, 0F, 0F);
        transform.Rotate(Vector3.up * horizontal);
    }

    void Update()
    {
        UpdateCamera();
        UpdateMovement();

        if(wordMenu.ReadValue<float>() > 0.5)
        {
            wordMenuShown = true;
        }else{
            wordMenuShown = false;
        }
        WordMenu();
    }

    void WordMenu()
    {
        wordsController.show(wordMenuShown);
    }
}
