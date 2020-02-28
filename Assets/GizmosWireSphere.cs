using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosWireSphere : MonoBehaviour
{
    public bool showGizmoOnSelect;
    public float size;
    public Color color = Color.green;
    
    public Transform lineObject;
    public Color lineColor = Color.grey;

    private void OnDrawGizmos()
    {
        if (!showGizmoOnSelect)
            DrawGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmoOnSelect)
            DrawGizmo();
    }

    void DrawGizmo()
    {
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, size);

        if (lineObject != null)
        {
            //Draw a line between this and the parent object
            Gizmos.color = lineColor;
            Gizmos.DrawLine(transform.position, transform.position);

        }
    }

}
