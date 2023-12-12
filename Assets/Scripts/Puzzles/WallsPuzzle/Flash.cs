using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    private const string KEY_INTENSITY = "_Intensity";

    public GameObject renderPlane;
    public float flashDuration = 2F;
    public bool permanent = false;
    
    private Material renderPlaneMaterial;
    private float flashTime = 0F;
    private float flashValue = 0F;
    private bool flashTriggered = false;

    private void UpdateFlashMaterial()
    {
        renderPlaneMaterial.SetFloat(KEY_INTENSITY, flashValue);
    }

    private void Reset()
    {
        flashTriggered = false;
        flashValue = 0F;
        flashTime = 0F;
        UpdateFlashMaterial();
    }

    public void DoFlash()
    {
        if (permanent)
        {
            renderPlaneMaterial.SetFloat(KEY_INTENSITY, 1.0F);
            return;
        }

        flashTime = flashDuration;
        flashTriggered = true;
    }

    public void Start()
    {
        renderPlaneMaterial = renderPlane.GetComponent<MeshRenderer>().material;
        UpdateFlashMaterial();
    }

    public void Update()
    {
        if (!flashTriggered || permanent)
        {
            return;
        }

        if (flashTime >= 0)
        {
            flashValue = flashTime / flashDuration;

            UpdateFlashMaterial();
            
            flashTime -= Time.deltaTime;
        } else
        {
            Reset();
        }
    }
}
