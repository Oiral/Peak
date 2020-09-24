using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlSeedMagic : MonoBehaviour
{
    public float GrabForce = 10f;
    public float ChuckForce = 1.5f;
    // public Transform GrabPoint;

    private Rigidbody Seed;
   

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Seed != null)
        {
            Seed.drag = 5;
            Seed.useGravity = false;
            Seed.AddForce((this.transform.position - Seed.gameObject.transform.position) * GrabForce);            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Rigidbody>() != null)
        {
            var oterRgedy = other.GetComponentInParent<Rigidbody>();
            if (oterRgedy.gameObject.tag == "Pickupable")
            {
                if (Seed != null)
                {
                    Seed.drag = 0;
                    Seed.useGravity = true;
                    //Seed.AddForce(((GameObject.Find("Player/PlayerRigidbody/HeadCamera").transform.position + new Vector3(0,1,0)) - Seed.gameObject.transform.position) * ChuckForce, ForceMode.Impulse); 
                }
                Seed = oterRgedy;              
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Rigidbody>() != null)
        {
            var oterRgedby = other.GetComponentInParent<Rigidbody>();
            if (oterRgedby == Seed)
            { 
                Seed.drag = 0;
                Seed.useGravity = true;
                Seed = null;                       
            }
        }
    }
}


