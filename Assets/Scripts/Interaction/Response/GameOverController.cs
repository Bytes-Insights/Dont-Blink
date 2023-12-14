using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverController : MonoBehaviour
{
    public GameObject gameOver;
    public UniversalRendererData data;
    public PlayerControls playerControls;
    public Texture gameOverVideo;
    public Texture brokenGlass;
    public VideoPlayer videoPlayer;
    public AudioClip chocked; 
    public AudioClip noise;

    private Material VHS;
    private RawImage AngelEyeLayout;
    private AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        //Get VHS Material
        FullScreenPassRendererFeature feature = (FullScreenPassRendererFeature) data.rendererFeatures.Find(renderFeature => (renderFeature.GetType() == typeof(FullScreenPassRendererFeature)));
        VHS = feature.passMaterial;

        //Get layout 
        AngelEyeLayout = gameOver.GetComponent<RawImage>();

        //Deactivate video 
        gameOver.SetActive(false);

        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        //Freeze player
        playerControls.enabled = false;
        //Set white noise
        StartCoroutine(runAnimation(1f));
    }

    //Activates white noise
    IEnumerator runAnimation(float noiseTime)
    {
        audioData.volume  = 0.05f;
        audioData.clip = noise;
        audioData.Play();
        VHS.SetFloat("_Intensity", 9999.0f);
        gameOver.SetActive(true);
        //Make effect invisible
        AngelEyeLayout.color = Color.black;
        
        //Turn down noise
        yield return new WaitForSeconds(noiseTime);
        audioData.Stop();
        VHS.SetFloat("_Intensity", 5000.0f);

        //Make effect visible
        AngelEyeLayout.color = Color.white;
        
        //Turn up noise
        yield return new WaitForSeconds(noiseTime);
        audioData.Play();
        VHS.SetFloat("_Intensity", 9999.0f);

        //Make effect invisible
        AngelEyeLayout.color = Color.black;

        //Move to floor 
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 newPosition = new Vector3(cameraPosition.x, cameraPosition.y - 1.3f, cameraPosition.z);
        Camera.main.transform.position = newPosition;

        //Rotate Camera
        Vector3 cameraRotation = Camera.main.transform.rotation.eulerAngles;
        Quaternion newRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, cameraRotation.z + 60.0f);
        Camera.main.transform.rotation = newRotation;

        //Turn down noise
        yield return new WaitForSeconds(noiseTime);
        VHS.SetFloat("_Intensity", 5000.0f);

        //Make broken glass visible
        AngelEyeLayout.texture = brokenGlass;

        audioData.Stop();
        audioData.volume  = 1f;
        audioData.clip = chocked;
        audioData.Play();
        
        //Make effect visible
        AngelEyeLayout.color = Color.white;

        yield return new WaitForSeconds(noiseTime);
        audioData.Stop();
        audioData.volume  = 0.05f;
        audioData.clip = noise;
        audioData.Play();
        VHS.SetFloat("_Intensity", 9999.0f);
        AngelEyeLayout.color = Color.black;
        AngelEyeLayout.texture = gameOverVideo;
    } 
}
