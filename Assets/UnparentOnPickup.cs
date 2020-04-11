using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentOnPickup : MonoBehaviour
{
    public void Unparent()
    {
        transform.parent = null;
    }
}
