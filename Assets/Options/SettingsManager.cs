using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;

    [SerializeField]
    Text quality;

    [SerializeField]
    Text fullScreen;

    [SerializeField]
    Text resolutionText;

    int currentResolutionIndex;
    bool resolutionChanged = false;
    Resolution[] resolutions;
    List<string> resolutionLabels = new List<string>();

    bool isFullscreen;

    int qualityIndex;
    enum QualityLevel
    {
        VeryLow,
        Low,
        Medium,
        High,
        VeryHigh,
        Ultra
    }

    void Awake()
    {
        qualityIndex = QualitySettings.GetQualityLevel();
        isFullscreen = Screen.fullScreen;
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionLabels.Add(string.Format("{0}x{1}", resolutions[i].width, resolutions[i].height));

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
    }

    public void SetResolutionUp()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex > resolutions.Length - 1)
            currentResolutionIndex = resolutions.Length - 1;

        resolutionChanged = true;
        SetResolutionText();
    }

    public void SetResolutionDown()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
            currentResolutionIndex = 0;

        resolutionChanged = true;
        SetResolutionText();
    }

    public void ChangeResolution()
    {
        if (!resolutionChanged)
            return;

        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, isFullscreen);
        resolutionChanged = false;

        // Debug.Log(string.Format("Resolution set to: {0}x{1} (Fullscreen:{2})", 
        //    resolutions[currentResolutionIndex].width, 
        //    resolutions[currentResolutionIndex].height,
        //    isFullscreen));
    }

    private void SetResolutionText()
    {
        resolutionText.text = resolutionLabels[currentResolutionIndex];
    }

    private void ToogleFullscreen()
    {
        isFullscreen = !isFullscreen;

        FullScreenChanged();
    }

    public void FullScreenChanged()
    {
        SetFullScreenLabel();
        Screen.fullScreen = isFullscreen;
    }

    public void SetFullScreenLabel()
    {
        if (isFullscreen)
            fullScreen.text = "YES";
        else
            fullScreen.text = "NO";
    }

    public void QualityUp()
    {
        qualityIndex++;
        if(qualityIndex > (int)QualityLevel.Ultra)
            qualityIndex = (int)QualityLevel.Ultra;

        QualityChanged();
    }

    public void QualityDown()
    {
        qualityIndex--;
        if (qualityIndex < (int)QualityLevel.VeryLow)
            qualityIndex = (int)QualityLevel.VeryLow;

        QualityChanged();
    }

    private void QualityChanged()
    {
        SetQualityLabel();
        QualitySettings.SetQualityLevel(qualityIndex); 
    }

    public void SetQualityLabel()
    {
        if (qualityIndex == (int)QualityLevel.VeryLow) quality.text = "VERY LOW";
        else if (qualityIndex == (int)QualityLevel.Low) quality.text = "LOW";
        else if (qualityIndex == (int)QualityLevel.Medium) quality.text = "MEDIUM";
        else if (qualityIndex == (int)QualityLevel.High) quality.text = "HIGH";
        else if (qualityIndex == (int)QualityLevel.VeryHigh) quality.text = "VERY HIGH";
        else if (qualityIndex == (int)QualityLevel.Ultra) quality.text = "ULTRA";
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }
}
