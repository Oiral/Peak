using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWithRigidbody : MonoBehaviour
{
    public float speedStrength = 2;
    public float emissionStrength = 1;
    

    Rigidbody rb;
    public ParticleSystem particle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ParticleSystem.MainModule particleMain = particle.main;
        ParticleSystem.EmissionModule particleEmission = particle.emission;

        particleMain.startSpeed = rb.velocity.magnitude * speedStrength;
        particleEmission.rateOverTime = rb.velocity.magnitude * emissionStrength;
    }
}
