using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoMeshRender : MonoBehaviour
{
    public Color GizmoColor = Color.red;

    private void OnDrawGizmosSelected()
    {
        Mesh displayMesh = GetComponent<MeshFilter>().sharedMesh;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = GizmoColor;
        Gizmos.DrawMesh(displayMesh, Vector3.zero, Quaternion.identity);
    }
}
