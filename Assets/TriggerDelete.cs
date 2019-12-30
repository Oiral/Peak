using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDelete : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        int loopTimes = 5;

        GameObject checkingObject = other.gameObject;

        for (int i = 0; i < loopTimes; i++)
        {
            if (checkingObject.transform.parent.gameObject.GetComponent<DungeonGenerator>() != null)
            {
                Destroy(checkingObject);
            }
            else
            {
                checkingObject = checkingObject.transform.parent.gameObject;
            }
        }
    }
}
