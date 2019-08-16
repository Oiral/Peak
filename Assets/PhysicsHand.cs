using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hitCloudPrefab;

    public float bigHitAmount = 5;
    public float minHitAmount = 2;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minHitAmount)
        {
            ParticleSystem particle = Instantiate(hitCloudPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal), null).GetComponent<ParticleSystem>();
            var module = particle.main;
            module.startSizeMultiplier = module.startSizeMultiplier * Mathf.Clamp(collision.relativeVelocity.magnitude / bigHitAmount, 0, 2);
            module.startSpeedMultiplier = module.startSpeedMultiplier * Mathf.Clamp(collision.relativeVelocity.magnitude / bigHitAmount, 0, 2);
        }
    }
}
