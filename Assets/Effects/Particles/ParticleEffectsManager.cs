using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    public ParticleSystem ghostPartlicles;

    private void Start()
    {
        PlayerEvents.Singleton.RegisterPreyCatchAction(BurstGhostParticles);
    }

    private void BurstGhostParticles()
    {
        ghostPartlicles.Play();
    }
}
