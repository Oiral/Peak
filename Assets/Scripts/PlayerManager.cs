using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Found Extra player. Deleting", gameObject);
            Destroy(gameObject);
        }


    }

    public List<GameObject> physicsBits;

    public void RespawnPlayer()
    {
        Vector3 startingPos = GameObject.FindGameObjectWithTag("Level Start").transform.position;

        foreach (GameObject physicsObject in physicsBits)
        {
            physicsObject.transform.position = startingPos;
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}
