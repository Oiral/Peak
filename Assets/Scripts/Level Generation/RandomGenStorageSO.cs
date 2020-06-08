using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class sizedSections
{
    //A storage class for each sized section.
    //This means we can use infinite amount of section and any size that we want
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

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Section Info", order = 1)]
public class RandomGenStorageSO : ScriptableObject
{
    [SerializeField]
    public List<sizedSections> sections = new List<sizedSections>();
    
    [HideInInspector]
    [SerializeField]
    //The max size for the connections to generate too
    //If the list gets above this size - removes the first half of the size
    public int GenListMax;

    [SerializeField]
    public GameObject seedVisualPrefab;

    //Get the whole section that fits the right size
    public sizedSections getSize(int num)
    {
        if (sections.Count == 0)
        {
            Debug.LogWarning("There are no sections in the tower storage");
            return null;
        }

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

    //Get the whole section that is the largest
    public sizedSections getLargestSize()
    {
        if (sections.Count == 0)
        {
            Debug.LogWarning("There are no sections in the tower storage");
            return null;
        }

        sizedSections largest = sections[0];

        //Only needs to check array index 1 - length because we have already set the largest to index 0
        for (int i = 1; i < sections.Count; i++)
        {
            if (sections[i].size > largest.size)
            {
                largest = sections[i];
            }
        }

        return largest;
    }
}
