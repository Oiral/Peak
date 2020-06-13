using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{

    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    public string tagToCheck = "";


    private void OnTriggerEnter(Collider other)
    {
        
        if (tagToCheck != "")
        {
            if (checkForPlayer(other.gameObject, tagToCheck))
            {
                Debug.Log(other.transform.name + " entered the top section", other.transform);
                onTriggerEnter.Invoke();
            }
        }
        else
        {
            Debug.Log(other.transform.name + " entered the top section", other.transform);
            onTriggerEnter.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (tagToCheck != "")
        {
            if (checkForPlayer(other.gameObject, tagToCheck))
            {
                onTriggerExit.Invoke();
            }
        }
        else
        {
            onTriggerExit.Invoke();
        }
    }

    bool checkForPlayer(GameObject toTest, string tag)
    {
        bool test = false;
        if (toTest.CompareTag(tag))
        {
            return true;
        }

        //Return true if parent or this rigibody has the tag player
        if (toTest.GetComponentInParent<Rigidbody>() != null)
        {
            if (toTest.GetComponentInParent<Rigidbody>().gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return test;
    }
}
