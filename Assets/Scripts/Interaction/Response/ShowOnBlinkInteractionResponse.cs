using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a demo interaction response that will hide a linked game object and
/// show it whenever the user closes their eyes.
/// </summary>
public class ShowOnBlinkInteractionResponse : MonoBehaviour
{
    public InteractionMediator mediator;
    public GameObject linkedObject;

    void Start()
    {
        linkedObject.SetActive(false);
        mediator.OnEyesStatusChanged += HandleEyeStatusChange;
    }

    void OnDestroy()
    {
        mediator.OnEyesStatusChanged -= HandleEyeStatusChange;
    }

    void HandleEyeStatusChange(bool closed)
    {
        linkedObject.SetActive(closed);
    }
}
