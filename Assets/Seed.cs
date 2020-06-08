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
                if (currentSeedVisual != null)
                {
#if UNITY_EDITOR
                    Debug.LogError("Trying to delete seed during edit mode. Will work in play mode - Just shrunk to 0,0,0");
                    //If there is a seed visual to change
                    currentSeedVisual.transform.localScale = Vector3.zero;
#else
                    //Destroy the current seed visual
                    Destroy(currentSeedVisual);

#endif
                }
                //Spawn in the new visual
                currentSeedVisual = Instantiate(storage.seedVisualPrefab, transform.position, transform.rotation, transform);
            }
        }

        TextMeshPro displayText = null;

        if (currentSeedVisual == null)
        {
            displayText = GetComponentInChildren<TextMeshPro>();
        }
        else
        {
            displayText = currentSeedVisual.GetComponentInChildren<TextMeshPro>();

            //If we can't find the display text in the seed visual, lets look for it in the seed
            if (displayText == null)
            {
                displayText = GetComponentInChildren<TextMeshPro>();
            }
        }


        if (displayText != null)
        {
            if (blankSeed)
            {
                //Change the seed text
                displayText.text = "Blank Seed";
            }
            else
            {
                //Change the seed text
                displayText.text = internalSeed.ToString();
            }
        }
        else
        {
            Debug.LogWarning("Cannot find text mesh pro for seed display", gameObject);
        }
        
    }

    private void Start()
    {
        UpdateSeedInfo();
    }

    
}
