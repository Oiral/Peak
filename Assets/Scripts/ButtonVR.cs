using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public UnityEvent onPress;
    public UnityEvent onHold;
    public UnityEvent onRelease;

    public float distanceForActivation;

    public bool pressed;

    Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, startingPos) > distanceForActivation)
        {
            if (pressed == false)
            {
                onPress.Invoke();
            }
            else
            {
                onHold.Invoke();
            }

            pressed = true;
        }
        else
        {
            if (pressed == true)
            {
                onRelease.Invoke();
            }
            pressed = false;
        }
    }

}
