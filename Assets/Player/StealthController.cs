using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if((PlayerStates.Singleton.IsWalkingBackward || PlayerStates.Singleton.IsWalking))
        {
            PlayerStates.Singleton.IsStealth = true; 
        } else
        {
            PlayerStates.Singleton.IsStealth = false;
        }

        Debug.Log("Stealth: " + PlayerStates.Singleton.IsStealth);
    }
}
