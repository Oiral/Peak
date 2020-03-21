using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGenAndPlayer : MonoBehaviour
{
    private bool moving;
    public float moveSpeed = 50f;

    public float TowerHeight;

    public float extraDropHeight = 50f;

    public List<GameObject> playerObjects = new List<GameObject>();

    public float playerStopHeight = 1f;

    private void FixedUpdate()
    {
        if (moving)
        {
            //Move the player

            DropPlayer();

            Vector3 pos = transform.position;
            pos.y -= moveSpeed * Time.fixedDeltaTime;

            transform.position = pos;

            if (pos.y < -(TowerHeight + extraDropHeight))
            {
                moving = false;
                pos.y = 0;
                transform.position = pos;
                //GetComponent<TowerGenerator>().GenerateLevelStart();
                GetComponent<TowerGenerator>().GenerateTower();
            }
        }
    }

    [ContextMenu("Reset Tower")]
    public void ResetTower()
    {
        //if (GetComponent<TowerGenerator>().generating == false && moving == false)
        //{
            TowerHeight = GetComponent<TowerGenerator>().towerHeight;
            moving = true;
        //}
    }

    void DropPlayer()
    {
        foreach (GameObject playerToDrop in playerObjects)
        {
            if (playerToDrop.transform.position.y > playerStopHeight)
            {
                Vector3 pos = playerToDrop.transform.position;
                pos.y -= moveSpeed * Time.fixedDeltaTime;

                playerToDrop.transform.position = pos;
            }
        }
    }

#if UNITY_EDITOR

    public bool showDebug;

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.one * playerStopHeight, new Vector3(10, 0, 10));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetTower();
        }
    }

#endif
}
