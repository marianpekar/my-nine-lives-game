using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test_StealthSlider : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }
    void Update()
    {
        slider.value = PlayerStates.Singleton.CurrentStealthLevel;
    }
}
