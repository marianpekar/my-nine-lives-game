using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectsManager : MonoBehaviour
{
    public ParticleSystem chickenLegs;

    private void Start()
    {
        PlayerEvents.Singleton.RegisterPreyCatchAction(BurstChickenLegsParticles);
    }

    private void BurstChickenLegsParticles()
    {
        chickenLegs.Play();
    }
}
