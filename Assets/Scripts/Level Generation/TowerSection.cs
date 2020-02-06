using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSection : MonoBehaviour
{
    [HideInInspector]
    public List<TowerConnection> horizontalConnectionPoints;
    [HideInInspector]
    public List<TowerConnection> verticalConnectionPoints;

    //Add all the connections points in child
    //This means we don't have to manually assign them in
    [ContextMenu("Get Connection Points")]
    public void GetPoints()
    {
        horizontalConnectionPoints = new List<TowerConnection>();
        verticalConnectionPoints = new List<TowerConnection>();

        foreach (TowerConnection conPoint in GetComponentsInChildren<TowerConnection>())
        {
            if (conPoint.type == connectionType.Horizontal)
            {
                horizontalConnectionPoints.Add(conPoint);
            }
            else
            {
                verticalConnectionPoints.Add(conPoint);
            }
        }
    }

    //Get all vertical sections either above or below connection points
    public List<TowerConnection> GetVertical(bool checkTop)
    {
        List<TowerConnection> checkedList = new List<TowerConnection>();

        //Loop through
        foreach (TowerConnection connection in verticalConnectionPoints)
        {
            //If we are checking for the top
            if (checkTop && connection.transform.localPosition.y > 0)
            {
                checkedList.Add(connection);
            }else if (checkTop == false && connection.transform.localPosition.y <= 0)
            {
                checkedList.Add(connection);
            }
        }

        return checkedList;
    }
}
