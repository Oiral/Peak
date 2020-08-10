using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerTest : MonoBehaviour
{
    public int numberPosition;

    public LockManager lockMan;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SpinnerMarks>() != null)
        {
            lockMan.UpdateNumber(numberPosition, other.gameObject.GetComponent<SpinnerMarks>().value);
        }
        
    }

    private void Start()
    {
        lockMan.UpdateNumber(numberPosition, 0);
    }
}
