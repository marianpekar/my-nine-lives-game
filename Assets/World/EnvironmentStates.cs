using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentStates
{
    public static bool FirstSet { get; set; } = false;
    public static int Hours { get; set;}
    public static int Minutes { get; set;}
    public static EnvironmentManager.EnvironmentType EnvironmentType { get; set; }
    public static EnvironmentManager.EnvironmentEpoch EnvironmentEpoch { get; set; }

    private static EnvironmentManager.DayTime dayTime;
    public static EnvironmentManager.DayTime DayTime
    {
        get => dayTime;
        set
        {
            dayTime = value;
            EnvironmentEvents.Singleton.InvokeTimeChangedActions();
        }
    }
}
