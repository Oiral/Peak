using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemRandom = System.Random;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour
{
    public RandomGenStorageSO storagethingy;

    public bool randomiseSeed;
    SystemRandom randomGenerator = new SystemRandom();
    public int seed;

    public int maxSize;

    public List<DungeonConnection> connectionsToConnect;

    public List<GameObject> generatedParts;

    [ContextMenu("Generate \"Dungeon\"")]
    void GenerateLevel()
    {
        ResetLevel();

        if (randomiseSeed)
        {
            seed = randomGenerator.Next(0, 90000);
        }
        randomGenerator = new SystemRandom(seed);

        GenerateBase(storagethingy.genLvl1);

        for (int i = 0; i < randomGenerator.Next(Mathf.Max(0, maxSize-5), maxSize + 1); i++)
        {
            //Generate A section
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                int connectionNumber = randomGenerator.Next(0, connectionsToConnect.Count - 1);

                GenerateSection(connectionsToConnect[connectionNumber]);
                connectionsToConnect.RemoveAt(connectionNumber);
            }

        }
    }

    void GenerateBase(List<GameObject> startingList)
    {
        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(startingList[randomGenerator.Next(0, startingList.Count - 1)], transform) as GameObject;
        DungeonSection section = spawnedObject.GetComponent<DungeonSection>();

        section.GetPoints();

        AddVerticalConnections(section.verticalConnectionPoints, spawnedObject);

        connectionsToConnect.AddRange(section.horizontalConnectionPoints);
    }

    void GenerateSection(DungeonConnection connectionPoint)
    {
        //Find the list to spawn from
        List<GameObject> toSpawnList;

        bool horizontal = (connectionPoint.type == connectionType.Horizontal);

        if (horizontal)
        {
            //If we are connecting horizontally
            switch (connectionPoint.level)
            {
                case 1:
                    toSpawnList = storagethingy.genLvl1;
                    break;
                case 2:
                    toSpawnList = storagethingy.genLvl2;
                    break;
                default:
                    toSpawnList = storagethingy.genLvl1;
                    break;
            }
        }
        else
        {
            //If we are connecting vertically
            switch (connectionPoint.level)
            {
                case 1:
                    toSpawnList = storagethingy.genLvl1Vert;
                    break;
                case 2:
                    toSpawnList = storagethingy.genLvl2Vert;
                    break;
                default:
                    toSpawnList = storagethingy.genLvl1Vert;
                    break;
            }
        }

        //Find the correct prefab to spawn

        //Remove the connection point
        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(toSpawnList[randomGenerator.Next(0, toSpawnList.Count)], transform) as GameObject;
        DungeonSection section = spawnedObject.GetComponent<DungeonSection>();
        section.GetPoints();

        List<DungeonConnection> spawnedConnections = section.horizontalConnectionPoints;
        List<DungeonConnection> spawnedVertConnections = section.verticalConnectionPoints;


        int startingConnectionPointNumber = 0;
        
        if (horizontal)
        {
            if (spawnedConnections.Count > 0)
            {
                //If we are connecting horizontally
                startingConnectionPointNumber = randomGenerator.Next(0, spawnedConnections.Count - 1);

                spawnedObject.transform.Rotate(Vector3.up * 180 - spawnedConnections[startingConnectionPointNumber].gameObject.transform.localEulerAngles);

                spawnedObject.transform.Rotate(connectionPoint.transform.eulerAngles);

                Vector3 pos = -spawnedConnections[startingConnectionPointNumber].gameObject.transform.position;
                pos += connectionPoint.transform.position;

                spawnedObject.transform.position = pos;

                spawnedConnections.RemoveAt(startingConnectionPointNumber);
            }
        }
        else
        {
            //If we are connecting vertically
            

            List<DungeonConnection> connectionsAtBottom = new List<DungeonConnection>();

            foreach (DungeonConnection connection in spawnedVertConnections)
            {
                if (connection.transform.position.y < spawnedObject.transform.position.y)
                {
                    connectionsAtBottom.Add(connection);
                }
            }

            if (connectionsAtBottom.Count > 0)
            {
                startingConnectionPointNumber = randomGenerator.Next(0, connectionsAtBottom.Count - 1);

                Vector3 pos = -connectionsAtBottom[startingConnectionPointNumber].gameObject.transform.position;
                pos += connectionPoint.transform.position;

                spawnedObject.transform.position = pos;

                //Give it a random rotation in increments of 90 degrees
                spawnedObject.transform.rotation = Quaternion.Euler(0, randomGenerator.Next(0, 4) * 90, 0);

                spawnedVertConnections.Remove(connectionsAtBottom[startingConnectionPointNumber]);
            }
        }

        

        //Get all vertical connection points
        //And check if they are above the current point


        AddVerticalConnections(spawnedVertConnections, spawnedObject);


        connectionsToConnect.AddRange(spawnedConnections);
        generatedParts.Add(spawnedObject);
    }

    void AddVerticalConnections(List<DungeonConnection> connectionPoints, GameObject spawned)
    {
        List<DungeonConnection> checkedConnections = new List<DungeonConnection>();

        foreach (DungeonConnection connection in connectionPoints)
        {
            if (connection.gameObject.transform.position.y > spawned.transform.position.y)
            {
                checkedConnections.Add(connection);
            }
        }

        connectionsToConnect.AddRange(checkedConnections);
    }


    void ResetLevel()
    {
        /* 
        foreach (Transform child in transform)
        {
            if (child != transform)
            {
                GameObject.DestroyImmediate(child.gameObject);
            }
        }
        */

        //Destroy all children
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));

        connectionsToConnect = new List<DungeonConnection>();
        generatedParts = new List<GameObject>();

        //Reset the current working height
        //currentWorkingHeight = 0;
    }
}
