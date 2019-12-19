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

    public EnvironmentType CurrentEnvironmentType { get; set; }
    public EnvironmentEpoch CurrentEnvironmentEpoch { get; set; }

    [Range(0, 24)]
    public int hours = 12;
    [Range(0, 60)]
    public int minutes = 0;

    public Color morningColor;
    public Color noonColor;
    public Color afternoonColor;
    public Color nightColor;

    public Color morningFog;
    public Color noonFog;
    public Color afternoonFog;
    public Color nightFog;

    const int MORNING = 8;
    const int NOON = 12;
    const int AFTERNOON = 16;
    const int NIGHT = 19;

    public Material morningSkybox;
    public Material noonSkybox;
    public Material afternoonSkybox;
    public Material nightSkybox;

    public Material terrain;
    public Color morningTerrain;
    public Color noonTerrain;
    public Color afternoonTerrain;
    public Color nightTerrain;

    public Light sun;
    public Light moon;

    public PostProcessManager postProcessManager;
    public float springMorningTemperature;
    public float fallMorningTemperature;
    public float winterMorningTemperature;

    public float springNoonTemperature;
    public float fallNoonTemperature;
    public float winterNoonTemperature;

    public float springAfternoonTemperature;
    public float fallAfternoonTemperature;
    public float winterAfternoonTemperature;

    public float springNightTemperature;
    public float fallNightTemperature;
    public float winterNightTemperature;

    // Start is called before the first frame update
    void Start()
    {

        if (!EnvironmentPreserver.FirstSet)
        {
            SetRandomEnvironmentType();
            SetRandomEnvironmentEpoch();
            SetRandomDayTime();
            EnvironmentPreserver.FirstSet = true;
        }
        else
        {
            SetDayTime(EnvironmentPreserver.Hours, EnvironmentPreserver.Minutes);
            CurrentEnvironmentType = EnvironmentPreserver.EnvironmentType;
            CurrentEnvironmentEpoch = EnvironmentPreserver.EnvironmentEpoch;
        }
        SetEnvironment();

    }

    public void SetDayTime(int hour, int minute)
    {
        hours = hour;
        minutes = minute;
    }
    public void SetRandomDayTime()
    {
        hours = Random.Range(0, 24);
        minutes = Random.Range(0, 60);
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

    public void SetRandomEnvironmentType()
    {
        CurrentEnvironmentType = (EnvironmentType)Random.Range(0, ENVIRONMENT_TYPES_COUNT - 1);
        //Debug.Log(CurrentEnvironmentType);
    }

    public void SetRandomEnvironmentEpoch()
    {
        CurrentEnvironmentEpoch = (EnvironmentEpoch)Random.Range(0, ENVIRONMENT_EPOCHS_COUNT - 1);
        Debug.Log(CurrentEnvironmentEpoch);
    }

    void SetPostProcessing()
    {
        if (CurrentEnvironmentEpoch == EnvironmentEpoch.Spring)
        {
            if (hours < MORNING || hours >= NIGHT)
            {
                postProcessManager.SetTemperature(springNightTemperature);
                return;
            }

            if (hours >= MORNING && hours < NOON)
                postProcessManager.SetTemperature(springMorningTemperature);
            else if (hours >= NOON && hours < AFTERNOON)
                postProcessManager.SetTemperature(springNoonTemperature);
            else if (hours >= AFTERNOON && hours < NIGHT)
                postProcessManager.SetTemperature(springAfternoonTemperature);
        }       
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Fall)
        {
            if (hours < MORNING || hours >= NIGHT)
            {
                postProcessManager.SetTemperature(fallNightTemperature);
                return;
            }

            if (hours >= MORNING && hours < NOON)
                postProcessManager.SetTemperature(fallMorningTemperature);
            else if (hours >= NOON && hours < AFTERNOON)
                postProcessManager.SetTemperature(fallNoonTemperature);
            else if (hours >= AFTERNOON && hours < NIGHT)
                postProcessManager.SetTemperature(fallAfternoonTemperature);
        }         
        else if (CurrentEnvironmentEpoch == EnvironmentEpoch.Winter)
        {
            if (hours < MORNING || hours >= NIGHT)
            {
                postProcessManager.SetTemperature(winterNightTemperature);
                return;
            }

            if (hours >= MORNING && hours < NOON)
                postProcessManager.SetTemperature(winterMorningTemperature);
            else if (hours >= NOON && hours < AFTERNOON)
                postProcessManager.SetTemperature(winterNoonTemperature);
            else if (hours >= AFTERNOON && hours < NIGHT)
                postProcessManager.SetTemperature(winterAfternoonTemperature);
        }
       
    }

    void SetTerrainColor()
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            terrain.color = nightTerrain;
            return;
        }

        if (hours >= MORNING && hours < NOON)
            terrain.color = morningTerrain;
        else if (hours >= NOON && hours < AFTERNOON)
            terrain.color = noonTerrain;
        else if (hours >= AFTERNOON && hours < NIGHT)
            terrain.color = afternoonTerrain;
    }

    void SetSkybox()
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            RenderSettings.skybox = nightSkybox;
            return;
        }

        if (hours >= MORNING && hours < NOON)
            RenderSettings.skybox = morningSkybox;
        else if (hours >= NOON && hours < AFTERNOON)
            RenderSettings.skybox = noonSkybox;
        else if (hours >= AFTERNOON && hours < NIGHT)
            RenderSettings.skybox = afternoonSkybox;
    }

    void SetSunRotation()
    {
        float xRotation = 2 * (0.125f * (hours * 60 + minutes));
        sun.transform.rotation = Quaternion.Euler(new Vector3(xRotation - 120, -30,0));
        moon.transform.rotation = Quaternion.Euler(new Vector3(xRotation + 180 - 120, -30, 0));
    }

    void SetFogColor()
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            RenderSettings.fogColor = nightFog;
            return;
        }

        if (hours >= MORNING && hours < NOON)
            RenderSettings.fogColor = morningFog;
        else if (hours >= NOON && hours < AFTERNOON)
            RenderSettings.fogColor = noonFog;
        else if (hours >= AFTERNOON && hours < NIGHT)
            RenderSettings.fogColor = afternoonFog;
    }

    void SetSunColor()
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            sun.color = nightColor;
            return;
        }

        if (hours >= MORNING && hours < NOON)
            sun.color = morningColor;
        else if (hours >= NOON && hours < AFTERNOON)
            sun.color = noonColor;
        else if (hours >= AFTERNOON && hours < NIGHT)
            sun.color = afternoonColor;
    }
}
