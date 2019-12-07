using UnityEngine;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[8]
    {
        "Jump",
        "Walk",
        "Change Environment",
        "Cancel",
        "Forward",
        "Backward",
        "Left",
        "Right"
    };
    static KeyCode[] defaults = new KeyCode[8]
    {
        KeyCode.Space,
        KeyCode.LeftControl,
        KeyCode.F1,
        KeyCode.Escape,
        KeyCode.W,
        KeyCode.S,
        KeyCode.A,
        KeyCode.D
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKey(string keyMap)
    {
        return Input.GetKey(keyMapping[keyMap]);
    }
}