using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNavication : MonoBehaviour
{
    public float MovementForce = 10000.0f;
    public float LookForce = 10000.0f;



    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(Vector3.up * Input.GetAxis("Mouse X") * LookForce);
        rb.AddRelativeTorque(Vector3.right * Input.GetAxis("Mouse Y") * LookForce *-1);
        rb.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * MovementForce);
        rb.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * MovementForce);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0.0f);

        if (Input.GetKey(KeyCode.E))
        {
            rb.AddForce(Vector3.up * MovementForce);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(-Vector3.up * MovementForce);
        }

    }
}
