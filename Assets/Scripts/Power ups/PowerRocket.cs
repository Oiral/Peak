using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHand))]
public class PowerRocket : MonoBehaviour
{
    #region Power Stuff
    PhysicsHand hand;

    public Vector3 direction;

    public float force = 1;

    private void Awake()
    {
        hand = GetComponent<PhysicsHand>();
    }

    private void OnEnable()
    {
        hand.gripHold.AddListener(jetPack);
    }

    private void OnDisable()
    {
        hand.gripHold.RemoveListener(jetPack);
    }
    #endregion

    void jetPack()
    {
        Debug.Log("Testing Rocket");
        GetComponent<Rigidbody>().AddForce((transform.forward + direction).normalized * force, ForceMode.VelocityChange);
    }
}
