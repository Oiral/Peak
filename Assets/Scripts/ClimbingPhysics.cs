using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClimbingPhysics : MonoBehaviour
{
    public GameObject LeftHandPhysics;
    public GameObject RightHandPhysics;

    public Transform LeftTrackedHand;
    public Transform RightTrackedHand;

    public float Strength = 1300;
    public float Damnping = 40;

    Vector3 Avp; //Average physics hand position
    Vector3 Avt; //average tracked hand position
    Vector3 lvp; // left hand velocity
    Vector3 rvp; //right hand velocitry
    Vector3 vpA; //average physics hand velocity
    Vector3 hv; //headVelocity
    Vector3 Offset;

    // Use this for initialization
    void Start()
    {
        LeftHandPhysics.transform.position = LeftTrackedHand.position;
        RightHandPhysics.transform.position = RightTrackedHand.position;
        LeftHandPhysics.transform.rotation = LeftTrackedHand.rotation;
        RightHandPhysics.transform.rotation = RightTrackedHand.rotation;
        RightHandPhysics.GetComponent<Rigidbody>().freezeRotation = true;
        LeftHandPhysics.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void FixedUpdate()
    {

        //float angleInDegreesr;
        //Vector3 rotationAxisr;
        //RightTrackedHand.rotation.ToAngleAxis(out angleInDegreesr, out rotationAxisr);

        //Vector3 angularDisplacement = rotationAxisr * angleInDegreesr * Mathf.Deg2Rad;
        //Vector3 angularSpeed = angularDisplacement / Time.deltaTime;

        //float angleInDegreesl;
        //Vector3 rotationAxisl;
        //RightTrackedHand.rotation.ToAngleAxis(out angleInDegreesr, out rotationAxisr);

        //Vector3 angularDisplacement = rotationAxisl * angleInDegreesl * Mathf.Deg2Rad;
        //Vector3 angularSpeed = angularDisplacement / Time.deltaTime;

        //Vector3 LAngular = LeftHandPhysics.transform.TransformVector(LeftTrackedHand.transform.up);
        //LeftHandPhysics.GetComponent<Rigidbody>().AddTorque(LAngular * 800);
        RightHandPhysics.GetComponent<Rigidbody>().MoveRotation(RightTrackedHand.rotation);
        LeftHandPhysics.GetComponent<Rigidbody>().MoveRotation(LeftTrackedHand.rotation);

        //LeftHandPhysics.GetComponent<Rigidbody>().maxAngularVelocity = 0;
        //RightHandPhysics.GetComponent<Rigidbody>().maxAngularVelocity = 0;

        Avt = RightTrackedHand.position - (RightTrackedHand.position - LeftTrackedHand.position) / 2;
        Avp = RightHandPhysics.transform.position - (RightHandPhysics.transform.position - LeftHandPhysics.transform.position) / 2;
        //transform.position -= (Avt - Avp);
        lvp = LeftHandPhysics.GetComponent<Rigidbody>().velocity;
        rvp = RightHandPhysics.GetComponent<Rigidbody>().velocity;
        hv = gameObject.GetComponent<Rigidbody>().velocity;
        vpA = rvp - (rvp - lvp) / 2;

        Vector3 headforce = (Avt - Avp) * -Strength - (hv - vpA) * Damnping;

        gameObject.GetComponent<Rigidbody>().AddForce(headforce);
        Vector3 hvl = LeftTrackedHand.position - LeftHandPhysics.transform.position;
        //hvl = Vector3.ClampMagnitude(hvl, 0.1f);
        Vector3 hvr = RightTrackedHand.position - RightHandPhysics.transform.position;
        //hvr = Vector3.ClampMagnitude(hvr, 0.1f);
        LeftHandPhysics.GetComponent<Rigidbody>().AddForce(hvl * Strength - (lvp - hv) * Damnping);
        RightHandPhysics.GetComponent<Rigidbody>().AddForce(hvr * Strength - (rvp - hv) * Damnping);

        if (transform.position.y < -10)
        {
            //Debug.Log("ghjghj");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = Vector3.zero;
        }

    }
}
