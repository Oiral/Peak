using System.Collections;
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
    public GameObject seedPrefab;
    public float seedLengthMultiplier;
    //[Header("Tower Info")]
    //public int maxSize;
    [Header("Generating")]
    public List<AudioClip> spawnInSounds;
    public float timeInSecToSpawn = 0.1f;

    [HideInInspector]
    public List<TowerConnection> connectionsToConnect;

    private List<GameObject> generatedParts;

    [HideInInspector]
    public TowerConnection highestPoint;
    private GameObject endPoint;

    private List<GameObject> nextSelectablePrefabs;

    [HideInInspector]
    public float towerHeight;

    [Header("Layer")]
    public bool overideLayer = true;
    public int layerToSetTowerPieces;

    public GameObject topTrigger;

    private IEnumerator generatingCoroutine;

    int toGenerate;

    [ContextMenu("Generate Tower")]
    public void GenerateTower()
    {
        //Reset the level before we start to generate it again
        ResetLevel();

        //Get a new seed if needed
        if (randomiseSeed)
        {
            seed = randomGenerator.Next(0, 900000);
        }
        //Create a new random generator with the seed
        //We want this so we can generate the level with a seed
        randomGenerator = new SystemRandom(seed);

        toGenerate = Mathf.RoundToInt(seed.ToString().Length * seedLengthMultiplier);

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
            FinishTowerGen();
            
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
        FinishTowerGen();
    }

    void GenerateBase()
    {
        //Get the largest size to start with
        List<GameObject> startingList = new List<GameObject>();

        //Try spawn in vertical, If there are none, Spawn in horizontal
        if (towerStorage.getLargestSize().vertical.Count > 0)
        {
            startingList = towerStorage.getLargestSize().vertical;
        }
        else if (towerStorage.getLargestSize().horizontal.Count > 0)
        {
            startingList = towerStorage.getLargestSize().horizontal;
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
        int backtracks = 0;
        while (toGenerate > 0)
        {
            //We need to check if something is happening
            //If not drop the togenerate count
            int toGenerateCheck = toGenerate;


            //Check if the genlistmax is greater than 0 as we don't want to remove each section to generate every single time
            //If the current pool of generation is greater than the max

            //Debug.Log(towerStorage.GenListMax);
            //RemoveHalfConnections(towerStorage.GenListMax);

            //If we have not run out of availiable places to place the tower
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                GenerateRandomSection(connectionsToConnect[0], false);
            }

            if (connectionsToConnect.Count <= 0 && toGenerate > 0 && backtracks <= 15)
            {
                Debug.LogWarning("Run out of gen room, back tracking and trying again");
                //If we get here we have run out of space and cannot generate further.
                //We should back track the generation and then try again
                BackTrackGeneration();
                GenerateRandomSection(connectionsToConnect[0], true);
                backtracks += 1;
            }
            if (toGenerateCheck == toGenerate)
            {
                toGenerate -= 1;
            }
        }
    }

    IEnumerator GenerateMiddleSectionSlowly()
    {
        int backtracks = 0;

        while (toGenerate > 0)
        {
            //We need to check if something is happening
            //If not drop the togenerate count
            int toGenerateCheck = toGenerate;


            //Check if the genlistmax is greater than 0 as we don't want to remove each section to generate every single time
            //If the current pool of generation is greater than the max

            //Debug.Log(towerStorage.GenListMax);
            //RemoveHalfConnections(towerStorage.GenListMax);

            //If we have not run out of availiable places to place the tower
            if (connectionsToConnect.Count > 0)
            {
                //Generate a section
                GenerateRandomSection(connectionsToConnect[0], false);
                yield return new WaitForSeconds(timeInSecToSpawn);
            } else if (connectionsToConnect.Count <= 0 && toGenerate > 0 && backtracks >= 15)
            {
                //If we get here we have run out of space and cannot generate further.
                //We should back track the generation and then try again
                Debug.LogWarning("Run out of gen room, back tracking and trying again");
                BackTrackGeneration();
                GenerateRandomSection(connectionsToConnect[0], true);
                backtracks += 1;
                yield return new WaitForSeconds(timeInSecToSpawn);
            }
            if (toGenerateCheck == toGenerate)
            {
                toGenerate -= 1;
            }
        }
    }

    /*
    void RemoveHalfConnections(int maxListSize)
    {
            if (maxListSize > 0 && connectionsToConnect.Count > maxListSize)
            {
                //Remove the first half of the list
                for (int c = 0; c <= towerStorage.GenListMax / 2; c++)
                {
                    connectionsToConnect.RemoveAt(0);
                }
            }
            //Debug.Log(connectionsToConnect.Count);
    }
    */

    void GenerateRandomSection(TowerConnection connectionPoint, bool forceGeneration)
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

        if (forceGeneration == false && isClearGameObject != null)
        {
            //we are not clear

            //Remove the current point from the list to spawn
            connectionsToConnect.Remove(connectionPoint);

            if (connectionsToConnect.Count >= 1)
            {
                //Generate another section
                GenerateRandomSection(connectionsToConnect[0], false);
            }

            //Debug.Log("Collided with something", connectionPoint.gameObject);
            return;
        }

        //Spawn in the selected object
        GameObject spawnedSection = GenerateSection(toSpawn, connectionPoint);

        connectionsToConnect.Remove(connectionPoint);

        /* We are already checking if we have created a dead end tower previously in the generate middle section bit
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
            GenerateRandomSection(connectionPoint, true);

        }
        */
    }

    //We do stuff like set the tower stats and generate the top seciton
    void FinishTowerGen()
    {
        //I think that we should remove the top piece and from there generate the top section as this will mean no matter what even if the top section has no connection points it will work

        //We need to get this highest tower section from the connection point

        Debug.Log("Finished building the tower, Doing final bits now");

        UnParentAllPieces();

        TowerConnection connectionPointToEndOn = highestPoint.GetSection().connectedPiece;

        RemoveSection(highestPoint.GetSection());

        
        sizedSections highestSize = towerStorage.getSize(connectionPointToEndOn.level);

        GenerateEndSection(connectionPointToEndOn, highestSize.horiztonalTop, highestSize.verticalTop);

        //towerHeight = connectionPointToEndOn.transform.position.y;
        GenerateSeed();

        
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
        endPoint = GenerateSection(toSpawn, connectionPoint);
        

        //If its horizontal we want to end on the vertical so create the end section
        if (horizontal)
        {
            GenerateEndSection(connectionsToConnect[0], horiztonalToSpawn, verticalToSpawn);
        }
        else
        {
            towerHeight = endPoint.transform.position.y;
        }
    }

    GameObject GenerateSection(GameObject objectToSpawn, TowerConnection connectionPoint)
    {
        //Spawn in the section
        GameObject spawnedObject = SpawnObject(objectToSpawn, transform);
        toGenerate -= 1;
        //Assign this objects parent to the piece it is generating too
        spawnedObject.transform.SetParent(connectionPoint.gameObject.transform.parent);

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

                if (levelCheckedConnections.Count <= 0)
                {
                    Debug.LogError("This spawned section only has vertical connections but is in horizontal", spawnedObject);
                }
                //Debug.Log("------------------");
                //Debug.Log(levelCheckedConnections.Count);
                

                //If we are connecting horizontally
                connectionUsedToConnect = levelCheckedConnections[randomGenerator.Next(0, levelCheckedConnections.Count /*- 1*/)];

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

        /*
        if (connectionUsedToConnect == null)
        {
            Debug.Log(connectionPoint.type.ToString(), connectionPoint.gameObject);
        }
        */

        //Move the spawned Section with the offset of the connection points it's connecing with
        
        sectionPos += transform.position - connectionUsedToConnect.gameObject.transform.position;

        spawnedObject.transform.position = sectionPos;

        section.connectedPiece = connectionPoint;

        //Add the generated connections to the to be generated list
        connectionsToConnect.AddRange(generatedConnections);

        //Loop through all the generatedConnections to see if the are the highest
        for (int i = 0; i < generatedConnections.Count; i++)
        {
            CheckIfHighestPoint(generatedConnections[i]);
        }
        

        //Add this part to the generated list
        generatedParts.Add(spawnedObject);
        
        if (Application.isPlaying && spawnInSounds.Count > 0)
        {
            AudioSource source = spawnedObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 1;
            source.pitch = Random.Range(0f, 2f);
            Destroy(source, timeInSecToSpawn * 3);
            source.minDistance = 2;
            source.maxDistance = 30;
            source.loop = false;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.clip = spawnInSounds[Random.Range(0, spawnInSounds.Count - 1)];
            source.Play();
        }

        return spawnedObject;
    }

    void BackTrackGeneration()
    {
        //We need to remove the last placed piece and then add it's connection point to the to generate
        //Find the last generated
        TowerSection lastGenerated = generatedParts[generatedParts.Count - 1].GetComponent<TowerSection>();
        connectionsToConnect.Add(lastGenerated.connectedPiece);
        RemoveSection(lastGenerated);
    }

    void GenerateSeed()
    {

        //Find the last generated part (this should have the game object correctly attached
        Transform topper = Helper.FindComponentInChildWithTag<Transform>(endPoint, "Seed Topper");

        if (topper == null)
        {
            Debug.LogError("Found no topper and the top of the tower", gameObject);
            return;
        }

        if (topTrigger != null)
        {
            topTrigger.transform.position = topper.position;
        }


        GameObject spawnedSeed = Instantiate(seedPrefab, topper.position, Quaternion.identity, endPoint.transform);
        Seed seedComp = spawnedSeed.GetComponent<Seed>();
        seedComp.towerSeed = seed;
        seedComp.storage = towerStorage;
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

        spawnedObject.transform.localPosition = Vector3.zero;
        spawnedObject.transform.localRotation = Quaternion.identity;
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

    void RemoveSection(TowerSection section)
    {
        //Now we need to remove any sections from our generation lists
        foreach (TowerSection childSections in section.gameObject.GetComponentsInChildren<TowerSection>())
        {
            //When removing a section, we want to add 1 more to the generation per each connection piece we are removing.
            //Easy way to check is find all TowerSections scripts in child
            toGenerate += 1;
            if (generatedParts.Contains(childSections.gameObject))
            {
                generatedParts.Remove(childSections.gameObject);
            }

        }

        foreach (TowerConnection connectionPointToRemove in section.gameObject.GetComponentsInChildren<TowerConnection>())
        {
            if (connectionsToConnect.Contains(connectionPointToRemove))
            {
                connectionsToConnect.Remove(connectionPointToRemove);
            }
        }
        //We need to remove this section from the generated list
        if (generatedParts.Contains(section.gameObject))
        {
            generatedParts.Remove(section.gameObject);
        }

        DestroyWithCheck(section.gameObject);
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
            if (generatedParts[i] != null)
            {
                generatedParts[i].transform.parent = transform;
            }
            
        } 
    }

    /// <summary>
    /// Only call this is the game is running not in the editor
    /// </summary>
    public void StopGeneration()
    {
        StopAllCoroutines();
    }

    void DestroyWithCheck(GameObject objectToDestroy)
    {
        if (!Application.isPlaying)
        {
            DestroyImmediate(objectToDestroy);
        }
        else
        {
            Destroy(objectToDestroy);
        }
    }

    
}