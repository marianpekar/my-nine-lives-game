using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DefaultConfigStates
{
    // Video
    public int ScreenWidth { get; } = 800;
    public int ScreenHeight { get; } = 600;
    public bool IsFullscreen { get; } = true;
    public int QualityIndex { get; } = 5;

    // Audio
    public float MasterVolume { get; } = 0f; // 0 dB = max, -80 dB = min
    public float MusicVolume { get; } = -20f;
    public float SfxVolume { get; } = -20f;

    private static DefaultConfigStates instance;
    public static DefaultConfigStates Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = new DefaultConfigStates();
            }

            return instance;
        }
    }
}
