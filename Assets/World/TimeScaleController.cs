using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResetTimeScale();

        PlayerEvents.Singleton.RegisterPlayerJumpsActions(SlowTime75Percent);
        PlayerEvents.Singleton.RegisterPlayerLandedActions(ResetTimeScale);
        PlayerEvents.Singleton.RegisterPlayerDiedActions(SlowTime85Percent);
    }

    // Update is called once per frame
    void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    void SlowTime75Percent()
    {
        Time.timeScale = 0.25f;
    }

    void SlowTime85Percent()
    {
        Time.timeScale = 0.15f;
    }
}
