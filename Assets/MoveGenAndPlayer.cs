using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGenAndPlayer : MonoBehaviour
{
    private bool moving;
    public float moveSpeed = 1f;

    public float TowerHeight;

    public float extraDropHeight;

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector3 pos = transform.position;
            pos.y -= moveSpeed * Time.fixedDeltaTime;

            transform.position = pos;

            if (pos.y < -(TowerHeight + extraDropHeight))
            {
                moving = false;
                pos.y = 0;
                transform.position = pos;
                GetComponent<DungeonGenerator>().GenerateLevelStart();
            }
        }
    }

    [ContextMenu("Reset Tower")]
    public void ResetTower()
    {
        if (GetComponent<DungeonGenerator>().generating == false && moving == false)
        {
            TowerHeight = GetComponent<DungeonGenerator>().towerHeight;
            moving = true;
        }
        
    }
}
