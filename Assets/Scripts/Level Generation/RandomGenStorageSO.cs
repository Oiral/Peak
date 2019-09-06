using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    public List<GameObject> middleSection;
    public List<GameObject> baseSection;
    public List<GameObject> floor;
}
