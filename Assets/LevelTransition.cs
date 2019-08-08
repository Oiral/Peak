using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public LevelEnd levelEnd;

    public GameObject startingMarker;
    public GameObject endMarker;

    public bool assigned = false;

    public int startingLevel;
    public int nextLevel;
    public int endLevelID;

    public bool startLoaded = true;

    public void SetEnd(LevelEnd levelEndObject)
    {
        levelEnd = levelEndObject;

        SetPositionAndRot(levelEnd.transform, startingMarker.transform);

    }

    void SetPositionAndRot(Transform moveTo, Transform internalMarker)
    {
        transform.rotation = moveTo.rotation;

        //Calculate offset between the end and the start marker
        Vector3 offset = moveTo.position - internalMarker.position;

        transform.position += offset;
    }

    public void LoadEndLevel()
    {
        //break if the end level is already loaded
        if (!startLoaded)
        {
            return;
        }

        startLoaded = false;

        StartCoroutine(loadAndSetStartPos());
    }

    public void LoadStartLevel()
    {

    }

    IEnumerator loadAndSetStartPos()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Scene myScene = gameObject.scene;

        SceneManager.SetActiveScene(myScene);

        //SceneManager.MoveGameObjectToScene(player, myScene);

        UnloadAllScenesExcept(myScene);

        yield return 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            yield return 1;
        }

        GameObject startingObject = GameObject.FindGameObjectWithTag("Level Start");

        player = GameObject.FindGameObjectWithTag("Player");

        yield return 0;


        player.transform.SetParent(transform);

        SetPositionAndRot(startingObject.transform, endMarker.transform);

        player.transform.SetParent(null);

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(nextLevel));

        DontDestroyOnLoad(player);
    }
    /*
    public void LoadStartLevel()
    {
        //break if the starting level is already loaded
        if (startLoaded)
        {
            return;
        }

        startLoaded = true;

    }*/

    void UnloadAllScenesExcept(Scene myScene)
    {
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            print(scene.name);
            if (scene != myScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
}
