using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopChecker : MonoBehaviour
{
    public MoveGenAndPlayer dropTower;

    public bool reachedTop;

    public void ReachedTop()
    {
        reachedTop = true;
    }

    public void LeftTop()
    {
        if (reachedTop == true)
        {
            dropTower.ResetTower();
            reachedTop = false;
        }
    }
}
