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
        PlayerStates.Singleton.FeedLevel -= PlayerStates.Singleton.EnergyConsumed;
    }
}
