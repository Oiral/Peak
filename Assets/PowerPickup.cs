using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPickup : MonoBehaviour
{
    public float amount = 50f;

    float coolDownTimer = 0f;
    public float coolDownTime = 1f;

    private void Update()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                //Turn It back on
                GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (coolDownTimer > 0)
        {
            //If the cool down timer is running, don't do anything
            return;
        }

        if (other.gameObject.GetComponentInChildren<PowerBase>() != null)
        {
            other.gameObject.GetComponentInChildren<PowerBase>().Charge(amount);
            GetComponent<MeshRenderer>().enabled = false;
            coolDownTimer = coolDownTime;
        }
    }

    //-11.137, 154.973, 41.153
}
