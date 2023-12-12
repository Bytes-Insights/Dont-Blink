using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsPuzzleController : MonoBehaviour
{
    public GameObject renderPlane;
    public GameObject player;
    public Texture2D[] idleStates;
    public Texture2D standHereTexture;
    public Texture2D finishTexture;
    public PressurePad pressurePad;

    private Material renderPlaneMaterial;
    private bool active = false;
    private bool finished = false;

    private int currentIndex = 0;

    public void Start()
    {
        renderPlaneMaterial = renderPlane.GetComponent<MeshRenderer>().material;
    }

    public void Update()
    {
        if (finished)
        {
            return;
        }

        if (!active)
        {
            UpdateIdleState();
            return;
        }

        UpdateActiveState();
    }

    public void IncrementIndex(int index)
    {
        if (index != currentIndex)
        {
            return;
        }

        if (currentIndex == 3)
        {
            finished = true;
            renderPlaneMaterial.SetTexture("_DisplayTexture", finishTexture);
        } else
        {
            currentIndex++;
        }
    }

    private void UpdateActiveState()
    {
        if (!pressurePad.IsActivated())
        {
            active = false;
            currentIndex = 0;
            return;
        }
    }

    private void UpdateIdleState()
    {
        if (pressurePad.IsActivated())
        {
            active = true;
            renderPlaneMaterial.SetTexture("_DisplayTexture", standHereTexture);
            return;
        }

        Vector3 normal = (renderPlane.transform.rotation * Vector3.down);
        normal.y = 0;
        normal.Normalize();

        Vector3 direction = (player.transform.position - renderPlane.transform.position);
        direction.y = 0;
        direction.Normalize();

        float angle = Vector3.Angle(direction, normal);

        if (angle < 22.5F)
        {
            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[0]);
            return;
        }

        Vector3 crossProduct = Vector3.Cross(normal, direction);
        if (crossProduct.y < 0)
        {
            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[2]);
        }
        else
        {
            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[1]);
        }
    }

    public bool CanActivate(int index)
    {
        return !finished && pressurePad.IsActivated() && (currentIndex == index || currentIndex == index + 1);
    }
}
