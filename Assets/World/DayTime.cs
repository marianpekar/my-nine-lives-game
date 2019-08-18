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

    public Color dayFog;
    public Color nightFog;

    const int MORNING = 6;
    const int NOON = 12;
    const int AFTERNOON = 16;
    const int NIGHT = 19;

    Light sun;
    // Start is called before the first frame update
    void Start()
    {
        sun = GetComponent<Light>();
        InvokeRepeating("DayCycle", 60f, 60f);
    }

    void DayCycle()
    {
        minutes++;
        if(minutes >= 60)
        {
            hours++;
            if (hours >= 24)
                hours = 0;

            minutes = 0;
        }
    }

    void Update()
    {
        SetSunRotation(hours, minutes);
        SetSunColor(hours);
        SetFogColor(hours);
    }

    // Update is called once per frame
    public void SetSunRotation(int hours, int minutes)
    {
        float xRotation = 2 * (0.125f * (hours * 60 + minutes));
        sun.transform.rotation = Quaternion.Euler(new Vector3(xRotation - 90, -30,0));
    }

    public void SetFogColor(int hours)
    {
        if (hours >= NIGHT)
            RenderSettings.fogColor = nightFog;
        else if (hours == NIGHT - 1)
            RenderSettings.fogColor = Color.Lerp(dayFog, nightFog, 0.03227f * minutes);
        else if (hours == MORNING)
            RenderSettings.fogColor = Color.Lerp(nightFog, dayFog, 0.01667f * minutes);
        else if (hours > MORNING)
            RenderSettings.fogColor = dayFog;
    }

    public void SetSunColor(int hours)
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            if (hours == NIGHT)
                sun.color = Color.Lerp(afternoonColor, nightColor, 0.01667f * minutes);
            else
                sun.color = nightColor;

            return;
        }

        if (hours >= MORNING && hours < NOON)
            if (hours == MORNING)
                sun.color = Color.Lerp(nightColor, morningColor, 0.01667f * minutes);
            else
                sun.color = morningColor;
        else if (hours >= NOON && hours < AFTERNOON)
            if (hours == NOON)
                sun.color = Color.Lerp(morningColor, noonColor, 0.01667f * minutes);
            else
                sun.color = noonColor;
        else if (hours >= AFTERNOON && hours < NIGHT)
            if (hours == AFTERNOON)
                sun.color = Color.Lerp(noonColor, afternoonColor, 0.01667f * minutes);
            else
                sun.color = afternoonColor;
    }
}
