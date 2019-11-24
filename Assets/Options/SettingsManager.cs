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

    [SerializeField]
    Slider masterVolumeSlider;

    [SerializeField]
    Slider musicVolumeSlider;

    [SerializeField]
    Slider sfxVolumeSlider;

    float currentMasterVolume;
    float currentMusicVolume;
    float currentSfxVolume;

    int currentResolutionIndex;
    bool resolutionChanged = false;
    Resolution[] resolutions;
    List<string> resolutionLabels = new List<string>();

    bool isFullscreen;

    int qualityIndex;
    public enum VideoQuality
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
        SetInitials();

        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionLabels.Add(string.Format("{0}x{1}", resolutions[i].width, resolutions[i].height));

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
    }

    private void SetInitials()
    {
        qualityIndex = (int)PlayerPrefsManager.QualityIndex;
        isFullscreen = PlayerPrefsManager.IsFullscreen;

        Screen.SetResolution(PlayerPrefsManager.ScreenWidth,
                             PlayerPrefsManager.ScreenHeight,
                             PlayerPrefsManager.IsFullscreen);

        SetResolutionText(PlayerPrefsManager.ScreenWidth,
                          PlayerPrefsManager.ScreenHeight);

        SetMasterVolume(PlayerPrefsManager.MasterVolume);
        SetMusicVolume(PlayerPrefsManager.MusicVolume);
        SetSfxVolume(PlayerPrefsManager.SfxVolume);

    }

    public void SetVolumeSliders()
    {
        masterVolumeSlider.value = currentMasterVolume;
        musicVolumeSlider.value = currentMusicVolume;
        sfxVolumeSlider.value = currentSfxVolume;
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
        PlayerPrefsManager.ScreenWidth = resolutions[currentResolutionIndex].width;
        PlayerPrefsManager.ScreenHeight = resolutions[currentResolutionIndex].height;
        resolutionChanged = false;
    }

    private void SetResolutionText()
    {
        resolutionText.text = resolutionLabels[currentResolutionIndex];
    }

    private void SetResolutionText(int screenWidth, int screenHeight)
    {
        resolutionText.text = string.Format("{0}x{1}", screenWidth, screenHeight);
    }

    private void ToogleFullscreen()
    {
        isFullscreen = !isFullscreen;

        FullScreenChanged();
    }

    public void FullScreenChanged()
    {
        SetFullScreenLabel();
        Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, isFullscreen);
        PlayerPrefsManager.IsFullscreen = isFullscreen;
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
        if(qualityIndex > (int)VideoQuality.Ultra)
            qualityIndex = (int)VideoQuality.Ultra;

        QualityChanged();
    }

    public void QualityDown()
    {
        qualityIndex--;
        if (qualityIndex < (int)VideoQuality.VeryLow)
            qualityIndex = (int)VideoQuality.VeryLow;

        QualityChanged();
    }

    private void QualityChanged()
    {
        SetQualityLabel();
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefsManager.QualityIndex = (VideoQuality)qualityIndex;
    }

    public void SetQualityLabel()
    {
        if (qualityIndex == (int)VideoQuality.VeryLow) quality.text = "VERY LOW";
        else if (qualityIndex == (int)VideoQuality.Low) quality.text = "LOW";
        else if (qualityIndex == (int)VideoQuality.Medium) quality.text = "MEDIUM";
        else if (qualityIndex == (int)VideoQuality.High) quality.text = "HIGH";
        else if (qualityIndex == (int)VideoQuality.VeryHigh) quality.text = "VERY HIGH";
        else if (qualityIndex == (int)VideoQuality.Ultra) quality.text = "ULTRA";
    }

    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        audioMixer.SetFloat("masterVolume", currentMasterVolume);
        PlayerPrefsManager.MasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        audioMixer.SetFloat("musicVolume", currentMusicVolume);
        PlayerPrefsManager.MusicVolume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        currentSfxVolume = volume;
        audioMixer.SetFloat("sfxVolume", currentSfxVolume);
        PlayerPrefsManager.SfxVolume = volume;
    }
}
