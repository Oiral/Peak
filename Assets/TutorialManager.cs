using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Transform tutorialPoint;
    public GameObject player;

    public bool drawGizmos = true;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Tutorial"))
        {
            //Check what the key is
            if (PlayerPrefs.GetInt("Tutorial") == 1)
            {
                //If the int is 1 - we tutorial is true
                //Run the tutorial
                StartTutorial();
            }
        }
        else
        {
            //If there is not a key
            StartTutorial();
        }
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
        //We want to send the player to the tutorial point
        player.transform.position = tutorialPoint.position;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(tutorialPoint.position + (Vector3.up * 0.5f), Vector3.one);
    }

}

