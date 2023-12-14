using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AngelVHSEffect : MonoBehaviour
{
    public UniversalRendererData data;
    public GameOverController gameOver;

    private GameObject[] angels;
    
    void Start()
    {
        angels = GameObject.FindGameObjectsWithTag("Angel");
    }

    void Update()
    {
        if(!gameOver.IsActive())
        {
            FullScreenPassRendererFeature feature = (FullScreenPassRendererFeature) data.rendererFeatures.Find(renderFeature => (renderFeature.GetType() == typeof(FullScreenPassRendererFeature)));
        
            if (feature == null)
            {
                return;
            }

            float closestDistance = 10F;

            foreach (GameObject angel in angels)
            {
                float dist = (angel.transform.position - this.transform.position).magnitude;
                if (dist < closestDistance) {
                    closestDistance = dist;
                }
            }

            float distance = Mathf.Min(10, Mathf.Max(closestDistance, 0.1F));
            float desiredIntensity = 1F;

            if (closestDistance < 10F)
            {
                desiredIntensity = -3.1F * (distance * distance * distance * distance) + 64.13F * (distance * distance * distance) -
                    485.25F * (distance * distance) + 1080.81F * distance + 9143.37F;
            }

            float intensity = Mathf.Min(9999F, Mathf.Max(1, desiredIntensity));

            Material mat = feature.passMaterial;
            mat.SetFloat("_Intensity", intensity);
        }
    }
}
