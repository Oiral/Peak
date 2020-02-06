using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerGenerator))]
public class SwapGenerators : MonoBehaviour
{
    public List<RandomGenStorageSO> storages;

    int current = 0;

    public void CycleGenerator()
    {
        current += 1;

        if (current >= storages.Count)
        {
            current = 0;
        }

        GetComponent<TowerGenerator>().towerStorage = storages[current];
    }
}
