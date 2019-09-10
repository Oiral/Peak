﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSection : MonoBehaviour
{
    public List<DungeonConnection> horizontalConnectionPoints;
    public List<DungeonConnection> verticalConnectionPoints;

    [ContextMenu("Get Connection Points")]
    public void GetPoints()
    {
        horizontalConnectionPoints = new List<DungeonConnection>();
        verticalConnectionPoints = new List<DungeonConnection>();

        foreach (DungeonConnection conPoint in GetComponentsInChildren<DungeonConnection>())
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
}
