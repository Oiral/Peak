using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSection : MonoBehaviour
{
    public GameObject linkedPrefab;

    public List<GameObject> insideTrigger;

    //We want to keep an eye on how many hands the player has inside the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            insideTrigger.Add(other.gameObject);
        }
    }

    //Remove the hand from the list if it leaves - to check if there are any left
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            insideTrigger.Remove(other.gameObject);
            if (insideTrigger.Count <= 0)
            {
                collectionTimer = 0f;
            }
        }
    }

    float collectionTimer;
    public float collectionTotalTime = 2f;

    private void OnTriggerStay(Collider other)
    {
        //We want to check if this is the first thing on inside trigger as we only want this called once not per every object
        //if it is not, return
        if (other.gameObject != insideTrigger[0])
        {
            return;
        }

        collectionTimer += Time.deltaTime;

        if (collectionTimer > collectionTotalTime)
        {
            //Collect the collectable section
        }
    }
}
