using UnityEngine;
using System.Collections.Generic;
using System;

public static class GameInputManager
{
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[7]
    {
        "Jump",
        "Walk",
        "Cancel",
        "Forward",
        "Backward",
        "Left",
        "Right"
    };
    static KeyCode[] defaults = new KeyCode[7]
    {
        KeyCode.Space,
        KeyCode.LeftControl,
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
        SetDefaults();
    }

    public static void SetDefaults()
    {
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Remove(keyMaps[i]);
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

    public static KeyCode GetKeyCode(string keyName)
    {
        KeyCode keyCode;
        keyMapping.TryGetValue(keyName, out keyCode);
        return keyCode; 
    }
}