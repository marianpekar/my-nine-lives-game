using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    [Range(0,24)]
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

    public Light sun;
    public Light moon;
    // Start is called before the first frame update
    void Start()
    {
        hours = Random.Range(0, 24);
        minutes = Random.Range(0, 60);
    }

    void Update()
    {
        // TODO: Remove this before release
        if(Input.GetKeyUp(KeyCode.F1)) {
            hours = Random.Range(0, 24);
            minutes = Random.Range(0, 60);
        }

        SetSunRotation();
        SetSunColor();
        SetFogColor();
        SetSkybox();
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
