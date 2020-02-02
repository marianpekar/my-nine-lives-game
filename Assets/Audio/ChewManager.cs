using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChewManager : MonoBehaviour
{
    public AudioSource chew;

    // Start is called before the first frame update
    void Start()
    {
        PlayerEvents.Singleton.RegisterPreyCatchAction(PlayChew);
    }

    void PlayChew()
    {
        chew.Play();
    }
}
