using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentPreserver : MonoBehaviour
{
    public int Hours { get; set;}
    public int Minutes { get; set;}

    public EnvironmentManager.EnvironmentType EnvironmentType { get; set; }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
