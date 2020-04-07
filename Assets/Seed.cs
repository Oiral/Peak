using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Seed : MonoBehaviour
{
    public GameObject currentSeedVisual;

    public RandomGenStorageSO storage {
        get
        {
            return internalStorage;
        }
        set
        {
            internalStorage = value;
            UpdateSeedInfo();
        }
    }
    
    [SerializeField]
    RandomGenStorageSO internalStorage;
    
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

    [ContextMenu("Update visuals for seed")]
    public void UpdateSeedInfo()
    {
        //Maybe change the look depending on the storage type?
        //Change the seed visuals
        if (storage != null)
        {
            if (storage.seedVisualPrefab != null)
            {
                //If we actually have visuals to change to
                //Destroy the current seed visual
                Destroy(currentSeedVisual);

                //Spawn in the new visual
                currentSeedVisual = Instantiate(storage.seedVisualPrefab, transform.position, transform.rotation, transform);
            }
        }

        //Change the seed text
        GetComponentInChildren<TextMeshPro>().text = internalSeed.ToString();
    }

    private void Start()
    {
        UpdateSeedInfo();
    }

    
}
