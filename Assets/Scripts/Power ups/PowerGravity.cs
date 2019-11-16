using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsHand))]
public class PowerGravity : MonoBehaviour
{
    #region Power Stuff
    PhysicsHand hand;

    private void Awake()
    {
        hand = GetComponent<PhysicsHand>();
    }

    private void OnEnable()
    {
        hand.gripDown.AddListener(setLowGrav);
        hand.gripUp.AddListener(setNormalGrav);
    }

    private void OnDisable()
    {
        hand.gripDown.RemoveListener(setLowGrav);
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

    void setNormalGrav()
    {
        Physics.gravity = new Vector3(0, NormalGravity, 0);
    }
}
