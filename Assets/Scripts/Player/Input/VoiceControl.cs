using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whisper.Utils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class VoiceControl : MonoBehaviour
{
    public Whisper.WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public PlayerInputActions inputActions;
    public GameObject indicator;

    private Whisper.WhisperStream _stream;
    private InputAction confirm;

    public void Activate()
    {
        indicator.SetActive(true);

        confirm.Enable();
        confirm.performed += Record;
    }

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
        }
        confirm = inputActions.Actions.Confirm;
    }

    void OnDisable()
    {
        confirm.Disable();
    }

    private async void Start()
    {
        indicator.SetActive(false);
        _stream = await whisper.CreateStream(microphoneRecord);
        _stream.OnResultUpdated += OnResult;

    }

    private void Record(InputAction.CallbackContext ctx)
    {
        if (!microphoneRecord.IsRecording)
        {
            indicator.SetActive(false);
            _stream.StartStream();
            microphoneRecord.StartRecord();
        }
        else
        {
            indicator.SetActive(true);
            microphoneRecord.StopRecord();
        }
    }

    private void OnResult(string result)
    {
        if(result.Contains("I am not afraid"))
        {
            Debug.Log("correct");
        }
    }
}