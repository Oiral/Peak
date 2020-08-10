using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNavication : MonoBehaviour
{
    public float MovementForce = 10000.0f;
    public float LookForce = 10000.0f;

    public GameObject shootingBallPrefab;

    public float shootForce = 10;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            //Lets spawn a ball to shoot
            Instantiate(shootingBallPrefab, transform.position + (transform.forward / 2), transform.rotation, null).GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        }

    }
}
