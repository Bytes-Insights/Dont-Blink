using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will keep track of input state for certain special interactions and supply events.
/// This way we can decouple the interaction modality from the interaction response.
/// </summary>
public class InteractionMediator : MonoBehaviour
{
    // --- STATE VARIABLES. ---
    private bool faceTracked = false;
    private bool eyesClosed = false;

    // --- EVENT DEFINITIONS. ---

    // Will trigger whenever OpenCV reports a change in whether a face is trackable or not.
    public delegate void FaceTrackedChangedHandler(bool tracked);
    public event FaceTrackedChangedHandler OnFaceTrackedChanged;

    // Will trigger whenever a threshold crossover for blinking is detected.
    public delegate void EyesStatusChangedHandler(bool closed);
    public event EyesStatusChangedHandler OnEyesStatusChanged;

    // Will trigger when a certain keyword is detected by speech recognition.
    public delegate void PasswordSpokenHandler();
    public event PasswordSpokenHandler OnPasswordSpoken;

    // --- API FOR INTERACTION MODALITIES. ---

    public void PasswordSpoken()
    {
        OnPasswordSpoken.Invoke();
    }

    public void SetFaceTracked(bool tracked)
    {
        faceTracked = tracked;
        OnFaceTrackedChanged.Invoke(tracked);
    }

    public void SetEyesStatus(bool closed)
    {
        eyesClosed = closed;
        OnEyesStatusChanged.Invoke(closed);
    }

    // --- API FOR INTERACTION RESPONSES. ---

    public bool FaceTracked()
    {
        return faceTracked;
    }

    public bool EyesClosed()
    {
        return eyesClosed;
    }
}
