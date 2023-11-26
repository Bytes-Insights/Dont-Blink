using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetupInputController : MonoBehaviour
{
    public UIDocument document;
    public InteractionMediator mediator;
    public BlinkingProvider blinking;
    public float minOpenTimeForReset = 0.1F;

    private bool lastCapturedEyeState = false;
    private float openTime = 0F;
    private int blinkCount = 0;
    private string Slider = "SliderValue";
    private string Sensitivity = "SensitivityValue";

    private float sensitivityValue;

    void Start()
    {
        sensitivityValue = document.rootVisualElement.Q<Slider>(Slider).value;
    }

    void Update()
    {
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
    }
}
