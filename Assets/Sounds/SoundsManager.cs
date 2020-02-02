using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public AudioSource heartBeat;
    void Start()
    {
        PlayerEvents.Singleton.RegisterStealthStartActions(StartPlayingHeartbeat);
        PlayerEvents.Singleton.RegisterStealthEndActions(StopPlayingHeartbeat);
    }

    private void StartPlayingHeartbeat()
    {
        InvokeRepeating("IncreaseVolume",0.05f, 0.05f);
    }

    private void StopPlayingHeartbeat()
    {
        InvokeRepeating("DecreaseVolume", 0.05f, 0.01f);
    }

    private void IncreaseVolume()
    {
        if(heartBeat.volume >= 1f)
        {
            CancelInvoke("IncreaseVolume");
            return;
        }

        heartBeat.volume += 0.05f;
    }

    private void DecreaseVolume()
    {
        if (heartBeat.volume <= 0f)
        {
            CancelInvoke("DecreaseVolume");
            return;
        }

        heartBeat.volume -= 0.05f;
    }
}
