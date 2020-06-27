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
    Slider masterVolumeSlider;

    [SerializeField]
    Slider musicVolumeSlider;

    [SerializeField]
    Slider sfxVolumeSlider;

    float currentMasterVolume;
    float currentMusicVolume;
    float currentSfxVolume;

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

    void Start()
    {
        SetInitials();
    }

    private void SetInitials()
    {
        qualityIndex = (int)PlayerPrefsManager.QualityIndex;

        Screen.SetResolution(PlayerPrefsManager.ScreenWidth,
                             PlayerPrefsManager.ScreenHeight,
                             PlayerPrefsManager.IsFullscreen);

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
        PlayerPrefsManager.Save();
    }

    public void SetQualityLabel()
    {
        switch (qualityIndex)
        {
            case (int)VideoQuality.VeryLow:
                quality.text = "VERY LOW";
                break;
            case (int)VideoQuality.Low:
                quality.text = "LOW";
                break;
            case (int)VideoQuality.Medium:
                quality.text = "MEDIUM";
                break;
            case (int)VideoQuality.High:
                quality.text = "HIGH";
                break;
            case (int)VideoQuality.VeryHigh:
                quality.text = "VERY HIGH";
                break;
            case (int)VideoQuality.Ultra:
                quality.text = "ULTRA";
                break;
        }
    }

    public void SetMasterVolume(float volume)
    {
        currentMasterVolume = volume;
        audioMixer.SetFloat("masterVolume", currentMasterVolume);
        PlayerPrefsManager.MasterVolume = volume;
        PlayerPrefsManager.Save();
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = volume;
        audioMixer.SetFloat("musicVolume", currentMusicVolume);
        PlayerPrefsManager.MusicVolume = volume;
        PlayerPrefsManager.Save();
    }

    public void SetSfxVolume(float volume)
    {
        currentSfxVolume = volume;
        audioMixer.SetFloat("sfxVolume", currentSfxVolume);
        PlayerPrefsManager.SfxVolume = volume;
        PlayerPrefsManager.Save();
    }
}
