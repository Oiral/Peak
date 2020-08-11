using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedUpdater : MonoBehaviour
{
    public LockManager lockMan;

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody.GetComponent<Seed>() != null)
        {
            other.attachedRigidbody.GetComponent<Seed>().towerSeed = lockMan.GetNumber();
        }
    }
}
