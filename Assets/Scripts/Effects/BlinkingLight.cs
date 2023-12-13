using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    Light myLight;
    float timer;
    bool flicker;
    int flickers;
    Color originalColor;
    Material myMaterial;
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.time;
        flicker = false;
        flickers = 0;
        myLight = GetComponent<Light>();
        Renderer renderer = transform.parent.GetComponent<Renderer>();
        if (renderer != null)
        {
            myMaterial = renderer.material;
            originalColor = myMaterial.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (flicker)
            Flicker(Random.Range(2, 5));
        else
            Sleep(Random.Range(1f, 3f));

        
    }

    void Flicker(int times)
    {
        if (Time.time - timer < 0.1f) // Adjusted the flicker time to be shorter
        {
            myLight.intensity = 0f; // Randomize intensity during flicker
            myMaterial.color = Color.black;
            myMaterial.SetColor("_EmissionColor", Color.black);
        }
        else if (Time.time - timer < Random.Range(0.2f, 0.3f))
        {
            myLight.intensity = 15f; // Set intensity back to normal after the flicker
            myMaterial.color = originalColor;
            myMaterial.SetColor("_EmissionColor", originalColor);
        }
        else
        {
            timer = Time.time;
            flickers++;
        }

        if(flickers == times)
        {
            flickers = 0;
            flicker = false;
            timer = Time.time;
        }
            
    }

    void Sleep(float time)
    {
        myLight.intensity = Random.Range(14f, 15f); // Randomize intensity slightly for a more realistic flicker
        myMaterial.color = originalColor;
            myMaterial.SetColor("_EmissionColor", originalColor);
        if (Time.time - timer >= time)
        {
            
            flicker = true;
            timer = Time.time;
        }
    }
}
