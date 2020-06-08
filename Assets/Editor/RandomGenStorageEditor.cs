using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CustomEditor(typeof(RandomGenStorageSO))]
public class RandomGenStorageEditor : Editor
{

    private bool showPreview;

    string errorMessages = "";

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //This the basic unity gui
        //base.OnInspectorGUI();

        RandomGenStorageSO currentScriptable = (RandomGenStorageSO)target;

        
        if (showPreview)
        {
            if (GUILayout.Button("Disable Preview"))
            {
                showPreview = false;
            }
        }
        else
        {
            if (GUILayout.Button("Show Preview"))
            {
                showPreview = true;
            }
        }

        currentScriptable.seedVisualPrefab = EditorGUILayout.ObjectField("Seed Visual", currentScriptable.seedVisualPrefab, typeof(GameObject), false) as GameObject;
        if (currentScriptable.seedVisualPrefab == null)
        {
            errorMessages = "Missing seed visual prefab";
        }

        //The slider for the tower spread
        //We also want to invert the slider so that 99 on screen is 1 in code
        /*
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Tower Spread %");
        currentScriptable.GenListMax = 100 - EditorGUILayout.IntSlider(100 - currentScriptable.GenListMax, 0, 99);
        EditorGUILayout.EndHorizontal();
        */

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

        if (errorMessages != "")
        {
            var style = new GUIStyle(GUI.skin.box);
            style.normal.textColor = Color.red;
            style.stretchWidth = true;
            style.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(errorMessages, style);
        }

        serializedObject.ApplyModifiedProperties();
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

            //Section Error Checking
            if (section.vertical == null)
            {
                errorMessages = "Section " + section.size.ToString() + "is missing a vertical Section";
            }else if (section.verticalTop == null)
            {
                errorMessages = "Section " + section.size.ToString() + " Vertical Topper is missing";
            }
            else if (Helper.FindComponentInChildWithTag<Transform>(section.verticalTop, "Seed Topper") == null)
            {
                //Check if this top point is missing the Seed Topper
                //If we are here. It is missing a seed topper
                errorMessages = "Section " + section.size.ToString() + " Is missing a Seed Topper";
            }
            if (section.horizontal == null)
            {
                errorMessages = "Section " + section.size.ToString() + "is missing a horizontal Section";
            }

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
        //Display a basic prefab
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
