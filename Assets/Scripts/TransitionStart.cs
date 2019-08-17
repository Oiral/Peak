using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionStart : MonoBehaviour
{
    public bool startingTrigger;

    private void OnTriggerEnter(Collider other)
    {
        //Inside Trigger box
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //The player has left the trigger
            if (Vector3.Dot(transform.forward, (transform.position - other.transform.position)) > 0)
            {
                //Leaving the transition thing
                if (startingTrigger)
                {
                    GetComponentInParent<LevelTransition>().currentState = TransitionState.start;
                }
                else
                {

                    GetComponentInParent<LevelTransition>().currentState = TransitionState.exit;

                }
            }
            
            else
            {
                //Entering the transition thing
                GetComponentInParent<LevelTransition>().currentState = TransitionState.inside;
            }
        }
    }
}
