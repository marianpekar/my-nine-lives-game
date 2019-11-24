using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultConfigStates
{
    // Video
    public static int ScreenWidth { get; } = 800;
    public static int ScreenHeight { get; } = 600;
    public static bool IsFullscreen { get; } = true;
    public static int QualityIndex { get; } = 5;

    // Audio
    public static float MasterVolume { get; } = 0f; // 0 dB = max, -80 dB = min
    public static float MusicVolume { get; } = -20f;
    public static float SfxVolume { get; } = -20f;
}
