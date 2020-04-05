using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreySoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public float minPlayDelay = 2f;
    public float maxPlayDelay = 10f;
    void Start()
    {
        Invoke("PlaySound", Random.Range(minPlayDelay, maxPlayDelay));
    }

    void PlaySound()
    {
        audioSource.Play();
        Invoke("PlaySound", Random.Range(minPlayDelay, maxPlayDelay));
    }
}
