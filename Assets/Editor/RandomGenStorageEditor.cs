using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomGenStorageSO))]
public class RandomGenStorageEditor : Editor
{

    private bool showPreview;

    private bool showMiddleSection = true;

    private bool showBaseSection = true;

    private bool showDungeonStuff = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        /*
        GUILayout.Label("------Settings------");


        BuildingGenerator gen = (BuildingGenerator)target;

        if (GUILayout.Button("Regen Buildings"))
        {
            //Remove
            gen.GenerateStartingBuildings();
        }

        showPreview = GUILayout.Toggle(showPreview, "Show Previews");
        */




        RandomGenStorageSO currentScriptable = (RandomGenStorageSO)target;

        //BasicBuildingLayout(gen);
        basicPrefabDisplay(ref currentScriptable.middleSection, "Middle" , ref showMiddleSection);
        //RareBuildingLayout(gen);

        basicPrefabDisplay(ref currentScriptable.baseSection, "Floor", ref showBaseSection);

        GUILayout.Space(10);
        basicPrefabDisplay(ref currentScriptable.genLvl1, "Size 1", ref showDungeonStuff);
        basicPrefabDisplay(ref currentScriptable.genLvl1Vert, "Size 1 Vertical", ref showDungeonStuff);

        GUILayout.Space(10);
        basicPrefabDisplay(ref currentScriptable.genLvl2, "Size 2", ref showDungeonStuff);
        basicPrefabDisplay(ref currentScriptable.genLvl2Vert, "Size 2 Vertical", ref showDungeonStuff);

        GUILayout.Space(10);


    }

    
    public void basicPrefabDisplay(ref  List<GameObject> refList, string name, ref bool showToggle)
    {
        GUILayout.Space(10);
        GUILayout.Label("------" + name + "------");

        if (showToggle = GUILayout.Toggle(showToggle, "Show " + name + " Prefabs")) 
        {

            GUILayout.BeginVertical();

            for (int i = 0; i < refList.Count; i++)
            {
                GUILayout.BeginHorizontal();

                string prefabName;
                if (refList[i] != null)
                {
                    prefabName = refList[i].name;
                }
                else
                {
                    prefabName = "Not Assigned";
                }

                refList[i] = EditorGUILayout.ObjectField(prefabName, refList[i], typeof(GameObject), false) as GameObject;


                if (GUILayout.Button("Remove", GUILayout.MaxWidth(70)))
                {
                    //Remove
                    refList.RemoveAt(i);
                }


                GUILayout.EndHorizontal();


                if (showPreview && refList[i] != null)
                {
                    GUILayout.Label(AssetPreview.GetAssetPreview((Object)refList[i]), GUILayout.Height(50));
                }

            }

            GUILayout.EndVertical();


            if (GUILayout.Button("Add New"))
            {
                //Remove
                refList.Add(null);
            }
        }
    }

    /*
    public void RareBuildingLayout(BuildingGenerator gen)
    {
        if (ShowRareBuildings = GUILayout.Toggle(ShowRareBuildings, "Show Rare Buildings"))
        {

            GUILayout.BeginVertical();

            for (int i = 0; i < gen.rarePrefabs.Count; i++)
            {
                GUILayout.BeginHorizontal();

                string prefabName;
                if (gen.rarePrefabs[i] != null)
                {
                    prefabName = gen.rarePrefabs[i].name;
                }
                else
                {
                    prefabName = "Not Assigned";
                }

                gen.rarePrefabs[i] = EditorGUILayout.ObjectField(prefabName, gen.rarePrefabs[i], typeof(GameObject), false) as GameObject;


                if (GUILayout.Button("Remove", GUILayout.MaxWidth(70)))
                {
                    //Remove
                    gen.rarePrefabs.RemoveAt(i);
                }


                GUILayout.EndHorizontal();


                if (showPreview && gen.rarePrefabs[i] != null)
                {
                    GUILayout.Label(AssetPreview.GetAssetPreview(gen.rarePrefabs[i]), GUILayout.Height(50));
                }

            }

            GUILayout.EndVertical();


            if (GUILayout.Button("Add New"))
            {
                //Remove
                gen.rarePrefabs.Add(null);
            }
        }
    }
    */

}
