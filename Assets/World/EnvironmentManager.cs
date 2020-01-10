using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private enum DayTime { EarlyMorning, Morning, BeforeNoon, Noon, AfterNoon, Night }
    private DayTime dayTime;

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

        if (!EnvironmentPreserver.FirstSet)
        {
            SetRandomEnvironmentType();
            SetRandomEnvironmentEpoch();
            SetRandomTime();
            EnvironmentPreserver.FirstSet = true;
        }
        else
        {
            SetTime(EnvironmentPreserver.Hours, EnvironmentPreserver.Minutes);
            CurrentEnvironmentType = EnvironmentPreserver.EnvironmentType;
            CurrentEnvironmentEpoch = EnvironmentPreserver.EnvironmentEpoch;
        }

        SetEnvironment();
    }

    public void SetTime(int hour, int minute)
    {
        hours = hour;
        minutes = minute;
        SetDayTime();
    }
    public void SetRandomTime()
    {      
        hours = Random.Range(0, CurrentEnvironmentEpoch == EnvironmentEpoch.Winter ? EARLY_MORNING - 1 : NIGHT);
        minutes = Random.Range(0, 60);
        SetDayTime();
    }

    private void SetDayTime()
    {
        if (hours < EARLY_MORNING)
        {
            dayTime = DayTime.Night;
            return;
        }

        if      (hours >= EARLY_MORNING && hours < MORNING)     dayTime = DayTime.EarlyMorning;
        else if (hours >= MORNING && hours < BEFORE_NOON)       dayTime = DayTime.Morning;
        else if (hours >= BEFORE_NOON && hours < NOON)          dayTime = DayTime.BeforeNoon;
        else if (hours >= NOON && hours < AFTERNOON)            dayTime = DayTime.Noon;
        else                                                    dayTime = DayTime.AfterNoon;
    }

    public void SetEnvironment()
    {
        SetPostProcessing();
        SetSunRotation();
        SetSunColor();
        SetFogColor();
        SetSkybox();
        SetTerrainColor();
        UpdateEnvironmentPreserver();
    }

    public void UpdateEnvironmentPreserver()
    {
        EnvironmentPreserver.Hours = hours;
        EnvironmentPreserver.Minutes = minutes;
        EnvironmentPreserver.EnvironmentType = CurrentEnvironmentType;
        EnvironmentPreserver.EnvironmentEpoch = CurrentEnvironmentEpoch;
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
    }

    void SetPostProcessing()
    {
        if (CurrentEnvironmentEpoch == EnvironmentEpoch.Spring)
        {
            if      (dayTime == DayTime.Night)              postProcessManager.SetTemperature(springNightTemperature);
            else if (dayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(springEarlyMorningTemperature);
            else if (dayTime == DayTime.Morning)            postProcessManager.SetTemperature(springMorningTemperature);
            else if (dayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(springBeforeNoonTemperature);
            else if (dayTime == DayTime.Noon)               postProcessManager.SetTemperature(springNoonTemperature);
            else                                            postProcessManager.SetTemperature(springAfternoonTemperature);
        }       
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Fall)
        {
            if      (dayTime == DayTime.Night)              postProcessManager.SetTemperature(fallNightTemperature);
            else if (dayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(fallEarlyMorningTemperature);
            else if (dayTime == DayTime.Morning)            postProcessManager.SetTemperature(fallMorningTemperature);
            else if (dayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(fallBeforeNoonTemperature);
            else if (dayTime == DayTime.Noon)               postProcessManager.SetTemperature(fallNoonTemperature);
            else                                            postProcessManager.SetTemperature(fallAfternoonTemperature);
        }         
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Winter)
        {
            if      (dayTime == DayTime.Night)              postProcessManager.SetTemperature(winterNightTemperature);
            else if (dayTime == DayTime.EarlyMorning)       postProcessManager.SetTemperature(winterEarlyMorningTemperature);
            else if (dayTime == DayTime.Morning)            postProcessManager.SetTemperature(winterMorningTemperature);
            else if (dayTime == DayTime.BeforeNoon)         postProcessManager.SetTemperature(winterBeforeNoonTemperature);
            else if (dayTime == DayTime.Noon)               postProcessManager.SetTemperature(winterNoonTemperature);
            else                                            postProcessManager.SetTemperature(winterAfternoonTemperature);
        }
       
    }

    void SetTerrainColor()
    {
        if      (dayTime == DayTime.Night)          terrain.color = nightTerrain;
        else if (dayTime == DayTime.EarlyMorning)   terrain.color = earlyMorningTerrain;
        else if (dayTime == DayTime.Morning)        terrain.color = morningTerrain;
        else if (dayTime == DayTime.BeforeNoon)     terrain.color = beforeNoonTerrain;
        else if (dayTime == DayTime.Noon)           terrain.color = noonTerrain;
        else                                        terrain.color = afternoonTerrain;
    }

    void SetSkybox()
    {
        if      (dayTime == DayTime.Night)          RenderSettings.skybox = nightSkybox;
        else if (dayTime == DayTime.EarlyMorning)   RenderSettings.skybox = earlyMorningSkybox;
        else if (dayTime == DayTime.Morning)        RenderSettings.skybox = morningSkybox;
        else if (dayTime == DayTime.BeforeNoon)     RenderSettings.skybox = beforenoonSkybox;
        else if (dayTime == DayTime.Noon)           RenderSettings.skybox = noonSkybox;
        else                                        RenderSettings.skybox = afternoonSkybox;
    }

    void SetFogColor()
    {
        if      (dayTime == DayTime.Night)          RenderSettings.fogColor = nightFog;
        else if (dayTime == DayTime.EarlyMorning)   RenderSettings.fogColor = earlyMorningFog;
        else if (dayTime == DayTime.Morning)        RenderSettings.fogColor = morningFog;
        else if (dayTime == DayTime.BeforeNoon)     RenderSettings.fogColor = beforeNoonFog;
        else if (dayTime == DayTime.Noon)           RenderSettings.fogColor = noonFog;
        else                                        RenderSettings.fogColor = afternoonFog;
    }

    void SetSunColor()
    {
        if (dayTime == DayTime.Night)               sun.color = nightColor;
        else if (dayTime == DayTime.EarlyMorning)   sun.color = earlyMorningColor;
        else if (dayTime == DayTime.Morning)        sun.color = morningColor;
        else if (dayTime == DayTime.BeforeNoon)     sun.color = beforeNoonColor;
        else if (dayTime == DayTime.Noon)           sun.color = noonColor;
        else                                        sun.color = afternoonColor;
    }
}
