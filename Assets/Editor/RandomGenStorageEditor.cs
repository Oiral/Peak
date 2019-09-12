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

    [SerializeField]
    bool showLevel1 = false;
    [SerializeField]
    bool showLevel2 = false;

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
        //basicPrefabDisplay(ref currentScriptable.middleSection, "Middle" , ref showMiddleSection);
        //RareBuildingLayout(gen);

        //basicPrefabDisplay(ref currentScriptable.baseSection, "Floor", ref showBaseSection);

        if (showLevel1 = GUILayout.Toggle(showLevel1, "Show Level 1"))
        {
            GUILayout.Space(10);
            basicPrefabDisplay(ref currentScriptable.genLvl1, "Size 1", "Only for things that have horizontal connectors");

            basicPrefabDisplay(ref currentScriptable.genLvl1Vert, "Size 1 Vertical", "Only for things that have a bottom vertical connector");
        }

        if (showLevel2 = GUILayout.Toggle(showLevel2, "Show Level 2"))
        {
            GUILayout.Space(10);
            basicPrefabDisplay(ref currentScriptable.genLvl2, "Size 2", "Only for things that have horizontal connectors");
            
            basicPrefabDisplay(ref currentScriptable.genLvl2Vert, "Size 2 Vertical", "Only for things that have a bottom vertical connector");
        }

        GUILayout.Space(10);


    }


    public void basicPrefabDisplay(ref List<GameObject> refList, string name, string comment, ref bool showToggle)
    {
        GUILayout.Space(10);
        GUILayout.Label("------" + name + "------");

        if (comment != "")
        {
            GUILayout.Space(5);
            GUILayout.Label(comment);
        }

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
                    EditorUtility.SetDirty(target);

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
                EditorUtility.SetDirty(target);
            }
        }
    }

    public void basicPrefabDisplay(ref List<GameObject> refList, string name, string comment)
    {
        GUILayout.Space(10);
        GUILayout.Label("------" + name + "------");

        if (comment != "")
        {
            GUILayout.Space(5);
            GUILayout.Label(comment);
        }

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
                EditorUtility.SetDirty(target);
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
            EditorUtility.SetDirty(target);
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
