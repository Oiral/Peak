using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    LevelTransition connectedTransition;

    public int levelLoadNum;

    public int endID;

    private void Awake()
    {
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);
        //StartCoroutine(SetTransition());
    }

    IEnumerator SetTransition()
    {
        yield return 0;
        bool foundTransition = false;
        foreach (LevelTransition levelTran in FindObjectsOfType<LevelTransition>())
        {
            if (levelTran.assigned == false)
            {
                foundTransition = true;
                connectedTransition = levelTran;
                break;
            }
        }
        if (foundTransition == true)
        {
            SetConnnectedTransition();
        }

    }

    void SetConnnectedTransition()
    {
        connectedTransition.assigned = true;
        connectedTransition.startingLevel = SceneManager.GetActiveScene().buildIndex;
        connectedTransition.nextLevel = levelLoadNum;
        connectedTransition.endLevelID = endID;

        //connectedTransition.SetStart(this);
    }
}
