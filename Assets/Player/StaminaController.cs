using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ConsumeStamina", PlayerStates.Singleton.StaminaConsumptionInterval, PlayerStates.Singleton.StaminaConsumptionInterval);
        InvokeRepeating("IncreaseStamina", PlayerStates.Singleton.StaminaIncreaseInterval, PlayerStates.Singleton.StaminaIncreaseInterval);
    }

    void ConsumeStamina()
    {
        if (PlayerStates.Singleton.IsSprinting && PlayerStates.Singleton.IsRunning)
        {
            PlayerStates.Singleton.Stamina -= PlayerStates.Singleton.StaminaStep;
        }
    }

    void IncreaseStamina()
    {
        if (!PlayerStates.Singleton.IsSprinting && !GameInputManager.GetKey("Sprint") && !GameInputManager.GetKey("Jump") && !PlayerStates.Singleton.IsDead)
        {
            PlayerStates.Singleton.Stamina += PlayerStates.Singleton.StaminaStep;
        }
    }
}
