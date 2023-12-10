using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameOverController : MonoBehaviour
{
    public WebcamSupplier supplier;
    public GameObject gameOver;
    public Canvas webCanvas;
    public UniversalRendererData data;
    public PlayerControls playerControls;
    public Texture gameOverVideo;
    public VideoPlayer videoPlayer;

    private Material VHS;
    private RawImage AngelEyeLayout;

    // Start is called before the first frame update
    void Start()
    {
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
        //Deactivate webcam
        supplier.enabled = false;
        webCanvas.GetComponent<UIDocument>().enabled = false;
        //Set white noise
        StartCoroutine(runAnimation(2f));
    }

    //Activates white noise
    IEnumerator runAnimation(float noiseTime)
    {
        VHS.SetFloat("_Intensity", 9999.0f);
        gameOver.SetActive(true);
        AngelEyeLayout.color = Color.black;
        //Play eye video
        yield return new WaitForSeconds(noiseTime);
        
        VHS.SetFloat("_Intensity", 5000.0f);
        AngelEyeLayout.color = Color.white;

        // Start playing the video
        //videoPlayer.Play();
        //Play eye video
        
        yield return new WaitForSeconds(noiseTime);
        VHS.SetFloat("_Intensity", 9999.0f);
        AngelEyeLayout.color = Color.black;
        
        yield return new WaitForSeconds(noiseTime);
        gameOver.SetActive(false);
        AngelEyeLayout.texture = null;
        VHS.SetFloat("_Intensity", 5000.0f);
    } 
}
