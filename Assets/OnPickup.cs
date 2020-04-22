using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnPickup : MonoBehaviour
{
    public UnityEvent OnPickupEvent;

    public void Pickup()
    {
        OnPickupEvent.Invoke();
    }

    public void Unparent()
    {
        transform.parent = null;
    }
}
