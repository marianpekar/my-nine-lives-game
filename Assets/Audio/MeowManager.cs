using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowManager : MonoBehaviour
{
    public AudioSource meow;

    // Start is called before the first frame update
    void Start()
    {
        PlayerEvents.Singleton.RegisterLifeAddedActions(PlayMeow);
    }

    void PlayMeow()
    {
        meow.Play();
    }
}
