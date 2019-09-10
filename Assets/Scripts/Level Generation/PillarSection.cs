using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemRandom = System.Random;

public class PillarSection : MonoBehaviour
{
    public bool drawOnSelected;

    public bool beenGenerated = false;

    private void OnDrawGizmos()
    {
        if (drawOnSelected == false)
        {
            DrawTheGizmos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawOnSelected)
        {
            DrawTheGizmos();
        }
    }

    void DrawTheGizmos()
    {
        if (beenGenerated == false)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }
    }

    [Header("Info")]
    public Vector3 size = Vector3.one;


    public RandomGenStorageSO testScriptable;

    [ContextMenu("Generate Test Pillar")]
    public void TestGenPillar()
    {
        if (testScriptable != null)
        {
            GeneratePillar(testScriptable, new SystemRandom());
        }
        else
        {
            Debug.Log("Test Scriptable not set", gameObject);
        }
    }

    public void GeneratePillar(RandomGenStorageSO scriptable, SystemRandom randomGenerator)
    {
        ClearGeneratedPillars();

        //Generate some pillars
        //Find each piece and height
    }

    public void ClearGeneratedPillars()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }
}
