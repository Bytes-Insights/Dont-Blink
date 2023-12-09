using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public PlayerInputActions inputActions;
    public Rigidbody rigidbody;
    public Camera camera;

    public float speed = 2F;
    public float lookSensitivity = 25F;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
        }

        inputActions.Camera.Enable();
        inputActions.Movement.Enable();
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

        Vector3 movement = (transform.right * right + transform.forward * forward) * speed;
        rigidbody.velocity = new Vector3(movement.x, rigidbody.velocity.y, movement.z);
    }

    private float xRot = 0;

    void UpdateCamera()
    {
        float lookSpeed = lookSensitivity * Time.deltaTime;
        float horizontal = inputActions.Camera.Horizontal.ReadValue<float>() * lookSpeed;
        float vertical = inputActions.Camera.Vertical.ReadValue<float>() * lookSpeed;

        xRot -= vertical;
        xRot = Mathf.Clamp(xRot, -90F, 90F);

        camera.transform.localRotation = Quaternion.Euler(xRot, 0F, 0F);
        transform.Rotate(Vector3.up * horizontal);
    }

    void Update()
    {
        UpdateMovement();
        UpdateCamera();
    }
}
