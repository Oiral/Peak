using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public List<GameObject> middleSection;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> baseSection;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl1;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl2;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl1Vert;

    [HideInInspector]
    [SerializeField]
    public List<GameObject> genLvl2Vert;
}
