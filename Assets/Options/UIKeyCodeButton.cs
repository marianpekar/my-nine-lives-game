using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIKeyCodeButton : MonoBehaviour
{
    public string keyName;


    Text text;

    void Awake()
    {
        text = GetComponentInChildren<Text>();
        SetTextAsCurrentKey();
    }
    public void SetText(string changedText)
    {
        text.text = changedText;
    }

    public void SetTextAsCurrentKey()
    {
        SetText(GameInputManager.GetKeyCode(keyName).ToString().ToUpper());
    }
}
