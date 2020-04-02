using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHand))]
[RequireComponent(typeof(PowerBase))]
public class PowerRocket : MonoBehaviour
{
    #region Power Stuff
    PhysicsHand hand;

    PowerBase basePower;

    public Vector3 direction;

    public float force = 1;

    public float drainPerSec = 10f;

    private void Awake()
    {
        hand = GetComponent<PhysicsHand>();
        basePower = GetComponent<PowerBase>();
    }

    private void OnEnable()
    {
        hand.gripHold.AddListener(jetPack);
        hand.gripUp.AddListener(jetPackOff);

        spawnedJetPackObject = Instantiate(jetPackPrefab, transform.position, transform.rotation, transform);
        particles.AddRange(spawnedJetPackObject.GetComponentsInChildren<ParticleSystem>());
        jetPackOff();
    }

    private void OnDisable()
    {
        hand.gripHold.RemoveListener(jetPack);
        hand.gripUp.RemoveListener(jetPackOff);

        Destroy(spawnedJetPackObject);
    }
    #endregion

    public List<ParticleSystem> particles;

    public GameObject jetPackPrefab;
    GameObject spawnedJetPackObject;

    void jetPack()
    {
        if (basePower.DrainOverTime(drainPerSec))
        {
            //Debug.Log("Testing Rocket");
            GetComponent<Rigidbody>().AddForce((particles[0].transform.forward * -1).normalized * force, ForceMode.VelocityChange);
            toggleParticles(true);
        }
        else
        {
            toggleParticles(false);
        }
    }

    void jetPackOff()
    {
        toggleParticles(false);
    }

    void toggleParticles(bool toggle)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].enableEmission = toggle;
        }
    }
    
}
