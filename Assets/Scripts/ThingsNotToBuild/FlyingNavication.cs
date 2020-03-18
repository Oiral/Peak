using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNavication : MonoBehaviour
{
    public float MovementForce = 10000.0f;
    public float LookForce = 10000.0f;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddTorque(Vector3.up * Input.GetAxis("Mouse X") * LookForce);
        GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * Input.GetAxis("Mouse Y") * LookForce *-1);
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * MovementForce);
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * MovementForce);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);
    }
}
