using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum connectionType { Vertical, Horizontal };
public class TowerConnection : MonoBehaviour
{
    public connectionType type = connectionType.Horizontal;
    public int level = 1;
    

    private void OnDrawGizmosSelected()
    {
        //Draw the correct Gizmos
        Gizmos.color = Color.green;
        Vector3 pos = Vector3.zero;
        Vector3 size = Vector3.one;
        //Use a switch instead of a if incase we want to add another type of connection point
        switch (type)
        {
            case connectionType.Vertical:

                
                pos += transform.position;

                //Draw the sphere that makes up the body
                Gizmos.DrawWireSphere(pos, size.x / 5f);
                //pos.z = 1f / 5f;
                size /= 4;


                //Draw the Cube that shows which direction
                //Show for vertical is we are above or below
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
                //Draw the sphere that makes up the body
                Gizmos.DrawWireSphere(pos, size.x / 5f);
                
                //Draw the front cube to show connection forward
                pos.z = 1f / 5f;
                size /= 4;

                Gizmos.DrawWireCube(pos, size);

                //Draw the top cube to show connection up
                size.y /= 4;
                pos.z = 0;
                pos.y = 1f / 8f;
                Gizmos.DrawWireCube(pos, size);
                break;
        }
        
    }
}
