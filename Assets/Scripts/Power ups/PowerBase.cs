using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
    public float maxFuel = 100;
    public float currentFuel;

    public void Charge(float amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public bool DrainOverTime(float multiplier)
    {
        currentFuel -= Time.deltaTime * multiplier;
        if (currentFuel <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Drain(float amount)
    {
        //Only drain if we can actually use the power up otherwise don't drain
        if (currentFuel - amount <= 0)
        {
            return false;
        }
        else
        {
            currentFuel -= amount;
            return true;
        }
    }
}
