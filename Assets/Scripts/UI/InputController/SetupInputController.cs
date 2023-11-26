using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

public class SetupInputController : MonoBehaviour
{
    public UIDocument document;
    public InteractionMediator mediator;
    public BlinkingProvider blinking;
    public BlinkModality blinkingScript;
    public float minOpenTimeForReset = 0.1F;

    private bool lastCapturedEyeState = false;
    private float openTime = 0F;
    private int blinkCount = 0;
    private string Slider = "SliderValue";
    private string Sensitivity = "SensitivityValue";
    private float sensitivityValue;
    private float maxValue; 
    private float minValue;

    public PlayerInputs playerControls;
    private InputAction move;
    private InputAction confirm;
    private Vector2 moveDirection;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        confirm = playerControls.Player.Confirm;
        confirm.performed += SetSensibility;
        confirm.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    void Start()
    {
        minValue = document.rootVisualElement.Q<Slider>(Slider).lowValue;
        maxValue = document.rootVisualElement.Q<Slider>(Slider).highValue;
        sensitivityValue = document.rootVisualElement.Q<Slider>(Slider).value;
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();

        document.rootVisualElement.Q<Slider>(Slider).value += Time.deltaTime * 3 * moveDirection.x;

        if (mediator.EyesClosed() && !lastCapturedEyeState)
        {
            lastCapturedEyeState = true;
            blinking.closeEye();

        } else if (!mediator.EyesClosed() && lastCapturedEyeState)
        {
            openTime += Time.deltaTime;
            if (openTime > minOpenTimeForReset)
            {
                lastCapturedEyeState = false;
                openTime = 0F;
                blinking.openEye();
            }
        }

        sensitivityValue = document.rootVisualElement.Q<Slider>(Slider).value;
        document.rootVisualElement.Q<Label>(Sensitivity).text = sensitivityValue.ToString("0.0");
        blinkingScript.updateSensitivity(sensitivityValue);
    }

    private void SetSensibility(InputAction.CallbackContext ctx)
    {
        DataTransfer.instance.valueToPass = sensitivityValue;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
