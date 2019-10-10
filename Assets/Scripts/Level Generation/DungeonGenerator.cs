using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemRandom = System.Random;

//Only if we are using the editor
#if UNITY_EDITOR
using UnityEditor;
#endif

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GenerateLevelSlowly());
        }
    }

    IEnumerator GenerateLevelSlowly()
    {
        yield return new WaitForSeconds(0.1f);
        ResetLevel();

        if (randomiseSeed)
        {
            seed = randomGenerator.Next(0, 90000);
        }
        randomGenerator = new SystemRandom(seed);
        Debug.Log(seed);

        yield return new WaitForSeconds(0.1f);
        //Generate the bottom structure
        GenerateBase(storagethingy.genLvl1);

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < randomGenerator.Next(Mathf.Max(0, maxSize - 5), maxSize + 1); i++)
        {
            //Generate A section
            if (connectionsToConnect.Count > 0)
            {
                yield return new WaitForSeconds(0);
                //Generate a section
                int connectionNumber = randomGenerator.Next(0, connectionsToConnect.Count - 1);

                GenerateSection(connectionsToConnect[connectionNumber]);
                connectionsToConnect.RemoveAt(connectionNumber);
            }

        }
    }
    [ContextMenu("Test Function")]
    void TestFunction()
    {
        storagethingy.genLvl1[0].GetComponent<DungeonSection>().GetPoints();

        Debug.Log(storagethingy.genLvl1[0].GetComponent<DungeonSection>().horizontalConnectionPoints.Count);
        Debug.Log(storagethingy.genLvl1[0].GetComponent<DungeonSection>().verticalConnectionPoints.Count);
    }

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
            //if we are using a max
            if (storagethingy.GenListMax > 0)
            {
                //If the current pool of generation is greater than the max
                if (connectionsToConnect.Count > storagethingy.GenListMax)
                {
                    //Remove the first half of the list
                    for (int c = 0; c < storagethingy.GenListMax/2; c++)
                    {
                        connectionsToConnect.RemoveAt(0);
                    }
                    //connectionsToConnect.RemoveRange(0, (int)(storagethingy.GenListMax / 2));
                }
            }



            //Generate A section
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                int connectionNumber = randomGenerator.Next(0, connectionsToConnect.Count - 1);

                GenerateSection(connectionsToConnect[connectionNumber]);
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

        

        GameObject spawnedObject = SpawnObject(startingList[randomGenerator.Next(0, startingList.Count - 1)], transform);
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

        //Check if we can acutally spawn the item
        bool isclear = CheckIfClear(connectionPoint);

        if (isclear == false)
        {
            //we are not clear

            //Remove the current point from the list to spawn
            connectionsToConnect.Remove(connectionPoint);

            //Generate another section
            GenerateSection(connectionsToConnect[0]);

            Debug.Log("Collided with something", connectionPoint.gameObject);
            return;
        }

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
        GameObject spawnedObject = SpawnObject(toSpawnList[randomGenerator.Next(0, toSpawnList.Count)], transform);
        //Reset the position of the prefab
        spawnedObject.transform.position = Vector3.zero;

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
        
        //if we want to remove some do so now
        if (storagethingy.enableRandomNoSpawn)
        {
            //Do not add some of these to the spawning tower
            spawnedVertConnections = RemoveFromListWithChance(spawnedVertConnections);
            spawnedConnections = RemoveFromListWithChance(spawnedConnections);
        }

        //Get all vertical connection points
        //And check if they are above the current point

        //Add the vertical connections to the main list
        AddVerticalConnections(spawnedVertConnections, spawnedObject);

        //Add the horiztonal connections to the main list
        connectionsToConnect.AddRange(spawnedConnections);

        //Add this part to the generated list
        generatedParts.Add(spawnedObject);

        connectionsToConnect.Remove(connectionPoint);
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

    bool CheckIfClear(DungeonConnection connection)
    {
        bool isClear = false;

        Vector3 direction;
        Vector3 center;

        center = connection.transform.position;

        Vector3 size = Vector3.one;

        size *= connection.level / 2;

        if (connection.type == connectionType.Vertical)
        {
            //If it is a vertical connection point
            //Check if it is clear either above or below
            if (connection.transform.localPosition.y > 0)
            {
                //Check if above is clear
                direction = Vector3.up;
            }
            else
            {
                //Check if below is clear
                direction = Vector3.down;
            }
        }
        else
        {
            //If it is a horizontal connection point
            //Check if it is clear in the direction of forward
            direction = connection.transform.forward;
            center.y -= connection.level / 2;
        }

        //Lets acutally do the raycast
        RaycastHit hit;
        isClear = Physics.BoxCast(center, size, direction, out hit, Quaternion.identity, connection.level);

        //If we hit something anwer will be true, but we want to be false
        //If we dont hit something answer will be false, but we want to be true

        //Flip the output of isClear
        isClear = !isClear;

        return isClear;
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

    public List<DungeonConnection> RemoveFromListWithChance(List<DungeonConnection> connectionPoints)
    {
        List<DungeonConnection> tempList = new List<DungeonConnection>();

        foreach (DungeonConnection connection in connectionPoints)
        {
            if ((float)randomGenerator.NextDouble() > storagethingy.chanceToNotSpawn)
            {
                tempList.Add(connection);
            }
            
        }
        return tempList;
    }

    GameObject SpawnObject(GameObject toSpawn, Transform parent)
    {
        GameObject spawnedObject = null;

        

        //Only if we are using the editor
#if UNITY_EDITOR
        spawnedObject = PrefabUtility.InstantiatePrefab(toSpawn, parent) as GameObject;

#else
         spawnedObject = Instantiate(toSpawn, parent);
#endif

        return spawnedObject;
    }
}
