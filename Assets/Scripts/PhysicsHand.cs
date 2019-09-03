using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PhysicsHand : MonoBehaviour
{
    public GameObject hitCloudPrefab;

    public float bigHitAmount = 5;
    public float minHitAmount = 2;




    // finger tip control variables:
    //{    
    public bool RightHand;

    public GameObject Thumb;
    public GameObject FI;
    public GameObject FM;
    public GameObject FR;
    public GameObject FP;
        
    private Vector3 ThumbTarget;
    private Vector3 FITarget;
    private Vector3 FMTarget;
    private Vector3 FRTarget;
    private Vector3 FPTarget;    
    //}

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
    //public SteamVR_Action Squeeze; //Squeeze is the trigger axes, select from inspecter

    public SteamVR_Action_Boolean grabPinch; //Grab Pinch is the trigger, select from inspecter
  
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;//which controller
                                                                         // Use this for initialization

// Fingertip control stufff:
//{
    void Start()
    {
        ThumbTarget = Thumb.transform.localPosition;
        FITarget = FI.transform.localPosition;
        FMTarget = FM.transform.localPosition;
        FRTarget = FR.transform.localPosition;
        FPTarget = FP.transform.localPosition;

       
    }

    private void Update()
    {
        
        //Thumb.GetComponent<ConfigurableJoint>().targetPosition = Vector3.Lerp(new Vector3 (0f, 0f, 0f), ThumbTarget, SteamVR_Input.GetFloat("Squeeze", inputSource));
       // FI.GetComponent<ConfigurableJoint>().targetPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), FITarget, SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FM.GetComponent<ConfigurableJoint>().targetPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), FMTarget, SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FR.GetComponent<ConfigurableJoint>().targetPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), FRTarget, SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FP.GetComponent<ConfigurableJoint>().targetPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), FPTarget, SteamVR_Input.GetFloat("Squeeze", inputSource));

        //Thumb.transform.localPosition = Vector3.Lerp(ThumbTarget, new Vector3(0f, 0f, 0f), SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FI.transform.localPosition = Vector3.Lerp(FITarget, new Vector3(0f, 0f, 0f), SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FM.transform.localPosition = Vector3.Lerp(FMTarget, new Vector3(0f, 0f, 0f), SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FR.transform.localPosition = Vector3.Lerp(FRTarget, new Vector3(0f, 0f, 0f), SteamVR_Input.GetFloat("Squeeze", inputSource));
        //FP.transform.localPosition = Vector3.Lerp(FPTarget, new Vector3(0f, 0f, 0f), SteamVR_Input.GetFloat("Squeeze", inputSource));

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
    //}

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
