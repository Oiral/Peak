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

    public void GetSeed(Seed seed)
    {
        generator.randomiseSeed = false;
        generator.seed = seed.towerSeed;

        generator.towerStorage = seed.storage;

        generator.GenerateTower();

        seed.gameObject.transform.position = seedRestingPoint.transform.position;
        seed.GetComponent<Rigidbody>().Sleep();

    }
}
