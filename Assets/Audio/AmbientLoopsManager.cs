using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientLoopsManager : MonoBehaviour
{
    public AudioClip dayAmbient;
    public AudioClip nightAmbient;
    public AudioSource ambientLoop;
    void Awake()
    {
        EnvironmentEvents.Singleton.RegisterTimeChangedAction(SelectAmbientLoop);
        EnvironmentEvents.Singleton.RegisterTimeChangedAction(StartPlayingAmbientLoop);
        PlayerEvents.Singleton.RegisterPlayerChasedStartActions(StopImmediately);
        PlayerEvents.Singleton.RegisterPlayerChasedEndActions(StartPlayingAmbientLoop);
        PlayerEvents.Singleton.RegisterStealthStartActions(StopPlayingAmbientLoop);
        PlayerEvents.Singleton.RegisterStealthEndActions(StartPlayingAmbientLoop);
    }

    // Update is called once per frame
    void SelectAmbientLoop()
    {
        ambientLoop.clip = dayAmbient;
        ambientLoop.Play();
    }

    private void StopImmediately()
    {
        CancelInvoke("DecreaseVolume");
        CancelInvoke("IncreaseVolume");
        ambientLoop.volume = 0f;
    }

    private void StartPlayingAmbientLoop()
    {
        InvokeRepeating("IncreaseVolume", 0.05f, 0.25f);
    }

    private void StopPlayingAmbientLoop()
    {
        InvokeRepeating("DecreaseVolume", 0.05f, 0.25f);
    }

    private void IncreaseVolume()
    {
        CancelInvoke("DecreaseVolume");

        if (ambientLoop.volume >= 1f)
        {
            CancelInvoke("IncreaseVolume");
            return;
        }

        ambientLoop.volume += 0.05f;
    }

    private void DecreaseVolume()
    {
        CancelInvoke("IncreaseVolume");

        if (ambientLoop.volume <= 0f)
        {
            CancelInvoke("DecreaseVolume");
            return;
        }

        ambientLoop.volume -= 0.05f;
    }
}
