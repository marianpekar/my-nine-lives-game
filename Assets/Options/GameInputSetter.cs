using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameInputSetter : MonoBehaviour
{
    public UnityEvent onKeyChanged = new UnityEvent();

    bool isListening = false;
    int[] values;
    bool[] keys;

    string keyToSet;

    void Awake()
    {
        values = (int[])System.Enum.GetValues(typeof(KeyCode));
        keys = new bool[values.Length];
    }

    void FixedUpdate()
    {
        if (!isListening) return;
            
        for (int i = 0; i < values.Length; i++)
        {
            keys[i] = Input.GetKey((KeyCode)values[i]);
            if (keys[i] == true)
            {
                GameInputManager.SetKeyMap(keyToSet, (KeyCode)values[i]);

                if(onKeyChanged != null)
                {
                    onKeyChanged.Invoke();
                }

                isListening = false;
                return;
            }
        }
    }

    public void SetKey(string keyToSet)
    {
        this.keyToSet = keyToSet;
        isListening = true;
    }

    public void SetDefaults()
    {
        GameInputManager.SetDefaults();
    }


}
