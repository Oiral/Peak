using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBase : MonoBehaviour
{
    public float maxFuel = 100;
    public float currentFuel;

    PowerBar powerBar;
    private void Start()
    {
        powerBar = GetComponentInChildren<PowerBar>();
        Debug.Log(powerBar,powerBar.gameObject);
        powerBar.UpdateFuel(currentFuel, maxFuel);
    }

    public void Charge(float amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
        powerBar.UpdateFuel(currentFuel, maxFuel);
    }

    public bool DrainOverTime(float multiplier)
    {
        currentFuel -= Time.deltaTime * multiplier;
        powerBar.UpdateFuel(currentFuel, maxFuel);
        if (currentFuel <= 0)
        {
            currentFuel = 0;
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
            powerBar.UpdateFuel(currentFuel,maxFuel);
            return true;
        }
    }
}
