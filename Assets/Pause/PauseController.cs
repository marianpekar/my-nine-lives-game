using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerEvents.Singleton.RegisterPausedActions(HandlePauseState);
    }

    // Update is called once per frame
    void HandlePauseState()
    {
        if (PlayerStates.Singleton.IsPaused)
            Time.timeScale = 0;
        else if (!PlayerStates.Singleton.IsPaused)
            Time.timeScale = 1f;
    }
}
