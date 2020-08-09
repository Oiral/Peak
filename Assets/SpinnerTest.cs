using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<SpinnerMarks>() != null)
        {
            Debug.Log(other.gameObject.GetComponent<SpinnerMarks>().value, other.gameObject);
            
        }
        
    }
}
