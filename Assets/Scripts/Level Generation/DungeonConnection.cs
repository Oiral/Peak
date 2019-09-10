using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum connectionType { Vertical, Horizontal };
public class DungeonConnection : MonoBehaviour
{
    public connectionType type = connectionType.Horizontal;
    public int level = 1;

    private void OnDrawGizmosSelected()
    {
        //Draw the correct Gizmos
        Gizmos.color = Color.green;

        Gizmos.matrix = transform.localToWorldMatrix;
        Vector3 pos = Vector3.zero;
        Vector3 size = Vector3.one;
        Gizmos.DrawWireSphere(pos, size.x/5f);

        pos.z = 1f/5f;
        size /= 4;

        Gizmos.DrawWireCube(pos, size);
    }
}
