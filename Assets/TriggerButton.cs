using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerButton : MonoBehaviour
{
    public UnityEvent buttonStay;

    public UnityEvent buttonEnter;

    public UnityEvent buttonLeave;

    public List<string> taggedNames;

    private void OnTriggerEnter(Collider other)
    {
        if (taggedNames.Contains(other.tag))
        {
            Debug.Log("Enter this button");
            buttonEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (taggedNames.Contains(other.tag))
        {
            Debug.Log(" this button");
            buttonStay.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (taggedNames.Contains(other.tag))
        {
            Debug.Log("Leave this button");
            buttonLeave.Invoke();
        }
    }
}
