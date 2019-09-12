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
        Vector3 pos = Vector3.zero;
        Vector3 size = Vector3.one;
        switch (type)
        {
            case connectionType.Vertical:

                pos += transform.position;

                Gizmos.DrawWireSphere(pos, size.x / 5f);
                //pos.z = 1f / 5f;
                size /= 4;

                if (transform.localPosition.y >= 0)
                {

                    pos.y += 1f / 5;

                }
                else
                {
                    pos.y -= 1f / 5;
                }

                Gizmos.DrawWireCube(pos, size);
                break;
            default:
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireSphere(pos, size.x / 5f);

                pos.z = 1f / 5f;
                size /= 4;

                Gizmos.DrawWireCube(pos, size);

                size.y /= 4;
                pos.z = 0;
                pos.y = 1f / 8f;
                Gizmos.DrawWireCube(pos, size);
                break;
        }
        
    }
}
