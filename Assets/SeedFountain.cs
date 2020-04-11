using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFountain : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Seed>() != null)
        {
            Debug.Log("Test thingy");
            //tell the thingy that we have a seed
            TowerManager.instance.SeedEnterSpot(other.GetComponent<Seed>());
        }else if (other.GetComponentInParent<Seed>() != null)
        {
            Debug.Log("Test thingy in parent");
            //tell the thingy that we have a seed
            TowerManager.instance.SeedEnterSpot(other.GetComponentInParent<Seed>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Seed>() != null)
        {
            Debug.Log("Test thingy");
            //tell the thingy that we have a seed
            TowerManager.instance.SeedLeftSpot(other.GetComponent<Seed>());
        }
        else if (other.GetComponentInParent<Seed>() != null)
        {
            Debug.Log("Test thingy in parent");
            //tell the thingy that we have a seed
            TowerManager.instance.SeedLeftSpot(other.GetComponentInParent<Seed>());
        }
    }
}
