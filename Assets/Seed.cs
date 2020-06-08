using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Seed : MonoBehaviour
{
    public GameObject currentSeedVisual;

    public bool blankSeed;

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
#if UNITY_EDITOR
                Debug.LogError("Trying to delete seed during edit mode. Will work in play mode - Just shrunk to 0,0,0");
                currentSeedVisual.transform.localScale = Vector3.zero;
#else
                Destroy(currentSeedVisual);

#endif

                //Spawn in the new visual
                currentSeedVisual = Instantiate(storage.seedVisualPrefab, transform.position, transform.rotation, transform);
            }
        }

        if (blankSeed)
        {
            //Change the seed text
            GetComponentInChildren<TextMeshPro>().text = "Blank Seed";
        }
        else
        {
            //Change the seed text
            GetComponentInChildren<TextMeshPro>().text = internalSeed.ToString();
        }
        
    }

    private void Start()
    {
        UpdateSeedInfo();
    }

    
}
