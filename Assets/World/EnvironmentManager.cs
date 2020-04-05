using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;
using Random = UnityEngine.Random;

public class EnvironmentManager : MonoBehaviour
{
    public enum EnvironmentType
    {
        Deciduous, 
        Coniferous,
        All
    }
    public enum EnvironmentEpoch
    {
        Spring,
        Fall,
        Winter,
        All
    }

    const int ENVIRONMENT_TYPES_COUNT = 3;
    const int ENVIRONMENT_EPOCHS_COUNT = 4;

    public static EnvironmentType CurrentEnvironmentType { get; set; }
    public static EnvironmentEpoch CurrentEnvironmentEpoch { get; set; }
    private EnvironmentEpoch lastEnvironmentEpoch;

    [Range(0, 24)]
    public int hours = 12;
    [Range(0, 60)]
    public int minutes = 0;

    public Color earlyMorningColor;
    public Color morningColor;
    public Color beforeNoonColor;
    public Color noonColor;
    public Color afternoonColor;
    public Color nightColor;

    public Color earlyMorningFog;
    public Color morningFog;
    public Color beforeNoonFog;
    public Color noonFog;
    public Color afternoonFog;
    public Color nightFog;

    const int EARLY_MORNING = 4;
    const int MORNING = 8;
    const int BEFORE_NOON = 12;
    const int NOON = 16;
    const int AFTERNOON = 20;
    const int NIGHT = 24;

    public enum DayTime { EarlyMorning, Morning, BeforeNoon, Noon, AfterNoon, Night }
    private DayTime lastDayTime = DayTime.EarlyMorning;
    public DayTime CurrentDayTime { get; private set; } = DayTime.EarlyMorning;

    public Material earlyMorningSkybox;
    public Material morningSkybox;
    public Material beforenoonSkybox;
    public Material noonSkybox;
    public Material afternoonSkybox;
    public Material nightSkybox;

    public Material terrain;
    public Color earlyMorningTerrain;
    public Color morningTerrain;
    public Color beforeNoonTerrain;
    public Color noonTerrain;
    public Color afternoonTerrain;
    public Color nightTerrain;

    public Light sun;
    public Light moon;

    public PostProcessManager postProcessManager;
    public float springEarlyMorningTemperature;
    public float fallEarlyMorningTemperature;
    public float winterEarlyMorningTemperature;

    public float springMorningTemperature;
    public float fallMorningTemperature;
    public float winterMorningTemperature;

    public float springBeforeNoonTemperature;
    public float fallBeforeNoonTemperature;
    public float winterBeforeNoonTemperature;

    public float springNoonTemperature;
    public float fallNoonTemperature;
    public float winterNoonTemperature;

    public float springAfternoonTemperature;
    public float fallAfternoonTemperature;
    public float winterAfternoonTemperature;

    public float springNightTemperature;
    public float fallNightTemperature;
    public float winterNightTemperature;

    void Start()
    {
        if (!EnvironmentStates.FirstSet)
        {
            SetRandomEnvironmentType();
            SetRandomEnvironmentEpoch();
            SetRandomTime();
            EnvironmentStates.FirstSet = true;
        }
        else
        {
            SetTime(EnvironmentStates.Hours, EnvironmentStates.Minutes);
            CurrentEnvironmentType = EnvironmentStates.EnvironmentType;
            CurrentEnvironmentEpoch = EnvironmentStates.EnvironmentEpoch;
            CurrentDayTime = EnvironmentStates.DayTime;
        }
        SetEnvironment();
    }

    public void SetTime(int hours, int minutes)
    {
        CurrentDayTime = SetDayTime(hours, minutes);
    }
    public void SetRandomTime()
    {   
        CurrentDayTime = SetDayTime(Random.Range(0, CurrentEnvironmentEpoch == EnvironmentEpoch.Winter ? EARLY_MORNING - 1 : NIGHT), Random.Range(0, 60));

        if (CurrentDayTime == lastDayTime)
            CurrentDayTime = SetDayTime(this.hours + hours < 20 ? 4 : -4, Random.Range(0, 60));
        else
            lastDayTime = CurrentDayTime;
    }

    private DayTime SetDayTime(int hours, int minutes)
    {
        this.hours = hours;
        this.minutes = minutes;

        if (hours < EARLY_MORNING)
            return DayTime.Night;

        if      (hours >= EARLY_MORNING && hours < MORNING)     return DayTime.EarlyMorning;
        else if (hours >= MORNING && hours < BEFORE_NOON)       return DayTime.Morning;
        else if (hours >= BEFORE_NOON && hours < NOON)          return DayTime.BeforeNoon;
        else if (hours >= NOON && hours < AFTERNOON)            return DayTime.Noon;
        else                                                    return DayTime.AfterNoon;
    }

    public void SetEnvironment()
    {
        SetPostProcessing();
        SetSunRotation();
        SetSunColor();
        SetFogColor();
        SetSkybox();
        SetTerrainColor();
        UpdateEnvironmentStates();
    }

    public void UpdateEnvironmentStates()
    {
        EnvironmentStates.Hours = hours;
        EnvironmentStates.Minutes = minutes;
        EnvironmentStates.EnvironmentType = CurrentEnvironmentType;
        EnvironmentStates.EnvironmentEpoch = CurrentEnvironmentEpoch;
        EnvironmentStates.DayTime = CurrentDayTime;
    }
    void SetSunRotation()
    {
        float xRotation = 2 * (0.125f * (hours * 60 + minutes));
        sun.transform.rotation = Quaternion.Euler(new Vector3(xRotation - 120, -30, 0));
        moon.transform.rotation = Quaternion.Euler(new Vector3(xRotation + 180 - 120, -30, 0));
    }

