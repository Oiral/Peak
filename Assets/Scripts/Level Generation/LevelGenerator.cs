﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SystemRandom = System.Random;

public class LevelGenerator : MonoBehaviour
{

    public RandomGenStorageSO randomGen;

    SystemRandom randomGenerator = new SystemRandom();

    float currentWorkingHeight = 0;
    public int seed;

    public int maxHeight = 10;
    private void Start() {
        
        GenerateLevel();
    }

    [ContextMenu("Generate Level")]
    void GenerateLevel(){

        //Destroy all children
        ResetLevel();

        //Set the seed for the random generation
        randomGenerator = new SystemRandom(seed);

        for (int i = 0; i < randomGenerator.Next(1, maxHeight + 1); i++)
        {
            //GenerateMiddleSection

            if (randomGen.baseSection.Count > 0)
            {
                GenerateFloorSection();
            }

            GenerateMiddleSection();
            
        }
    }
    [ContextMenu("Generate Level With coroutine")]

    void GenerateLevelCoroutine()
    {
        StartCoroutine(GenerateLevelSlowly());
    }
    IEnumerator GenerateLevelSlowly()
    {
        
        //Destroy all children
        ResetLevel();
        yield return new WaitForSeconds(0.2f);
        //Set the seed for the random generation
        randomGenerator = new SystemRandom(seed);

        for (int i = 0; i < randomGenerator.Next(1, maxHeight + 1); i++)
        {
            //GenerateMiddleSection

            if (randomGen.baseSection.Count > 0)
            {
                GenerateFloorSection();
            }
            yield return new WaitForSeconds(0.2f);
            GenerateMiddleSection();
            yield return new WaitForSeconds(0.2f);

        }
    }

    void GenerateMiddleSection(){
        //What section are we going to work with
        GameObject prefab = randomGen.middleSection[randomGenerator.Next(randomGen.middleSection.Count)];

        GenerateSection(prefab);
    }

    void GenerateFloorSection()
    {
        GameObject prefab = randomGen.baseSection[randomGenerator.Next(randomGen.baseSection.Count)];

        GenerateSection(prefab);
    }

    void GenerateSection(GameObject prefab)
    {
        Vector3 pos = new Vector3(0, currentWorkingHeight, 0);

        //Generate this section
        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(prefab, transform) as GameObject;

        Section spawnedSection = spawnedObject.GetComponent<Section>();

        pos.y += spawnedSection.size.y / 2;

        currentWorkingHeight += spawnedSection.size.y;
        Debug.Log(currentWorkingHeight);

        spawnedObject.transform.position = pos;
    }

    void ResetLevel()
    {
        /* 
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        */

        //Destroy all children
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));  

        //Reset the current working height
        currentWorkingHeight = 0;
    }
}
