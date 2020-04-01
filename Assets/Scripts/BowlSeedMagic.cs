using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlSeedMagic : MonoBehaviour
{

    public float GrabForce = 20000f;
    public Transform GrabPoint;

    private Rigidbody Seed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Seed != null)
        {
            Seed.drag = 5;
            Seed.useGravity = false;
            Seed.AddForce(GrabPoint.position - Seed.transform.position * GrabForce);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Rigidbody>().gameObject.tag == "Pickupable")
            if(Seed != null)
            {
                Seed.drag = 0;
                Seed.useGravity = true;
            }
            Seed = other.GetComponentInParent<Rigidbody>();
    }
}

