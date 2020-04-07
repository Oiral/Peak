using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    #region SINGLETON
    public static TowerManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Debug.Log("Found two instances of Tower Manager, Dstroying this", gameObject);
            Destroy(this);
        }
    }


    #endregion

    public TowerGenerator generator;
    public GameObject seedRestingPoint;
    public MoveGenAndPlayer moveTower;

    Seed currentSeed;

    public void GetSeed(Seed seed)
    {
        generator.randomiseSeed = false;
        generator.seed = seed.towerSeed;

        generator.towerStorage = seed.storage;

        //generator.GenerateTower();
    }

    public void SeedEnterSpot(Seed seed)
    {
        //We need to check if we are generating a tower already

        //First we need to change some things on the generator

        generator.StopGeneration();

        GetSeed(seed);

        //we might need to replace a seed
        if (currentSeed != null)
        {
            //We need to drop the tower and then generate
            moveTower.ResetTower();
        }
        else
        {
            //We just need to generate the tower
            moveTower.ResetTower();
        }
        currentSeed = seed;
        
    }

    public void SeedLeftSpot(Seed seed)
    {
        if (seed == currentSeed)
        {
            //Remove the current seed and destroy the tower.
            //We don't care if this seed is not the current seed
            generator.StopGeneration();
            currentSeed = null;
        }
    }
}
