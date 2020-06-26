using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.Events;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hitCloudPrefab;

    public float GrabStrength = 400;

    public float bigHitAmount = 5;
    public float minHitAmount = 2;

    public GameObject Thumb;
    public GameObject FI;
    public GameObject FM;

    //public GameObject FR;
    //public GameObject FP;
    public Transform Head;

    private GameObject nearHand;
    private GameObject grabbed;
    private FixedJoint grabJoint;

    private bool Gripping = false;


    //public SteamVR_Action Squeeze; //Squeeze is the trigger axes, select from inspecter

    public SteamVR_Action_Boolean grabPinch; //Grab Pinch is the trigger, select from inspecter

    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller
                                                                         // Use this for initialization

    // Grips
    [HideInInspector]
    public UnityEvent gripDown;
    [HideInInspector]
    public UnityEvent gripUp;
    [HideInInspector]
    public UnityEvent gripHold;

    // Top Button
    [HideInInspector]
    public UnityEvent topButtonDown;
    [HideInInspector]
    public UnityEvent topButtonUp;
    [HideInInspector]
    public UnityEvent topButtonHold;

    private void Update()
    {

        //Physics Finger Grab when trigger is pressed
        Thumb.GetComponent<SpringJoint>().spring = SteamVR_Input.GetFloat("Squeeze", inputSource) * 5500 - 50;
        FI.GetComponent<SpringJoint>().spring = SteamVR_Input.GetFloat("Squeeze", inputSource) * 5500 - 50;
        FM.GetComponent<SpringJoint>().spring = SteamVR_Input.GetFloat("Squeeze", inputSource) * 5500 - 50;
        //FR.GetComponent<SpringJoint>().spring = SteamVR_Input.GetFloat("Squeeze", inputSource) * 5500 - 50;
        //FP.GetComponent<SpringJoint>().spring = SteamVR_Input.GetFloat("Squeeze", inputSource) * 5500 - 50;


        // grab phisics object
        if (SteamVR_Input.GetFloat("Squeeze", inputSource) >= 0.5)
        {
            //Grab
            if (nearHand != null)
            {
                grabbed = nearHand;
                grabJoint = grabbed.AddComponent<FixedJoint>() as FixedJoint;
                grabJoint.connectedBody = GetComponent<Rigidbody>();
                grabJoint.breakForce = GrabStrength;
                //if the physics object has the do something on grab
                if (grabbed.GetComponent<OnPickup>())
                {
                    grabbed.GetComponent<OnPickup>().Pickup();
                }

            }
        }
        else
        {
            //Release
            if (grabbed != null)
            {
                Destroy(grabJoint);
                grabbed = null;
            }
        }

        //Grip input for powerups
        if (SteamVR_Input.GetState("GrabGrip", inputSource) == true)
        {
            gripHold.Invoke();

            if (Gripping == false)
            {
                Gripping = true;
                gripDown.Invoke();
            }
        }
        else if (SteamVR_Input.GetState("GrabGrip", inputSource) == false && Gripping == true)
        {
            Gripping = false;
            gripUp.Invoke();
        }
    }



    private void OnTriggerStay(Collider other)
    {

        if (grabbed == null)
        {
            if (other != null)
            {
                if (other.GetComponentInParent<Rigidbody>() != null)
                {
                    var otherRigidbody = other.GetComponentInParent<Rigidbody>();
                    if (otherRigidbody.gameObject.tag == "Pickupable")
                        nearHand = otherRigidbody.gameObject;
                }
            }
        }
        else
        {
            nearHand = null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nearHand = null;
    }



    private void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude > minHitAmount)
        {
            ParticleSystem particle = Instantiate(hitCloudPrefab, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal), null).GetComponent<ParticleSystem>();
            var module = particle.main;
            module.startSizeMultiplier = module.startSizeMultiplier * Mathf.Clamp(collision.relativeVelocity.magnitude / bigHitAmount, 0, 2);
            module.startSpeedMultiplier = module.startSpeedMultiplier * Mathf.Clamp(collision.relativeVelocity.magnitude / bigHitAmount, 0, 2);
        }
    }


    void OnEnable()
    {
        if (grabPinch != null)
        {
            // grabPinch.AddOnChangeListener(VRController_OnInteract_ButtonPressed, inputSource);
        }
    }


    private void OnDisable()
    {
        if (grabPinch != null)
        {
            // grabPinch.RemoveOnChangeListener(VRController_OnInteract_ButtonPressed, inputSource);
        }
    }
}