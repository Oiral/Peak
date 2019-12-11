using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    #region singleton
    //Create a singleton
    public static CollectionManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }else if (instance != this)
        {
            Debug.LogWarning("Found two cases of collection manager, Destroying");
            Destroy(gameObject);
        }
            
    }
    #endregion

    public List<GameObject> nextSelectablePrefabs;

    public RandomGenStorageSO storage;

    public void SpawnSelectionItems()
    {

        if (nextSelectablePrefabs.Count > 0)
        {
            int count = 0;
            foreach (GameObject selectionPiece in GameObject.FindGameObjectsWithTag("SelectionPieces"))
            {
                //Add in a random selectable prefab
                if (nextSelectablePrefabs.Count < count + 1)
                {
                    break;
                }
                SpawnNextSelectable(nextSelectablePrefabs[0], selectionPiece);
                count++;
            }
        }

        //nextSelectablePrefabs;
    }

    void SpawnNextSelectable(GameObject toSpawn, GameObject objectToSpawnAt)
    {
        Instantiate(toSpawn, objectToSpawnAt.transform.position, objectToSpawnAt.transform.rotation, objectToSpawnAt.transform);
    }

    public void SelectPrefab(GameObject prefabToSelect)
    {
        
        if (nextSelectablePrefabs.Contains(prefabToSelect))
        {
            //Remove this from the storage
            nextSelectablePrefabs.Remove(prefabToSelect);

            //Add it to the current storage
            //storage
        }
    }
}
