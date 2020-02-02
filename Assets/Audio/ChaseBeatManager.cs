using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBeatManager : MonoBehaviour
{
    public AudioSource chaseBeat;

    void Start()
    {
        PlayerEvents.Singleton.RegisterPlayerChasedStartActions(StartPlayingChaseBeat);
        PlayerEvents.Singleton.RegisterPlayerChasedEndActions(StopPlayingChaseBeat);
        PlayerEvents.Singleton.RegisterLifeRemovedActions(StopImmediately);
    }

    private void StopImmediately()
    {
        CancelInvoke("DecreaseVolume");
        CancelInvoke("IncreaseVolume");
        chaseBeat.volume = 0f;
    }

    private void StartPlayingChaseBeat()
    {
        InvokeRepeating("IncreaseVolume", 0.05f, 0.25f);
    }

    private void StopPlayingChaseBeat()
    {
        InvokeRepeating("DecreaseVolume", 0.05f, 0.25f);
    }

    private void IncreaseVolume()
    {
        CancelInvoke("DecreaseVolume");

        if (chaseBeat.volume >= 1f)
        {
            CancelInvoke("IncreaseVolume");
            return;
        }

        chaseBeat.volume += 0.05f;
    }

    private void DecreaseVolume()
    {
        CancelInvoke("IncreaseVolume");

        if (chaseBeat.volume <= 0f)
        {
            CancelInvoke("DecreaseVolume");
            return;
        }

        chaseBeat.volume -= 0.05f;
    }
}
