using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTimePreserver : MonoBehaviour
{
    public int Hours { get; set;}
    public int Minutes { get; set;}

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
