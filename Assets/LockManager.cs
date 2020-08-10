using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public Dictionary<int, int> trackedNumber = new Dictionary<int, int>();

    public int number;

    public TextMesh text;

    public void GetNumber()
    {
        number = 0;

        foreach (KeyValuePair<int, int> entry in trackedNumber)
        {
            // do something with entry.Value or entry.Key
            number += Mathf.RoundToInt(Mathf.Pow(10, entry.Key) * entry.Value);
        }
    }

    public void UpdateNumber(int position, int value)
    {
        int clampedValue = Mathf.Clamp(value, 0, 9);

        if (trackedNumber.ContainsKey(position))
        {
            trackedNumber[position] = clampedValue;
        }
        else
        {
            trackedNumber.Add(position, clampedValue);
        }
         
    }

    private void Update()
    {
        GetNumber();
        text.text = number.ToString();
    }

}
