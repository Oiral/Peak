using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPlayer : MonoBehaviour
{
    public bool floatIsActive;

    public Rigidbody bitToFloat;

    public Transform standingPos;

    public float upForce;

    Vector3 posDif;

    private void Update()
    {
        if (floatIsActive)
        {
            posDif = standingPos.position - bitToFloat.transform.position;


            if (posDif.y > 0)
            {
                bitToFloat.AddForce(0, upForce, 0, ForceMode.Acceleration);
            }
        }
    }

}
