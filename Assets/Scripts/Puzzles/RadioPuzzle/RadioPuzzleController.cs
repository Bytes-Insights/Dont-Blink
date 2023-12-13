using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPuzzleController : MonoBehaviour
{
    public Dial dial;
    public GameObject player;
    public float activationDistance = 3F;
    public float tolerance = 0.5F;
    public float completeTime = 2F;
    public float[] targetFrequencies;
    public GameObject[] stageIndicators;

    private bool active = false;
    private bool finished = false;
    private int stage = 0;
    private bool inZone = false;
    private float timeLeft = 0F;

    void Start()
    {
        dial.gameObject.SetActive(false);
        HideIndicators();
    }

    void HideIndicators()
    {
        foreach (GameObject indicator in stageIndicators)
        {
            indicator.SetActive(false);
        }
    }

    void ChangeActivation(bool activate)
    {
        active = activate;
        dial.gameObject.SetActive(activate);

        if (activate && stage < stageIndicators.Length)
        {
            stageIndicators[stage].SetActive(true);
        } else if (!activate)
        {
            HideIndicators();
        }
    }

    void UpdateInactive()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude <= activationDistance)
        {
            ChangeActivation(true);
            return;
        }
    }

    void Finish()
    {
        finished = true;
        ChangeActivation(false);
    }

    void ProgressStage()
    {
        stageIndicators[stage].SetActive(false);

        if (stage == 2)
        {
            Finish();
            return;
        }

        stage++;
        stageIndicators[stage].SetActive(true);
    }

    void UpdateActive()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude > activationDistance)
        {
            ChangeActivation(false);
            return;
        }

        float rotation = dial.GetRotation();
        float normalized = (rotation + 90) / 180;
        float frequency = 87F + (normalized * (105 - 87));

        float targetFrequency = targetFrequencies[stage];
        bool inRange = Mathf.Abs(frequency - targetFrequency) < tolerance;

        if (!inZone && inRange)
        {
            inZone = true;
            timeLeft = completeTime;
        } else if (inZone && !inRange)
        {
            inZone = false;
        } else if (inZone && inRange)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                ProgressStage();
            }
        }
    }

    void Update()
    {
        if (finished)
        {
            return;
        }

        if (!active)
        {
            UpdateInactive();
            return;
        }

        UpdateActive();
    }
}
