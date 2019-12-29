using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StomachController : MonoBehaviour
{ 
    void Start()
    {
        InvokeRepeating("ConsumeEnergy", PlayerStates.Singleton.EnergyConsumedInterval, PlayerStates.Singleton.EnergyConsumedInterval);
    }

    void ConsumeEnergy()
    {
        if (PlayerStates.Singleton.IsDead)
            return;

        PlayerStates.Singleton.FeedLevel -= PlayerStates.Singleton.EnergyConsumed;
    }
}
