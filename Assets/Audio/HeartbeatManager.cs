using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatManager : MonoBehaviour
{
    public AudioSource heartBeat;

    void Start()
    {
        PlayerEvents.Singleton.RegisterStealthStartActions(StartPlayingHeartbeat);
        PlayerEvents.Singleton.RegisterStealthEndActions(StopPlayingHeartbeat);
    }

    private void StartPlayingHeartbeat()
    {
        InvokeRepeating("IncreaseVolume", 0.8f, 0.25f);
    }

    private void StopPlayingHeartbeat()
    {
        InvokeRepeating("DecreaseVolume", 0.4f, 0.12f);
    }

    private void IncreaseVolume()
    {
        if (heartBeat.volume >= 1f)
        {
            CancelInvoke("IncreaseVolume");
            return;
        }

        if(PlayerStates.Singleton.IsWalking)
            heartBeat.volume += 0.05f;
    }

    private void DecreaseVolume()
    {
        CancelInvoke("IncreaseVolume");

        if (heartBeat.volume <= 0f)
        {
            CancelInvoke("DecreaseVolume");
            return;
        }

        if (!PlayerStates.Singleton.IsWalking)
            heartBeat.volume -= 0.05f;
    }
}
