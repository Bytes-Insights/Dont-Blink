using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Whisper.Utils;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class VoiceControl : MonoBehaviour
{
    public Whisper.WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public PlayerInputActions inputActions;
    public GameObject indicator;
    public UniversalRendererData data;
    public AudioClip noise;
    public AudioClip endClip;
    public GameObject gameOver;
    public RawImage rawImage;

    private Whisper.WhisperStream _stream;
    private InputAction confirm;
    private AudioSource source;
    private Material VHS;
    private bool isActive = false;

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
        FullScreenPassRendererFeature feature = (FullScreenPassRendererFeature) data.rendererFeatures.Find(renderFeature => (renderFeature.GetType() == typeof(FullScreenPassRendererFeature)));
        VHS = feature.passMaterial;
        source = GetComponent<AudioSource>();
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
        if (result.ToLower().Contains("i am not afraid") || result.ToLower().Contains("i'm not afraid"))
        {
            StartCoroutine(runAnimation());
        }
    }

    IEnumerator runAnimation()
    {
        isActive = true;

        VHS.SetFloat("_Intensity", 0.0f);
        source.clip = endClip;
        source.Play();
        yield return new WaitForSeconds(5.0f);
        gameOver.SetActive(true);
        rawImage.texture = null;
        source.volume  = 0.05f;
        source.clip = noise;
        source.Play();

        rawImage.color = Color.black;
        VHS.SetFloat("_Intensity", 9999.0f);
        yield return new WaitForSeconds(3.0f);
        source.Stop();
        VHS.SetFloat("_Intensity", 7000.0f);
        SceneManager.LoadScene("Congratulations", LoadSceneMode.Single);
    }

    public bool IsActive()
    {
        return isActive;
    }
}