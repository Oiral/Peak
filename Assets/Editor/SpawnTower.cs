using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class SpawnTower
{
    [MenuItem("Tower Menu/Spawn Tower")]
    static void Spawn()
    {
        GameObject go = new GameObject("Tower");

        TowerGenerator generator = go.AddComponent<TowerGenerator>();
        generator.seed = Random.Range(10000, 99999);
        generator.randomiseSeed = true;

        MoveGenAndPlayer dropTower = go.AddComponent<MoveGenAndPlayer>();

        dropTower.moveSpeed = 50;
        dropTower.extraDropHeight = 50;

        dropTower.playerObjects.Add(go);
        dropTower.playerObjects.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        dropTower.playerStopHeight = 1;

        Undo.RegisterCreatedObjectUndo((Object)go, "Undo Spawn Tower");

        Selection.activeObject = go;
    }

    [MenuItem("Tower Menu/Spawn Seed")]
    static void SpawnSeed()
    {
        GameObject go = new GameObject("Seed");

        Seed seed = go.AddComponent<Seed>();
        seed.towerSeed = Random.Range(10000, 99999);

        if (Selection.activeObject.GetType() == typeof(RandomGenStorageSO))
        {
            seed.storage = (RandomGenStorageSO)Selection.activeObject;
        }

        Undo.RegisterCreatedObjectUndo((Object)go, "Undo Spawn Seed");
        Selection.activeObject = go;
    }

    [MenuItem("Tower Menu/Spawn Seed Spot")]
    static void SpawnSeedSpot()
    {
        GameObject go = new GameObject("Seed Spot");

        go.AddComponent<SeedSpot>().showDebug = true;

        go.tag = "Seed Topper";

        Undo.RegisterCreatedObjectUndo((Object)go, "Undo Spawn Seed Spot");
        Selection.activeObject = go;
    }
}
