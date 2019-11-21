using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHand))]
public class PowerGravity : MonoBehaviour
{
    #region Power Stuff
    PhysicsHand hand;

    PowerBase basePower;

    private void Awake()
    {
        hand = GetComponent<PhysicsHand>();
        basePower = GetComponent<PowerBase>();
    }

    private void OnEnable()
    {
        hand.gripDown.AddListener(setLowGrav);
        hand.gripHold.AddListener(usingGrav);
        hand.gripUp.AddListener(setNormalGrav);
    }

    private void OnDisable()
    {
        hand.gripDown.RemoveListener(setLowGrav);
        hand.gripHold.RemoveListener(usingGrav);
        hand.gripUp.RemoveListener(setNormalGrav);
    }
    #endregion


    float NormalGravity;

    void Start()
    {
        NormalGravity = Physics.gravity.y;
    }

    void setLowGrav()
    {
        Physics.gravity = new Vector3(0, 0.0F, 0);
    }

    void usingGrav()
    {
        //Drain the power usage, If we run out stop the gravity
        if (!basePower.DrainOverTime(10))
        {
            setNormalGrav();
        }
    }

    void setNormalGrav()
    {
        Physics.gravity = new Vector3(0, NormalGravity, 0);
    }
}
