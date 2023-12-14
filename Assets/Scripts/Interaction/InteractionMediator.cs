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
    private float faceDirectionalWeight = 0F;
    private int faceCount = 0;
    public FaceRecognitionController faceRecognitionController;

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

    public void SetFaceDirectionalWeight(float f)
    {
        faceDirectionalWeight = f;
    }

    public void SetFaceAmount(int faces)
    {
        faceCount = faces;
    }

    public void PasswordSpoken()
    {
        if (OnPasswordSpoken != null)
        {
            OnPasswordSpoken.Invoke();
        } else
        {
            Debug.LogWarning("Password spoken but no event handler was present!");
        }
    }

    public void SetFaceTracked(bool tracked)
    {
        faceTracked = tracked;
        faceRecognitionController.updateFace(tracked);
        if (OnFaceTrackedChanged != null)
        {
            OnFaceTrackedChanged.Invoke(tracked);
        } else
        {
            Debug.LogWarning("Face track changed but no event handler was present!");
        }
    }

    public void SetEyesStatus(bool closed)
    {
        eyesClosed = closed;
        if (OnEyesStatusChanged != null)
        {
            OnEyesStatusChanged.Invoke(closed);
        }
        {
            Debug.LogWarning("Eye status changed but no event handler was present!");
        }
    }

    // --- API FOR INTERACTION RESPONSES. ---

    public int FaceAmount()
    {
        return faceCount;
    }

    public bool FaceTracked()
    {
        return faceTracked;
    }

    public bool EyesClosed()
    {
        return eyesClosed;
    }

    public float FaceDirectionalWeight()
    {
        return faceDirectionalWeight;
    }
}
