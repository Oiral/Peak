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
}
