using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hitCloudPrefab;

    public float bigHitAmount = 5;
    public float minHitAmount = 2;

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



    public GameObject nearHand;
    public GameObject grabbed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ExternalCam")
        {
            nearHand = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ExternalCam")
        {
            nearHand = null;
        }
    }

    public SteamVR_Action_Boolean grabPinch; //Grab Pinch is the trigger, select from inspecter
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller
                                                                         // Use this for initialization

    void OnEnable()
    {
        if (grabPinch != null)
        {
            grabPinch.AddOnChangeListener(VRController_OnInteract_ButtonPressed, inputSource);
        }
    }


    private void OnDisable()
    {
        if (grabPinch != null)
        {
            grabPinch.RemoveOnChangeListener(VRController_OnInteract_ButtonPressed, inputSource);
        }
    }

    private void VRController_OnInteract_ButtonPressed(SteamVR_Action_Boolean action, SteamVR_Input_Sources sources, bool isConnected)
    {
        if (isConnected)
        {
            Debug.Log("Grab");
            //Grab
            if (nearHand != null)
            {
                grabbed = nearHand;
                grabbed.AddComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
                grabbed.GetComponent<Rigidbody>().drag = 0;
                grabbed.GetComponent<Rigidbody>().drag = 0;
            }
        }
        else
        {
            Debug.Log("Release");
            //Release
            if (grabbed != null)
            {
                Destroy(grabbed.GetComponent<FixedJoint>());
                grabbed.GetComponent<Rigidbody>().drag = 5;
                grabbed.GetComponent<Rigidbody>().angularDrag = 5;
                grabbed = null;
            }
        }
    }
}
