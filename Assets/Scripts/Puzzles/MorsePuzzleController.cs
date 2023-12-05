using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsePuzzleController : MonoBehaviour
{
    public GameObject renderPlane;
    public GameObject player;
    public Texture2D[] idleStates;
    public Texture2D[] morseTextures;
    public Texture2D finishedTexture;
    public InteractionMediator mediator;

    private List<bool[]> morseStages = new List<bool[]>();
    private Material renderPlaneMaterial;
    private bool active = false;
    private int stage = 0;
    private int index = 0;
    private int textureCounter = 0;
    private bool finished = false;

    private void InitMorseStages()
    {
        morseStages.Add(new bool[] { false, false, false });
        morseStages.Add(new bool[] { true, true });
        morseStages.Add(new bool[] { false, false });
        morseStages.Add(new bool[] { false, true, false, false });
        morseStages.Add(new bool[] { false });
    }

    public void Start()
    {
        InitMorseStages();
        renderPlaneMaterial = renderPlane.GetComponent<MeshRenderer>().material;
    }

    private void SwitchToIdleStage()
    {
        active = false;
    }

    private void SwitchToActiveStage()
    {
        active = true;
    }

    private void FinishPuzzle()
    {
        finished = true;
        renderPlaneMaterial.SetTexture("_DisplayTexture", finishedTexture);
    }

    private bool CheckDistance()
    {
        Vector3 planePos = renderPlane.transform.position;
        planePos.y = 0;

        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;

        return (playerPos - planePos).magnitude < 4;
    }

    private void UpdateIdleState()
    {
        Vector3 normal = (renderPlane.transform.rotation * Vector3.down);
        normal.y = 0;
        normal.Normalize();

        Vector3 direction = (player.transform.position - renderPlane.transform.position);
        direction.y = 0;
        direction.Normalize();

        float angle = Vector3.Angle(direction, normal);

        if (angle < 22.5F)
        {
            if (CheckDistance())
            {
                SwitchToActiveStage();
                return;
            }

            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[0]);
            return;
        }

        Vector3 crossProduct = Vector3.Cross(normal, direction);
        if (crossProduct.y < 0)
        {
            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[2]);
        } else
        {
            renderPlaneMaterial.SetTexture("_DisplayTexture", idleStates[1]);
        }
    }

    private bool blinkStarted = false;
    private float blinkTime = 0F;
    private float openTime = 0F;

    private bool transitioning = false;
    private float transitionTime = 0F;

    private void ResetBlink()
    {
        blinkStarted = false;
        blinkTime = 0F;
        openTime = 0F;
    }

    private void UpdateActiveState()
    {
        if (stage >= 5)
        {
            FinishPuzzle();
            return;
        }

        if (!CheckDistance())
        {
            SwitchToIdleStage();
            return;
        }

        if (transitioning)
        {
            transitionTime += Time.deltaTime;

            if (transitionTime > 1F)
            {
                textureCounter++;
                stage++;
                index = 0;
                transitioning = false;
                transitionTime = 0F;
            }

            return;
        }

        renderPlaneMaterial.SetTexture("_DisplayTexture", morseTextures[textureCounter]);

        if (!blinkStarted)
        {
            if (mediator.EyesClosed())
            {
                blinkStarted = true;
            }
            return;
        }

        if (mediator.EyesClosed())
        {
            openTime = 0F;
            blinkTime += Time.deltaTime;
        } else
        {
            openTime += Time.deltaTime;
        }

        if (openTime >= 0.15F)
        {
            bool[] stageList = morseStages[stage];
            bool currentDetection = stageList[index];

            const float LONG_THRESHOLD = 0.5F;

            if ((currentDetection && blinkTime >= LONG_THRESHOLD) || (!currentDetection && blinkTime < LONG_THRESHOLD && blinkTime > 0.032F))
            {
                textureCounter++;

                if (index == stageList.Length - 1)
                {
                    transitioning = true;
                    transitionTime = 0F;
                    renderPlaneMaterial.SetTexture("_DisplayTexture", morseTextures[textureCounter]);
                } else
                {
                    index++;
                }
            } else {
                textureCounter -= index;
                index = 0;
                renderPlaneMaterial.SetTexture("_DisplayTexture", morseTextures[textureCounter]);
            }

            ResetBlink();
        }
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
}
