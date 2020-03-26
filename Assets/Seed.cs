using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seed : MonoBehaviour
{
    public RandomGenStorageSO storage;
    
    public int towerSeed {
        get {
            return internalSeed;
        }
        set {
            internalSeed = value;
            UpdateSeedInfo();
        }
    }

    [SerializeField]
    int internalSeed;

    public void UpdateSeedInfo()
    {
        //Maybe change the look depending on the storage type?
        //Change the seed text
        GetComponentInChildren<Text>().text = internalSeed.ToString();
    }

}
