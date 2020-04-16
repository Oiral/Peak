using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopChecker : MonoBehaviour
{
    public MoveGenAndPlayer dropTower;

    public bool reachedTop;

    public void ReachedTop()
    {
        if (reachedTop == false && GameObject.FindGameObjectWithTag("Stats") != null)
        {
            //Lets call our stats to update that we hit the top of the tower
            SteamStatsAndAchievements stats = GameObject.FindGameObjectWithTag("Stats").GetComponent<SteamStatsAndAchievements>();
            stats.ClimbTower();
        }

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