    public void SetRandomEnvironmentType()
    {
        CurrentEnvironmentType = (EnvironmentType)Random.Range(0, ENVIRONMENT_TYPES_COUNT - 1);
    }

    public void SetRandomEnvironmentEpoch()
    {
        CurrentEnvironmentEpoch = (EnvironmentEpoch)Random.Range(0, ENVIRONMENT_EPOCHS_COUNT - 1);

        if (CurrentEnvironmentEpoch == lastEnvironmentEpoch)
            CurrentEnvironmentEpoch = CurrentEnvironmentEpoch == (EnvironmentEpoch)(ENVIRONMENT_EPOCHS_COUNT - 1) ? CurrentEnvironmentEpoch + 1 : 0;
        else
            lastEnvironmentEpoch = CurrentEnvironmentEpoch;
    }

    void SetPostProcessing()
    {
        if (CurrentEnvironmentEpoch == EnvironmentEpoch.Spring)
        {
            if      (CurrentDayTime == DayTime.Night)              postProcessManager.SetTemperature(springNightTemperature);
            else if (CurrentDayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(springEarlyMorningTemperature);
            else if (CurrentDayTime == DayTime.Morning)            postProcessManager.SetTemperature(springMorningTemperature);
            else if (CurrentDayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(springBeforeNoonTemperature);
            else if (CurrentDayTime == DayTime.Noon)               postProcessManager.SetTemperature(springNoonTemperature);
            else                                            postProcessManager.SetTemperature(springAfternoonTemperature);
        }       
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Fall)
        {
            if      (CurrentDayTime == DayTime.Night)              postProcessManager.SetTemperature(fallNightTemperature);
            else if (CurrentDayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(fallEarlyMorningTemperature);
            else if (CurrentDayTime == DayTime.Morning)            postProcessManager.SetTemperature(fallMorningTemperature);
            else if (CurrentDayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(fallBeforeNoonTemperature);
            else if (CurrentDayTime == DayTime.Noon)               postProcessManager.SetTemperature(fallNoonTemperature);
            else                                            postProcessManager.SetTemperature(fallAfternoonTemperature);
        }         
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Winter)
        {
            if      (CurrentDayTime == DayTime.Night)              postProcessManager.SetTemperature(winterNightTemperature);
            else if (CurrentDayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(winterEarlyMorningTemperature);
            else if (CurrentDayTime == DayTime.Morning)            postProcessManager.SetTemperature(winterMorningTemperature);
            else if (CurrentDayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(winterBeforeNoonTemperature);
            else if (CurrentDayTime == DayTime.Noon)               postProcessManager.SetTemperature(winterNoonTemperature);
            else                                            postProcessManager.SetTemperature(winterAfternoonTemperature);
        }
       
    }

    void SetTerrainColor()
    {
        if      (CurrentDayTime == DayTime.Night)          terrain.color = nightTerrain;
        else if (CurrentDayTime == DayTime.EarlyMorning)   terrain.color = earlyMorningTerrain;
        else if (CurrentDayTime == DayTime.Morning)        terrain.color = morningTerrain;
        else if (CurrentDayTime == DayTime.BeforeNoon)     terrain.color = beforeNoonTerrain;
        else if (CurrentDayTime == DayTime.Noon)           terrain.color = noonTerrain;
        else                                        terrain.color = afternoonTerrain;
    }

    void SetSkybox()
    {
        if      (CurrentDayTime == DayTime.Night)          RenderSettings.skybox = nightSkybox;
        else if (CurrentDayTime == DayTime.EarlyMorning)   RenderSettings.skybox = earlyMorningSkybox;
        else if (CurrentDayTime == DayTime.Morning)        RenderSettings.skybox = morningSkybox;
        else if (CurrentDayTime == DayTime.BeforeNoon)     RenderSettings.skybox = beforenoonSkybox;
        else if (CurrentDayTime == DayTime.Noon)           RenderSettings.skybox = noonSkybox;
        else                                        RenderSettings.skybox = afternoonSkybox;
    }

    void SetFogColor()
    {
        if      (CurrentDayTime == DayTime.Night)          RenderSettings.fogColor = nightFog;
        else if (CurrentDayTime == DayTime.EarlyMorning)   RenderSettings.fogColor = earlyMorningFog;
        else if (CurrentDayTime == DayTime.Morning)        RenderSettings.fogColor = morningFog;
        else if (CurrentDayTime == DayTime.BeforeNoon)     RenderSettings.fogColor = beforeNoonFog;
        else if (CurrentDayTime == DayTime.Noon)           RenderSettings.fogColor = noonFog;
        else                                        RenderSettings.fogColor = afternoonFog;
    }

    void SetSunColor()
    {
        if (CurrentDayTime == DayTime.Night)               sun.color = nightColor;
        else if (CurrentDayTime == DayTime.EarlyMorning)   sun.color = earlyMorningColor;
        else if (CurrentDayTime == DayTime.Morning)        sun.color = morningColor;
        else if (CurrentDayTime == DayTime.BeforeNoon)     sun.color = beforeNoonColor;
        else if (CurrentDayTime == DayTime.Noon)           sun.color = noonColor;
        else                                        sun.color = afternoonColor;
    }
}
