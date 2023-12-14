using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

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
    public GameObject Ringo;
    public GameObject Kfc;

    public Material VHS;
    private RawImage AngelEyeLayout;
    private AudioSource audioData;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        audioData = GetComponent<AudioSource>();
        //Get VHS Material
        //FullScreenPassRendererFeature feature = (FullScreenPassRendererFeature) data.rendererFeatures.Find(renderFeature => (renderFeature.GetType() == typeof(FullScreenPassRendererFeature)));
        //VHS = feature.passMaterial;
        VHS.SetFloat("_Intensity", 9999.0f);
        //Get layout 
        AngelEyeLayout = gameOver.GetComponent<RawImage>();

        //Deactivate video 
        gameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if(!isActive)
        {
            isActive = true;
            //Freeze player
            playerControls.enabled = false;
            //Set white noise
            StartCoroutine(runAnimation(1f));
        }
    }

    private void ResetPositions()
    {
        //Select respawn room depending on angel that killed you
        if(Vector3.Distance(Ringo.transform.position, transform.position) < Vector3.Distance (Kfc.transform.position, transform.position))
        {
            transform.position = new Vector3(37.89f, 1.01f, 46.31f);
        }else
        {
            transform.position = new Vector3(-28.8f, 1.01f, 61f);
        }
        //Reset angel positions
        Ringo.transform.position = new Vector3(81f, 0f, -12.5f);
        Kfc.transform.position = new Vector3(-66.1f, 1.73f, -5.5f);
        isActive=false;
    }

    //Activates white noise
    IEnumerator runAnimation(float noiseTime)
    {
        gameOver.SetActive(true);
        AngelEyeLayout.texture = gameOverVideo;
        audioData.volume  = 0.05f;
        audioData.clip = noise;
        audioData.Play();
        VHS.SetFloat("_Intensity", 9999.0f);
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

        yield return new WaitForSeconds(noiseTime*3);

        newPosition = new Vector3(cameraPosition.x, cameraPosition.y, cameraPosition.z);
        Camera.main.transform.position = newPosition;
        newRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, cameraRotation.z);
        Camera.main.transform.rotation = newRotation;
        audioData.Stop();
        AngelEyeLayout.texture = null;
        
        gameOver.SetActive(false);
        playerControls.enabled = true;
        ResetPositions();
        VHS.SetFloat("_Intensity", 5000.0f);
    } 

    public bool IsActive()
    {
        return isActive;
    }
}
