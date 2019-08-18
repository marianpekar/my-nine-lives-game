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
    }

    private void Update()
    {
        SetSunRotation(hours, minutes);
        SetColor(hours);
    }

    // Update is called once per frame
    public void SetSunRotation(int hours, int minutes)
    {
        float xRotation = 2 * (0.125f * (hours * 60 + minutes));
        sun.transform.rotation = Quaternion.Euler(new Vector3(xRotation - 90, 0,0));
    }

    public void SetColor(int hours)
    {
        if (hours < MORNING || hours >= NIGHT)
        {
            sun.color = nightColor;
            RenderSettings.fogColor = nightFog;
            return;
        }

        RenderSettings.fogColor = dayFog;

        if (hours >= MORNING && hours < NOON)
            sun.color = morningColor;
        else if (hours >= NOON && hours < AFTERNOON)
            sun.color = noonColor;
        else if (hours >= AFTERNOON && hours < NIGHT)
            sun.color = afternoonColor;
    }
}
