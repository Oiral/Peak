using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public void SpawnObject()
    {
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity, transform);
    }
}
