﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class PlayerPrefsManager
{
    public static int ScreenWidth { 
        get 
        {
            if (PlayerPrefs.HasKey("ScreenWidth")) 
                return PlayerPrefs.GetInt("ScreenWidth");

            return DefaultConfigStates.ScreenWidth;
        } 
        set
        {
            PlayerPrefs.SetInt("ScreenWidth", value);
        }
    }
    public static int ScreenHeight
    {
        get
        {
            if (PlayerPrefs.HasKey("ScreenHeight"))
                return PlayerPrefs.GetInt("ScreenHeight");

            return DefaultConfigStates.ScreenHeight;
        }
        set
        {
            PlayerPrefs.SetInt("ScreenHeight", value);
        }
    }

    public static bool IsFullscreen
    {
        get
        {
            if (PlayerPrefs.HasKey("IsFullscreen"))
            {
                if (PlayerPrefs.GetInt("IsFullscreen") == 1) return true;
                else if (PlayerPrefs.GetInt("IsFullscreen") == 0) return false;
            }
            
            return DefaultConfigStates.IsFullscreen;
        }
        set
        {
            PlayerPrefs.SetInt("IsFullscreen", value ? 1 : 0);
        }
    }
    public static SettingsManager.VideoQuality QualityIndex
    {
        get
        {
            if (PlayerPrefs.HasKey("QualityIndex"))
                return (SettingsManager.VideoQuality)PlayerPrefs.GetInt("QualityIndex");

            return (SettingsManager.VideoQuality)DefaultConfigStates.QualityIndex;
        }
        set
        {
                PlayerPrefs.SetInt("QualityIndex", (int)value);
        }
    }
    public static float MasterVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("MasterVolume"))
                return PlayerPrefs.GetFloat("MasterVolume");

            return DefaultConfigStates.MasterVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
    }
    public static float MusicVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
                return PlayerPrefs.GetFloat("MusicVolume");

            return DefaultConfigStates.MusicVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }
    public static float SfxVolume
    {
        get
        {
            if (PlayerPrefs.HasKey("SfxVolume"))
                return PlayerPrefs.GetFloat("SfxVolume");

            return DefaultConfigStates.SfxVolume;
        }
        set
        {
            PlayerPrefs.SetFloat("SfxVolume", value);
        }
    }
}