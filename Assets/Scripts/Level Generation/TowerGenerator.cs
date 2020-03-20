﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//We want to use system.random rather than Unity's inbuilt random because we want to seed the randomness
using SystemRandom = System.Random;

//Only if we are using the editor
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TowerGenerator : MonoBehaviour
{
    public RandomGenStorageSO towerStorage;

    [Header("Seed")]
    public bool randomiseSeed;
    SystemRandom randomGenerator = new SystemRandom();
    public int seed;

    [Header("Tower Info")]
    public int maxSize;

    private List<TowerConnection> connectionsToConnect;

    private List<GameObject> generatedParts;

    [HideInInspector]
    public TowerConnection highestPoint;

    private List<GameObject> nextSelectablePrefabs;

    [HideInInspector]
    public float towerHeight;

    [Header("Layer")]
    public bool overideLayer = true;
    public int layerToSetTowerPieces;
    
    [ContextMenu("Generate Tower")]
    public void GenerateTower()
    {
        //Reset the level before we start to generate it again
        ResetLevel();

        //Get a new seed if needed
        if (randomiseSeed)
        {
            seed = randomGenerator.Next(0, 90000);
        }
        //Create a new random generator with the seed
        //We want this so we can generate the level with a seed
        randomGenerator = new SystemRandom(seed);

        if (Application.isPlaying)
        {
            StartCoroutine(generateSlowly());
        }
        else
        {
            //Generate the bottom structure
            GenerateBase();

            //Generate the main section of the tower
            GenerateMiddleSection();

            //Lets get some stats for the tower

            towerHeight = highestPoint.gameObject.transform.position.y;
            //Debug.Log(highestPoint.gameObject.transform.position.y, highestPoint.gameObject);
            //Generate "topSection"
            //Find the highest point level
            sizedSections highestSize = towerStorage.getSize(highestPoint.level);

            if (highestSize != null)
            {
                GenerateEndSection(highestPoint, highestSize.horiztonalTop, highestSize.verticalTop);
            }

        }
    }

    IEnumerator generateSlowly()
    {
        yield return 0;
        //Generate the bottom structure
        GenerateBase();
        
        //Generate the main section of the tower
        yield return StartCoroutine( GenerateMiddleSectionSlowly() );
        //Lets get some stats for the tower

        yield return 0;
        towerHeight = highestPoint.gameObject.transform.position.y;
        //Debug.Log(highestPoint.gameObject.transform.position.y, highestPoint.gameObject);
        //Generate "topSection"
        //Find the highest point level
        sizedSections highestSize = towerStorage.getSize(highestPoint.level);

        if (highestSize != null)
        {
            GenerateEndSection(highestPoint, highestSize.horiztonalTop, highestSize.verticalTop);
        }
    }

    void GenerateBase()
    {
        //Get the largest size to start with
        List<GameObject> startingList = new List<GameObject>();

        //Check if there is some horizontal points that we can spawn
        //If not try spawn in vertical
        if (towerStorage.getLargestSize().horizontal.Count > 0)
        {
            startingList = towerStorage.getLargestSize().horizontal;
        }else if (towerStorage.getLargestSize().vertical.Count > 0)
        {
            startingList = towerStorage.getLargestSize().vertical;
        }
        else
        {
            Debug.LogError("Missing Vertical and Horiztonal pieces in size - " + towerStorage.getLargestSize().size);
        }

        //Spawn in the selected piece
        GameObject spawnedObject = SpawnObject(startingList[randomGenerator.Next(0, startingList.Count - 1)], transform);
        TowerSection section = spawnedObject.GetComponent<TowerSection>();

        section.GetPoints();

        //Add the vertical connection points
        connectionsToConnect.AddRange(section.GetVertical(true));

        //Add the horizontal connection points
        connectionsToConnect.AddRange(section.horizontalConnectionPoints);
    }

    void GenerateMiddleSection()
    {
        //Generate at most 1 less that the max as we need to put a topping section on top
        for (int i = 0; i < randomGenerator.Next(Mathf.Max(0, maxSize - 5), maxSize); i++)
        {
            //Check if the genlistmax is greater than 0 as we don't want to remove each section to generate every single time
            //If the current pool of generation is greater than the max

            //Debug.Log(towerStorage.GenListMax);

            if (towerStorage.GenListMax > 0 && connectionsToConnect.Count > towerStorage.GenListMax)
            {
                //Remove the first half of the list
                for (int c = 0; c <= towerStorage.GenListMax / 2; c++)
                {
                    connectionsToConnect.RemoveAt(0);
                }
            }
            //Debug.Log(connectionsToConnect.Count);


            //If we have not run out of availiable places to place the tower
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                GenerateRandomSection(connectionsToConnect[0]);
            }

        }
    }

    IEnumerator GenerateMiddleSectionSlowly()
    {
        //Generate at most 1 less that the max as we need to put a topping section on top
        for (int i = 0; i < randomGenerator.Next(Mathf.Max(0, maxSize - 5), maxSize); i++)
        {
            //Check if the genlistmax is greater than 0 as we don't want to remove each section to generate every single time
            //If the current pool of generation is greater than the max

            //Debug.Log(towerStorage.GenListMax);

            if (towerStorage.GenListMax > 0 && connectionsToConnect.Count > towerStorage.GenListMax)
            {
                //Remove the first half of the list
                for (int c = 0; c <= towerStorage.GenListMax / 2; c++)
                {
                    connectionsToConnect.RemoveAt(0);
                }
            }
            //Debug.Log(connectionsToConnect.Count);


            //If we have not run out of availiable places to place the tower
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                GenerateRandomSection(connectionsToConnect[0]);
                yield return 0;
            }

        }
    }

    void GenerateRandomSection(TowerConnection connectionPoint)
    {
        //Find the list to spawn from
        List<GameObject> toSpawnList;

        bool horizontal = (connectionPoint.type == connectionType.Horizontal);

        //Find the spawning list we want to use
        if (horizontal)
        {
            //If we are connecting horizontally

            toSpawnList = towerStorage.getSize(connectionPoint.level).horizontal;
        }
        else
        {
            //If we are connecting vertically

            toSpawnList = towerStorage.getSize(connectionPoint.level).vertical;
        }
        
        //Randomly select a prefab to spawn
        GameObject toSpawn = toSpawnList[randomGenerator.Next(0, toSpawnList.Count)];

        //Check if we can acutally spawn the item
        GameObject isClearGameObject = CheckIfClear(connectionPoint);

        if (isClearGameObject != null)
        {
            //we are not clear

            //Remove the current point from the list to spawn
            connectionsToConnect.Remove(connectionPoint);

            if (connectionsToConnect.Count >= 1)
            {
                //Generate another section
                GenerateRandomSection(connectionsToConnect[0]);
            }
            else
            {
                Debug.LogError("No more pieces to spread too, stopping generation", highestPoint.gameObject);
            }

            //Debug.Log("Collided with something", connectionPoint.gameObject);
            return;
        }

        //Spawn in the selected object
        GameObject spawnedSection = GenerateSection(toSpawn, connectionPoint);

        connectionsToConnect.Remove(connectionPoint);

        //We want to check if we are going to create a dead end tower
        //And if we are, deleted the spawned in piece and regenerate it
        if (connectionsToConnect.Count <= 0)
        {
            Debug.Log("Created a dead end tower, Removing part");
            connectionsToConnect.Add(connectionPoint);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(spawnedSection);
            }
            else
            {
                Destroy(spawnedSection);
            }

#else
            Destroy(spawnedSection);
#endif
            GenerateRandomSection(connectionPoint);

        }
    }


    //We want to end generating the top of the tower (the vertical section)
    void GenerateEndSection(TowerConnection connectionPoint, GameObject horiztonalToSpawn, GameObject verticalToSpawn)
    {
        //Lets clear the to be connected section
        connectionsToConnect.Clear();

        bool horizontal = (connectionPoint.type == connectionType.Horizontal);
        
        //Find the correct top section to spawn

        GameObject toSpawn = null;
        //Check if there is something we can spawn
        if (horizontal)
        {
            toSpawn = horiztonalToSpawn;
        }
        else
        {
            toSpawn = verticalToSpawn;
        }

        if (toSpawn == null)
        {
            Debug.LogWarning("Missing Size " + connectionPoint.level + " end point");
            return;
        }

        //Generate the end section Either horizontal or vertical
        GenerateSection(toSpawn, connectionPoint);

        //If its horizontal we want to end on the vertical so create the end section
        if (horizontal)
        {
            GenerateEndSection(connectionsToConnect[0], horiztonalToSpawn, verticalToSpawn);
        }
    }

    GameObject GenerateSection(GameObject objectToSpawn, TowerConnection connectionPoint)
    {
        //Spawn in the section
        GameObject spawnedObject = SpawnObject(objectToSpawn, transform);

        //spawnedObject.transform.SetParent(connectionPoint.gameObject.transform.parent);

        //Make sure we get all the connection points of the section
        TowerSection section = spawnedObject.GetComponent<TowerSection>();
        section.GetPoints();

        List<TowerConnection> spawnedConnections = new List<TowerConnection>();

        TowerConnection connectionUsedToConnect = null;
        Vector3 sectionPos = new Vector3();

        if (connectionPoint.type == connectionType.Horizontal)
        {
            spawnedConnections = section.horizontalConnectionPoints;

            if (spawnedConnections.Count > 0)
            {
                //Find all connection points that match the correct level to connect to
                List<TowerConnection> levelCheckedConnections = GetCorrectSize(spawnedConnections, connectionPoint.level);

                //If we are connecting horizontally
                connectionUsedToConnect = levelCheckedConnections[randomGenerator.Next(0, levelCheckedConnections.Count - 1)];

                //Rotate to the correct rotation
                //Allign the piece with the connection point we are connecting with
                spawnedObject.transform.Rotate(Vector3.up * 180 - connectionUsedToConnect.gameObject.transform.localEulerAngles);
                
                //Rate to match the same rotation as the to connect point
                spawnedObject.transform.Rotate(connectionPoint.transform.eulerAngles);
            }
        }
        else
        {
            //If we are connecting vertically

            List<TowerConnection> connectionsAtBottom = section.GetVertical(false);

            //Find the correct level
            connectionsAtBottom = GetCorrectSize(connectionsAtBottom, connectionPoint.level);

            if (connectionsAtBottom.Count > 0)
            {
                //Pick a random connection point to connect to
                connectionUsedToConnect = connectionsAtBottom[randomGenerator.Next(0, connectionsAtBottom.Count - 1)];

                //Give it a random rotation in increments of 90 degrees
                spawnedObject.transform.rotation = Quaternion.Euler(0, (randomGenerator.Next(0, 3) * 90) + connectionPoint.transform.rotation.eulerAngles.y, 0);
                
            }
        }

        //Create a list of all the connection points
        List<TowerConnection> generatedConnections = new List<TowerConnection>();

        //For the vertical sections we only want to add the vertical connections that are going upwards
        generatedConnections.AddRange(section.GetVertical(true));
        
        generatedConnections.AddRange(section.horizontalConnectionPoints);

        //Check if the generated connections contains the conenction we used
        if (generatedConnections.Contains(connectionUsedToConnect))
        {
            //Remove the connection point that we just generated the section with
            generatedConnections.Remove(connectionUsedToConnect);
        }
        
        //Position section inline with the connection point it's trying to conenct to
        sectionPos = connectionPoint.transform.position;
        //Move the spawned Section with the offset of the connection points it's connecing with
        sectionPos += transform.position - connectionUsedToConnect.gameObject.transform.position;

        spawnedObject.transform.position = sectionPos;

        //Add the generated connections to the to be generated list
        connectionsToConnect.AddRange(generatedConnections);

        //Loop through all the generatedConnections to see if the are the highest
        for (int i = 0; i < generatedConnections.Count; i++)
        {
            CheckIfHighestPoint(generatedConnections[i]);
        }
        

        //Add this part to the generated list
        generatedParts.Add(spawnedObject);

        return spawnedObject;
    }

    List<TowerConnection> GetCorrectSize(List<TowerConnection> connections, int level)
    {
        List<TowerConnection> checkedConnections = new List<TowerConnection>();

        //Loop through each inputed connection point
        foreach (TowerConnection con in connections)
        {
            //if it is the correct level add to the new list to export
            if (con.level == level)
            {
                checkedConnections.Add(con);
            }
        }
        return checkedConnections;
    }

    GameObject CheckIfClear(TowerConnection connection)
    {
        GameObject hitObject = null;

        //Bounds bounds = CalculateBounds(toSpawn);

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
            center.y -= connection.level / 4;
            size.y *= 0.5f;
        }

        //Lets acutally do the raycast
        RaycastHit hit;
        bool isClear;

        isClear = Physics.BoxCast(center, size, direction, out hit, Quaternion.identity, connection.level);

        //If we hit something anwer will be true, but we want to be false
        //If we dont hit something answer will be false, but we want to be true
        
        //Flip the output of isClear
        isClear = !isClear;

        //If we did hit something, We want to know about it
        if (isClear == false)
        {
            //Debug.Log("Hit Something While Generating", hit.transform.gameObject);
            Debug.DrawLine(connection.transform.position,hit.point, Color.red, 0.5f);
            hitObject = hit.transform.gameObject;
        }

        return hitObject;
    }

    //Loop through each child object to find the correct bounds
    //Currently unused will use in the check to clear
    public Bounds CalculateBounds(GameObject boundsParent)
    {
        Bounds bounds = new Bounds();

        bounds.size = Vector3.zero; // reset
        Collider[] colliders = boundsParent.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            bounds.Encapsulate(col.bounds);
        }

        return bounds;
    }

    //Simple time to check if it is the highest point
    void CheckIfHighestPoint(TowerConnection connection)
    {
        if (highestPoint == null)
        {
            highestPoint = connection;
            return;
        }

        if (connection.gameObject.transform.position.y > highestPoint.gameObject.transform.position.y)
        {
            highestPoint = connection;
        }
    }

    [ContextMenu("Reset Tower")]
    void ResetLevel()
    {

        //Loop through each child and destroy them
        //Destroy all children
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        if (!Application.isPlaying)
        {
            children.ForEach(child => DestroyImmediate(child));
        }
        else
        {
            children.ForEach(child => Destroy(child));
        }

        //Reset the connections and generated parts list
        connectionsToConnect = new List<TowerConnection>();
        generatedParts = new List<GameObject>();
    }

    //Spawn an object into the game either a prefab or not depending on the situation
    GameObject SpawnObject(GameObject toSpawn, Transform parent)
    {
        //If we are using the editor we want to spawn a prefab
        //else we want to spawn it as a normal object

        GameObject spawnedObject = null;
        
        //Only if we are using the editor
#if UNITY_EDITOR
        spawnedObject = PrefabUtility.InstantiatePrefab(toSpawn, parent) as GameObject;

#else
         spawnedObject = Instantiate(toSpawn, parent);
#endif

        if (overideLayer && spawnedObject != null)
        {
            MoveToLayer(spawnedObject.transform, layerToSetTowerPieces);
        }

        return spawnedObject;
    }

    //Set the "root" transform and all children to a certain layer
    void MoveToLayer(Transform root, int layer)
    {
        //only change if "root" layer is default
        if (root.gameObject.layer == 0)
            root.gameObject.layer = layer;

        foreach (Transform child in root)
            MoveToLayer(child, layer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Debug.Log(generatedParts.Count);
        if (generatedParts == null || generatedParts.Count <= 0)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawCube(transform.position + (Vector3.up * 0.5f), new Vector3(2, 1, 3));
        }
    }
#endif

    public void UnParentAllPieces()
    {
        for (int i = 0; i < generatedParts.Count; i++)
        {
            generatedParts[i].transform.parent = transform;
        } 
    }
}