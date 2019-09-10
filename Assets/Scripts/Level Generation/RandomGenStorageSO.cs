using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    [HideInInspector]
    public List<GameObject> middleSection;
    [HideInInspector]
    public List<GameObject> baseSection;
    [HideInInspector]
    public List<GameObject> genLvl1;
    [HideInInspector]
    public List<GameObject> genLvl2;
    [HideInInspector]
    public List<GameObject> genLvl1Vert;
    [HideInInspector]
    public List<GameObject> genLvl2Vert;
}
