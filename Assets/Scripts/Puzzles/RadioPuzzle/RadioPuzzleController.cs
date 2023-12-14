using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioPuzzleController : MonoBehaviour
{
    // Audio clip indices.
    private const int IDX_STATIC = 0;
    private const int IDX_LOADING = 1;
    private const int IDX_EIGHTY_NINE = 2;
    private const int IDX_NINETY_EIGHT = 3;
    private const int IDX_AFRAID = 4;
    private const int IDX_RICK = 5;

    public Dial dial;
    public GameObject player;
    public float activationDistance = 3F;
    public float tolerance = 0.5F;
    public float completeTime = 2F;
    public float[] targetFrequencies;
    public GameObject[] stageIndicators;
    public GameObject[] audioClips;

    private int currentAudioClip = -1;
    private bool easterEggActivated = false;
    private float easterEggTimeLeft = 10F;
    private bool active = false;
    private bool finished = false;
    private int stage = 0;
    private bool inZone = false;
    private float timeLeft = 0F;

    void Start()
    {
        dial.gameObject.SetActive(false);
        HideIndicators();
        DisableAllAudioClips();
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

    bool IsAtFrequency(float targetFrequency)
    {
        float rotation = dial.GetRotation();
        float normalized = (rotation + 90) / 180;
        float frequency = 87F + (normalized * (105 - 87));
        return Mathf.Abs(frequency - targetFrequency) < tolerance;
    }

    void UpdateActive()
    {
        if ((player.transform.position - gameObject.transform.position).magnitude > activationDistance)
        {
            ChangeActivation(false);
            return;
        }

        if (!easterEggActivated)
        {
            if (IsAtFrequency(87))
            {
                easterEggTimeLeft -= Time.deltaTime;
                if (easterEggTimeLeft <= 0)
                {
                    easterEggActivated = true;
                }
            }
            else
            {
                easterEggTimeLeft = 10F;
            }
        }

        float targetFrequency = targetFrequencies[stage];
        bool inRange = IsAtFrequency(targetFrequency);

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

    void ApplyAudioClip(int index)
    {
        if (currentAudioClip != -1)
        {
            audioClips[currentAudioClip].SetActive(false);
        }

        currentAudioClip = index;

        audioClips[currentAudioClip].SetActive(true);
    }

    void DisableAllAudioClips()
    {
        foreach (GameObject clip in audioClips)
        {
            clip.SetActive(false);
        }
    }

    void UpdateAudioClip()
    {
        if (finished)
        {
            if (currentAudioClip != IDX_AFRAID)
            {
                ApplyAudioClip(IDX_AFRAID);
            }
            return;
        }

        if (active && IsAtFrequency(targetFrequencies[stage]))
        {
            if (currentAudioClip != IDX_LOADING)
            {
                ApplyAudioClip(IDX_LOADING);
            }
        }
        else if (stage > 0 && IsAtFrequency(targetFrequencies[0]))
        {
            if (currentAudioClip != IDX_EIGHTY_NINE)
            {
                ApplyAudioClip(IDX_EIGHTY_NINE);
            }
        }
        else if (stage > 1 && IsAtFrequency(targetFrequencies[1]))
        {
            if (currentAudioClip != IDX_NINETY_EIGHT)
            {
                ApplyAudioClip(IDX_NINETY_EIGHT);
            }
        } else if (easterEggActivated && IsAtFrequency(87))
        {
            if (currentAudioClip != IDX_RICK)
            {
                ApplyAudioClip(IDX_RICK);
            }
        } else if (active)
        {
            if (currentAudioClip != IDX_STATIC)
            {
                ApplyAudioClip(IDX_STATIC);
            }
        } else
        {
            if (currentAudioClip != -1)
            {
                currentAudioClip = -1;
                DisableAllAudioClips();
            }
        }
    }


    void Update()
    {
        UpdateAudioClip();

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
