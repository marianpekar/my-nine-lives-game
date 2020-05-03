using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHighScore : MonoBehaviour
{
    [SerializeField]
    private Text highScore;

    void Start()
    {
        highScore.text = PlayerStates.Singleton.HighScore.ToString();
    }
}
