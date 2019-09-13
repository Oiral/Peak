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

    public int startingLvl = 1;

    [ContextMenu("Generate \"Dungeon\"")]
    void GenerateLevel()
    {
        ResetLevel();

        if (randomiseSeed)
        {
            seed = randomGenerator.Next(0, 90000);
        }
        randomGenerator = new SystemRandom(seed);

        //Generate the bottom structure
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
        switch (startingLvl)
        {
            case 1:
                startingList = storagethingy.genLvl1;
                break;
            case 2:
                startingList = storagethingy.genLvl2;
                break;
            default:
                startingList = storagethingy.genLvl1;
                break;
        }

        GameObject spawnedObject = PrefabUtility.InstantiatePrefab(startingList[randomGenerator.Next(0, startingList.Count - 1)], transform) as GameObject;
        DungeonSection section = spawnedObject.GetComponent<DungeonSection>();

        section.GetPoints();

        //Only add the horizontal so that there is no vertical attached to this
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

            //Make sure we are generating from the correct level
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

            //Make sure we are generating from the correct level
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
                //Find all connection points that match the correct level to connect to
                List<DungeonConnection> levelCheckedConnections = GetCorrectLevel(spawnedConnections, connectionPoint.level);

                //If we are connecting horizontally
                startingConnectionPointNumber = randomGenerator.Next(0, levelCheckedConnections.Count - 1);

                //Rotate to the correct rotation
                spawnedObject.transform.Rotate(Vector3.up * 180 - levelCheckedConnections[startingConnectionPointNumber].gameObject.transform.localEulerAngles);

                spawnedObject.transform.Rotate(connectionPoint.transform.eulerAngles);

                //Move to the correct position
                Vector3 pos = -levelCheckedConnections[startingConnectionPointNumber].gameObject.transform.position;
                pos += connectionPoint.transform.position;

                spawnedObject.transform.position = pos;

                spawnedConnections.Remove(levelCheckedConnections[startingConnectionPointNumber]);
            }
        }
        else
        {
            //If we are connecting vertically

            List<DungeonConnection> connectionsAtBottom = new List<DungeonConnection>();

            //Check if all of these are below the object to connect to
            foreach (DungeonConnection connection in spawnedVertConnections)
            {
                if (connection.transform.position.y < spawnedObject.transform.position.y)
                {
                    connectionsAtBottom.Add(connection);
                }
            }

            //Find the correct level
            connectionsAtBottom = GetCorrectLevel(connectionsAtBottom, connectionPoint.level);

            if (connectionsAtBottom.Count > 0)
            {
                //Pick a random connection point to connect to
                startingConnectionPointNumber = randomGenerator.Next(0, connectionsAtBottom.Count - 1);

                //Give it a random rotation in increments of 90 degrees
                spawnedObject.transform.rotation = Quaternion.Euler(0, randomGenerator.Next(0, 5) * 90, 0);

                //Move it to the correct position
                Vector3 pos = -connectionsAtBottom[startingConnectionPointNumber].gameObject.transform.position;
                pos += connectionPoint.transform.position;

                spawnedObject.transform.position = pos;

                spawnedVertConnections.Remove(connectionsAtBottom[startingConnectionPointNumber]);
            }
        }

        

        //Get all vertical connection points
        //And check if they are above the current point

        //Add the vertical connections to the main list
        AddVerticalConnections(spawnedVertConnections, spawnedObject);

        //Add the horiztonal connections to the main list
        connectionsToConnect.AddRange(spawnedConnections);

        //Add this part to the generated list
        generatedParts.Add(spawnedObject);
    }

     List<DungeonConnection> GetCorrectLevel(List<DungeonConnection> connections, int level)
    {
        List<DungeonConnection> checkedConnections = new List<DungeonConnection>();

        //Loop through each inputed connection point
        foreach (DungeonConnection con in connections)
        {
            //if it is the correct level add to the new list to export
            if (con.level == level)
            {
                checkedConnections.Add(con);
            }
        }
        return checkedConnections;
    }

    void AddVerticalConnections(List<DungeonConnection> connectionPoints, GameObject spawned)
    {
        List<DungeonConnection> checkedConnections = new List<DungeonConnection>();

        foreach (DungeonConnection connection in connectionPoints)
        {
            //Check if these connection points are above, Because if below we don't want to generate it
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

        //Loop through each child and destroy them
        //Destroy all children
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));

        //Reset the connections and generated parts list
        connectionsToConnect = new List<DungeonConnection>();
        generatedParts = new List<GameObject>();

        //Reset the current working height
        //currentWorkingHeight = 0;
    }
}
