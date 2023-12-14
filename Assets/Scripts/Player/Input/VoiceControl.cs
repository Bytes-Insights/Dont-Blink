using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whisper.Utils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class VoiceControl : MonoBehaviour
{
    //public WhisperManager whisper;
    //public MicrophoneRecord microphoneRecord;
    public PlayerInputActions inputActions;

    //private WhisperStream _stream;
    private InputAction confirm;

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
        }
    
        confirm = inputActions.Actions.Confirm;
        confirm.Enable();
        confirm.performed += Record;
    }

    void OnDisable()
    {
        confirm.Disable();
    }
    // Start is called before the first frame update
    private async void Start()
    {
        /*_stream = await whisper.CreateStream(microphoneRecord);
        _stream.OnResultUpdated += OnResult;
        _stream.OnSegmentUpdated += OnSegmentUpdated;
        _stream.OnSegmentFinished += OnSegmentFinished;
        _stream.OnStreamFinished += OnFinished;

        microphoneRecord.OnRecordStop += OnRecordStop;
        button.onClick.AddListener(OnButtonPressed);*/
    }

    private void Record(InputAction.CallbackContext ctx)
    {
        Debug.Log("Pressed");
    }
}
