using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashController : MonoBehaviour
{
    public float overlayDecreaseStep = 0.05f;
    public float overlayDecreaseInterval = 0.01f;
    
    float overlayIntensity;

    Image overlay;
    void Start()
    {
        overlay = GetComponent<Image>();
        PlayerEvents.Singleton.RegisterLifeRemovedActions(Flash);
    }
    void Flash()
    {
        overlayIntensity = 1f;
        InvokeRepeating("AddOverlay", 0.00001f, overlayDecreaseInterval);
    }

    void AddOverlay()
    {
        overlayIntensity -= overlayDecreaseStep;

        if(overlayIntensity <= 0f)
        {
            CancelInvoke("AddOverlay");
            return;
        }

        overlay.color = new Color(1, 1, 1, overlayIntensity);
    }
}
