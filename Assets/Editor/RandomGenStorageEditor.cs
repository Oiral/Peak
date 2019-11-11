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
        //This the basic unity gui
        //base.OnInspectorGUI();

        RandomGenStorageSO currentScriptable = (RandomGenStorageSO)target;

        GUILayout.Space(10);

        foreach (sizedSections section in currentScriptable.sections)
        {
            //Display the section
            DisplaySizedSection(section);
        }

        if (GUILayout.Button("Add New Size"))
        {
            currentScriptable.sections.Add(new sizedSections(currentScriptable));
            EditorUtility.SetDirty(currentScriptable);
        }
    }

    public void DisplaySizedSection(sizedSections section)
    {
        RandomGenStorageSO currentScriptable = (RandomGenStorageSO)target;

        EditorGUILayout.BeginHorizontal();
        //If we actually want to see this section
        section.openInInspector = EditorGUILayout.Foldout(section.openInInspector, "Section " + section.size, true);

        //Add the option to remove this section
        if (GUILayout.Button("Remove This Section"))
        {
            EditorUtility.SetDirty(currentScriptable);
            currentScriptable.sections.Remove(section);
        }

        EditorGUILayout.EndHorizontal();

        if (section.openInInspector)
        {
            EditorGUI.indentLevel++;
            GUILayout.Label("Size");
            section.size = EditorGUILayout.IntField(section.size);

            //Show the vertical one
            //EditorGUILayout.PropertyField(section.vertical);
            if (section.verticalOpenInInspector = EditorGUILayout.Foldout(section.verticalOpenInInspector, "Vertical Section", true))
            {
                EditorGUI.indentLevel++;
                //GUILayout.Label("Test For Vertical");
                basicPrefabDisplay(ref section.vertical, "Vertical", "Testing");
                EditorGUI.indentLevel--;
            }
            //Show the horiztonal one
            if (section.horizontalOpenInInspector = EditorGUILayout.Foldout(section.horizontalOpenInInspector, "Horizontal Section", true))
            {
                EditorGUI.indentLevel++;
                //GUILayout.Label("Test For Horizontal");
                basicPrefabDisplay(ref section.horizontal, "Horizontal", "Testing");
                EditorGUI.indentLevel--;
            }
            //Show the final vertical
            section.verticalTop = EditorGUILayout.ObjectField("Vertical Top", section.verticalTop, typeof(GameObject), false) as GameObject;
            //Show the final Horiztonal
            section.horiztonalTop = EditorGUILayout.ObjectField("Horizontal Top", section.horiztonalTop, typeof(GameObject), false) as GameObject;

            EditorGUI.indentLevel--;
        }

        
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


            EditorGUI.indentLevel++;
            if (GUILayout.Button("Add New"))
            {
                //Remove
                refList.Add(null);
                EditorUtility.SetDirty(target);
            }
            EditorGUI.indentLevel--;
        }
    }

    public void basicPrefabDisplay(ref List<GameObject> refList, string name, string comment)
    {
        //GUILayout.Space(10);
        //GUILayout.Label("------" + name + "------");

        /*
        if (comment != "")
        {
            GUILayout.Space(5);
            GUILayout.Label(comment);
        }
        */

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


        EditorGUI.indentLevel++;
        if (GUILayout.Button("Add New"))
        {
            //Remove
            refList.Add(null);
            EditorUtility.SetDirty(target);
        }
        EditorGUI.indentLevel--;

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
