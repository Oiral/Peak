using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SectionType {Vertical, Horizontal, Both};

public class TowerSection : MonoBehaviour
{
    [HideInInspector]
    public List<TowerConnection> horizontalConnectionPoints;
    [HideInInspector]
    public List<TowerConnection> verticalConnectionPoints;

    public SectionType type = SectionType.Vertical;
    public List<int> size;

    //Add all the connections points in child
    //This means we don't have to manually assign them in
    [ContextMenu("Get Connection Points")]
    public void GetPoints()
    {
        horizontalConnectionPoints = new List<TowerConnection>();
        verticalConnectionPoints = new List<TowerConnection>();

        bool hasHoriztonal = false;
        bool hasVeritcal = false;

        foreach (TowerConnection conPoint in GetComponentsInChildren<TowerConnection>())
        {
            if (conPoint.type == connectionType.Horizontal)
            {
                horizontalConnectionPoints.Add(conPoint);
                hasHoriztonal = true;
            }
            else
            {
                verticalConnectionPoints.Add(conPoint);
                //Check if vertical is below if so set has vertical to true
                if (conPoint.transform.localPosition.y < 0)
                {
                    hasVeritcal = true;
                }
            }

            if (size.Contains(conPoint.level)  == false)
            {
                size.Add(conPoint.level);
            }

        }


        //Lets check the type of piece this is
        if (hasHoriztonal && hasVeritcal)
        {
            type = SectionType.Both;
        }
        else if (hasVeritcal)
        {
            type = SectionType.Vertical;
        }else if (hasHoriztonal)
        {
            type = SectionType.Horizontal;
        }
        else
        {
            Debug.LogError("Piece has no conneciton points");
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
