using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public bool drawOnSelected;
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
        Gizmos.DrawWireCube(transform.position, size);
    }

    [Header("Info")]
    public Vector3 size = Vector3.one;
}
