using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHand))]
[RequireComponent(typeof(PowerBase))]
public class PowerCube : MonoBehaviour
{
    #region Power Stuff
    PhysicsHand hand;

    private void Awake()
    {
        hand = GetComponent<PhysicsHand>();
    }

    private void OnEnable()
    {
        hand.gripHold.AddListener(spawnCube);
        cubeParentObject = new GameObject("Cube Parent");
    }

    private void OnDisable()
    {
        hand.gripHold.RemoveListener(spawnCube);
    }
    #endregion

    public GameObject cubePrefab;
    GameObject cubeParentObject;
    public Transform cubeSpawnLocation;

    void spawnCube()
    {
        Instantiate(cubePrefab, cubeSpawnLocation.position, cubeSpawnLocation.rotation, cubeParentObject.transform);
    }

}
