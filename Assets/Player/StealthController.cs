using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(HandleStealthLevelAddition());
    }

    // Update is called once per frame
    void Update()
    {
        if((PlayerStates.Singleton.IsWalkingBackward || PlayerStates.Singleton.IsWalking) && PlayerStates.Singleton.IsGrounded)
        {
            PlayerStates.Singleton.CurrentStealthLevel = PlayerStates.Singleton.StealthLevelLow + PlayerStates.Singleton.StealthLevelAddition;   
        } else
        {
            PlayerStates.Singleton.CurrentStealthLevel = PlayerStates.Singleton.StealthLevelHigh + PlayerStates.Singleton.StealthLevelAddition;
        }

        //Debug.Log("Current stealth level: " + PlayerStates.Singleton.CurrentStealthLevel);
    }

    IEnumerator HandleStealthLevelAddition()
    {
        while(true)
        {
            if (PlayerStates.Singleton.StealthLevelAddition < PlayerStates.Singleton.StealthLevelMaximumAddition && !PlayerStates.Singleton.IsCleaning)
                PlayerStates.Singleton.StealthLevelAddition += PlayerStates.Singleton.AddToStealthAddition;
            else if (PlayerStates.Singleton.IsCleaning && PlayerStates.Singleton.StealthLevelAddition > 0f)
                PlayerStates.Singleton.StealthLevelAddition -= PlayerStates.Singleton.SubstractFromStealthAddition;

            if (PlayerStates.Singleton.StealthLevelAddition < 0f)
                PlayerStates.Singleton.StealthLevelAddition = 0f;

            yield return new WaitForSeconds(PlayerStates.Singleton.AddToStealthAdditionRate);
        }
    }
}
