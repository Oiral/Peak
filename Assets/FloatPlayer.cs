using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FloatPlayer : MonoBehaviour
{
    public bool floatIsActive;

    public Rigidbody bitToFloat;

    public Transform standingPos;
    public Transform playerHead;

    public float hoverForce;
    public float hoverTolerance;
    public float maxVelocity;

    Vector3 posDif;

    public string VFXBoolPropertyName = "Active";

    public VisualEffect effect;

    float normalDrag;

    public GameObject baseVisuals;
    public GameObject baseVisualsParent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeFloatState();
        }

        if (floatIsActive)
        {
            posDif = standingPos.position - bitToFloat.transform.position;

            /*
            if (posDif.y > 0)
            {
                bitToFloat.AddForce(0, hoverForce * posDif.y, 0, ForceMode.Acceleration);
            }
            */

            //Physics.Raycast(bitToFloat.transform.position, Vector3.down, out RaycastHit hit);

            Vector3 modifiedVelocity = bitToFloat.velocity;

            if ((posDif.y > hoverTolerance) && (posDif.y < -hoverTolerance))
            {
                modifiedVelocity.y = 0f;
            }
            else
            {
                modifiedVelocity.y = (posDif.y) * hoverForce;
                modifiedVelocity.y = Mathf.Clamp(modifiedVelocity.y, -maxVelocity, maxVelocity);
            }

            //Debug.Log($"Distance from ground: {hit.distance}, Bike Velocity.y: {modifiedVelocity}");
            bitToFloat.velocity = modifiedVelocity;
        }
    }

    public void ChangeFloatState()
    {
        floatIsActive = !floatIsActive;
        

        if (floatIsActive)
        {
            //Raycast downwards to check if we can spawn the floating base
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(playerHead.transform.position, Vector3.down, out hit, playerHead.transform.localPosition.y + 0.3f))
            {
                normalDrag = bitToFloat.drag;
                //Set its drag to 1
                bitToFloat.drag = 1f;
                baseVisualsParent.transform.position = hit.point;
                baseVisuals.SetActive(true);
            }
            else
            {
                floatIsActive = false;
            }

        }
        else
        {
            //Set its drag back to normal
            bitToFloat.drag = normalDrag;
            baseVisuals.SetActive(false);
        }
        if (effect != null)
        {
            effect.SetBool(VFXBoolPropertyName, floatIsActive);
        }
    }

}
