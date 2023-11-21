using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Replacement for the blinking interaction for debugging.
/// </summary>
public class UIButtonsModality : MonoBehaviour
{
    private const string CLOSE_EYES_BUTTON = "CloseEyesButton";
    private const string OPEN_EYES_BUTTON = "OpenEyesButton";
    private const string TRACK_FACE_BUTTON = "TrackFaceButton";
    private const string UNTRACK_FACE_BUTTON = "UntrackFaceButton";
    private const string SPEAK_PASSWORD_BUTTON = "SpeakPasswordButton";

    public UIDocument explanationDocument;
    public InteractionMediator mediator;

    void Start()
    {
        VisualElement root = explanationDocument.GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>(CLOSE_EYES_BUTTON).clicked += CloseEyes;
        root.Q<Button>(OPEN_EYES_BUTTON).clicked += OpenEyes;
        root.Q<Button>(TRACK_FACE_BUTTON).clicked += TrackFace;
        root.Q<Button>(UNTRACK_FACE_BUTTON).clicked += UntrackFace;
        root.Q<Button>(SPEAK_PASSWORD_BUTTON).clicked += SpeakPassword;
    }

    void CloseEyes()
    {
        mediator.SetEyesStatus(true);
    }

    void OpenEyes()
    {
        mediator.SetEyesStatus(false);
    }

    void TrackFace()
    {
        mediator.SetFaceTracked(true);
    }

    void UntrackFace()
    {
        mediator.SetFaceTracked(false);
    }

    void SpeakPassword()
    {
        mediator.PasswordSpoken();
    }
}
