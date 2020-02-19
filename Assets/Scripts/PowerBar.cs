using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBar : MonoBehaviour
{
    public GameObject bar;

    public void UpdateFuel(float fuelAmount, float fuelMax)
    {
        Vector3 localScale = bar.transform.localScale;

        localScale.y = fuelAmount / fuelMax;

        bar.transform.localScale = localScale;
    }
}
