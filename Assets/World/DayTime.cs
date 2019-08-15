using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTime : MonoBehaviour
{
    [Range(0,24)]
    public int hours = 12;
    [Range(0, 60)]
    public int minutes = 0;

    Light sun;
    // Start is called before the first frame update
    void Start()
    {
        sun = GetComponent<Light>();

        hours = Random.Range(0, 24);
        minutes = Random.Range(0, 60);
        SetSunRotation(hours, minutes);
    }

    // Update is called once per frame
    public void SetSunRotation(int hours, int minutes)
    {
        float xRotation = 0.125f * (hours * 60 + minutes);
        sun.transform.rotation = Quaternion.Euler(new Vector3(xRotation, 0,0));
    }
}
