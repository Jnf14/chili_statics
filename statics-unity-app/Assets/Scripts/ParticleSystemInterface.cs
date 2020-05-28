using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemInterface : MonoBehaviour
{
    private ParticleSystem particleSystemComponent;

    private void Start()
    {
        particleSystemComponent = GetComponent<ParticleSystem>();
    }

    public void SetEmissionRateOverTime(float min, float max)
    {
        ParticleSystem.EmissionModule emissionModule = particleSystemComponent.emission;
        emissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(min, max);
    }
}
