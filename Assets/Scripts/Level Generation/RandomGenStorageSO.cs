using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public List<GameObject> middleSection = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public List<GameObject> baseSection = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl1 = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl2 = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl1Vert = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl2Vert = new List<GameObject>();

    [HideInInspector]
    [SerializeField]
    public float chanceToNotSpawn;

    [HideInInspector]
    [SerializeField]
    public bool enableRandomNoSpawn;
}
