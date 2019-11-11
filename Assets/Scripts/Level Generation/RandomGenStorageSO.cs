using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sizedSections
{
    [SerializeField]
    public int size = 1;

    [SerializeField]
    public List<GameObject> vertical = new List<GameObject>();
    [SerializeField]
    public List<GameObject> horizontal = new List<GameObject>();

    [SerializeField]
    public GameObject verticalTop = null;
    [SerializeField]
    public GameObject horiztonalTop = null;

    [SerializeField]
    public bool openInInspector = false;
    [SerializeField]
    public bool verticalOpenInInspector = false;
    [SerializeField]
    public bool horizontalOpenInInspector = false;

    public sizedSections(RandomGenStorageSO storage)
    {
        if (storage.sections.Count >= 1)
        {
            size = storage.getLargestSize().size + 1;
        }
        else
        {
            size = 1;
        }
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    //[HideInInspector]
    //[SerializeField]
    //public List<GameObject> middleSection = new List<GameObject>();

    //[HideInInspector]
    //[SerializeField]
    //public List<GameObject> baseSection = new List<GameObject>();

    /*
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

*/

    [SerializeField]
    public List<sizedSections> sections = new List<sizedSections>();


    [HideInInspector]
    [SerializeField]
    public float chanceToNotSpawn;

    [HideInInspector]
    [SerializeField]
    public bool enableRandomNoSpawn;

    [HideInInspector]
    [SerializeField]
    public float GenListMax;

    public sizedSections getSize(int num)
    {
        foreach (sizedSections section in sections)
        {
            if (section.size == num)
            {
                return section;
            }
        }
        Debug.LogError("Cannot find correct size for end");
        return null;
    }

    public sizedSections getLargestSize()
    {
        if (sections.Count == 0)
        {
            return null;
        }

        sizedSections largest = sections[0];

        foreach (sizedSections section in sections)
        {
            if (section.size > largest.size)
            {
                largest = section;
            }
        }

        return largest;
    }
}
