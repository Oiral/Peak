using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedSpot : MonoBehaviour
{
    public bool showDebug;
    public float radius = 1f;

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
