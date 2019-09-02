using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelGeneration : ScriptableWizard
{
    public int seed;
    public float maxLength;
    private Object[] prefabs;
    


    [MenuItem("Custom/Generate Structure")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Generate Structure", typeof(LevelGeneration), "Generate");
    }

    void OnWizardCreate()
    {
        //Get all these things
        Random.seed = seed;

        prefabs = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/LevelGeneration");

        Component[] allComponents = Selection.activeGameObject.GetComponents<Component>();
        //Generate the first section
        if (allComponents.Length == 1) // Contains only Transform?
        { 
            Debug.Log("That gameobject is empty");
            GenerateSection(Selection.activeGameObject.transform, Selection.activeGameObject.transform.position);

            //If there are no more end points
            for (int i = 0; i < maxLength; i++)
            {
                if (GameObject.FindGameObjectsWithTag("GenerationEnd").Length < 0)
                {
                    foreach (GameObject endPoint in GameObject.FindGameObjectsWithTag("GenerationEnd"))
                    {
                        GenerateSection(endPoint.transform, endPoint.transform.position);
                        DestroyImmediate(endPoint);
                    }
                }
            }

            

        }
    }

    public void GenerateSection(Transform parent, Vector3 position)
    {
        GameObject newobject = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[Random.Range(0, prefabs.Length)]);
        newobject.transform.position = position;
        newobject.transform.parent = parent;

    }
}
