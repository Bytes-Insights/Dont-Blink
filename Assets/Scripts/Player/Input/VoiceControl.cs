using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whisper.Utils;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Whisper.Samples
{
public class VoiceControl : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public PlayerInputActions inputActions;

    private WhisperStream _stream;
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
        _stream = await whisper.CreateStream(microphoneRecord);
        _stream.OnResultUpdated += OnResult;

        Debug.Log("READY!");
    }

    private void Record(InputAction.CallbackContext ctx)
    {
        if (!microphoneRecord.IsRecording)
        {
            _stream.StartStream();
            microphoneRecord.StartRecord();
            Debug.Log("Recording");
        }
        else{
            microphoneRecord.StopRecord();
            Debug.Log("Stopped");
        }
    }

    private void OnResult(string result)
    {
        Debug.Log(result);
        if(result.Contains("I am not afraid"))
        {
            Debug.Log("correct");
            //TODO
        }
    }


}
}