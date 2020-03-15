﻿using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Steamworks;

public enum AchievementType : int {Climb, Tower ,misc};

// This is a port of StatsAndAchievements.cpp from SpaceWar, the official Steamworks Example.
class SteamStatsAndAchievements : MonoBehaviour
{
    public bool showDebug;

    // Our GameID
    private CGameID m_GameID;

    // Did we get the stats from Steam?
    private bool m_bRequestedStats;
    private bool m_bStatsValid;

    // Should we store stats this frame?
    private bool m_bStoreStats;

    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    void OnEnable()
    {
        if (!SteamManager.Initialized)
            return;

        // Cache the GameID for use in the Callbacks
        m_GameID = new CGameID(SteamUtils.GetAppID());

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

        // These need to be reset to get the stats upon an Assembly reload in the Editor.
        m_bRequestedStats = false;
        m_bStatsValid = false;
    }

    private void Update()
    {
        CheckSteam();
        TrackStats();
    }

    #region STEAM

    private void CheckSteam()
    {
        if (!SteamManager.Initialized)
            return;

        if (!m_bRequestedStats)
        {
            // Is Steam Loaded? if no, can't get stats, done
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }

            // If yes, request our stats
            bool bSuccess = SteamUserStats.RequestCurrentStats();

            // This function should only return false if we weren't logged in, and we already checked that.
            // But handle it being false again anyway, just ask again later.
            m_bRequestedStats = bSuccess;
        }

        if (!m_bStatsValid)
            return;

        // Get info from sources

        // Evaluate achievements
        foreach (Achievement_t achievement in m_Achievements)
        {
            if (achievement.m_bAchieved)
                continue;
            //Make a switch for the type

            /*
            switch (achievement.m_achievementType)
            {
                case AchievementType.Climb:
                    if (m_nTotalNumWins != 0)
                    {
                        UnlockAchievement(achievement);
                    }
                    break;

                case AchievementType.Tower:
                    if (m_nTotalNumWins != 0)
                    {
                        UnlockAchievement(achievement);
                    }
                    break;

                case AchievementType.misc:
                    if (m_nTotalNumWins != 0)
                    {
                        UnlockAchievement(achievement);
                    }
                    break;
            }*/

        }

        //Store stats in the Steam database if necessary
        if (m_bStoreStats)
        {
            // already set any achievements in UnlockAchievement

            // set stats
            SteamUserStats.SetStat("Height", yClimb);
            /*
            // Update average feet / second stat
            SteamUserStats.UpdateAvgRateStat("AverageSpeed", m_flGameFeetTraveled, m_flGameDurationSeconds);
            // The averaged result is calculated for us
            SteamUserStats.GetStat("AverageSpeed", out m_flAverageSpeed);
            */

            bool bSuccess = SteamUserStats.StoreStats();
            // If this failed, we never sent anything to the server, try
            // again later.
            m_bStoreStats = !bSuccess;
        }

        if (showDebug)
        {
            Render();
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Accumulate distance traveled
    //-----------------------------------------------------------------------------
    public void AddDistanceTraveled(float flDistance)
    {
        //m_flGameFeetTraveled += flDistance;
    }
    
    //-----------------------------------------------------------------------------
    // Purpose: Unlock this achievement
    //-----------------------------------------------------------------------------
    private void UnlockAchievement(Achievement_t achievement)
    {
        achievement.m_bAchieved = true;

        // the icon may change once it's unlocked
        //achievement.m_iIconImage = 0;

        // mark it down
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

        // Store stats end of frame
        m_bStoreStats = true;
    }

    //-----------------------------------------------------------------------------
    // Purpose: We have stats data from Steam. It is authoritative, so update
    //			our data with those results now.
    //-----------------------------------------------------------------------------
    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized)
            return;

        // we may get callbacks for other games' stats arriving, ignore them
        Debug.Log(m_GameID);

        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                m_bStatsValid = true;

                // load achievements
                foreach (Achievement_t ach in m_Achievements)
                {
                    bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
                    if (ret)
                    {
                        ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
                        ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
                    }
                    else
                    {
                        Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
                    }
                }

                // load stats
                SteamUserStats.GetStat("Height", out yClimb);
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Our stats data was stored!
    //-----------------------------------------------------------------------------
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                // One or more stats we set broke a constraint. They've been reverted,
                // and we should re-iterate the values now to keep in sync.
                Debug.Log("StoreStats - some failed to validate");
                // Fake up a callback here so that we re-load the values.
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: An achievement was stored
    //-----------------------------------------------------------------------------
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        // We may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Display the user's stats and achievements
    //-----------------------------------------------------------------------------
    public void Render()
    {
        if (!SteamManager.Initialized)
        {
            GUILayout.Label("Steamworks not Initialized");
            return;
        }

        GUILayout.Label("Height Climbed: " + yClimb);
        GUILayout.Label("Horizontal Moved: " + horizontalMovement);
        GUILayout.Label("Max Speed: " + maxSpeed);
        GUILayout.Space(10);

    GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 800));
        foreach (Achievement_t ach in m_Achievements)
        {
            GUILayout.Label(ach.m_eAchievementID.ToString());
            GUILayout.Label(ach.m_strName + " - " + ach.m_strDescription);
            GUILayout.Label("Achieved: " + ach.m_bAchieved);
            GUILayout.Space(20);
        }

        // FOR TESTING PURPOSES ONLY!
        if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS"))
        {
            SteamUserStats.ResetAllStats(true);
            SteamUserStats.RequestCurrentStats();
        }
        GUILayout.EndArea();
    }

    #endregion


    #region AchievementData
    private class Achievement_t
    {
        public Achievement m_eAchievementID;
        public string m_strName;
        public string m_strDescription;
        public bool m_bAchieved;
        public AchievementType m_achievementType;
        public float m_value;

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="type">The type of achievement</param>
        /// <param name="value">Only required if this needs to have a value</param>
        public Achievement_t(Achievement achievementID, string name, string desc, AchievementType type, float value)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_achievementType = type;
            m_value = value;
            m_bAchieved = false;
        }

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="type">The type of achievement</param>
        public Achievement_t(Achievement achievementID, string name, string desc, AchievementType type)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_achievementType = type;
            m_value = 0;
            m_bAchieved = false;
        }

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        public Achievement_t(Achievement achievementID, string name, string desc)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_achievementType = AchievementType.misc;
            m_value = 0;
            m_bAchieved = false;
        }
    }

    private enum Achievement : int
    {
        Height_1,
        Height_2,
        Height_3,
        Height_4,
        Height_5,
        Tower_Climb_1,
        Tower_Climb_2,
        Tower_Climb_3,
        Tower_Climb_4

    };

    //Defining the achievements
    private Achievement_t[] m_Achievements = new Achievement_t[] {
        new Achievement_t(Achievement.Height_1, "Big Ben", "Climbed 96 meters", AchievementType.Climb, 96),
        new Achievement_t(Achievement.Height_2, "Eiffel Tower", "Climbed 324 meters", AchievementType.Climb, 324),
        new Achievement_t(Achievement.Height_3, "Buj Khalifa", "Climbed 828 meters", AchievementType.Climb, 828),
        new Achievement_t(Achievement.Height_4, "Grand Canyon", "Climbed 1857 meters", AchievementType.Climb, 1857),
        new Achievement_t(Achievement.Height_5, "Mount Everest", "Climbed 8848 meters", AchievementType.Climb, 8848),

        new Achievement_t(Achievement.Tower_Climb_1, "What goes up, must come down", "Reached the top of 1 Tower", AchievementType.Tower, 1),
        new Achievement_t(Achievement.Tower_Climb_2, "Hat Trick", "Reached the top of 3 Towers", AchievementType.Tower, 3),
        new Achievement_t(Achievement.Tower_Climb_3, "Bakers Dozen", "Reached the top of 13 Towers", AchievementType.Tower, 13),
        new Achievement_t(Achievement.Tower_Climb_4, "Spire God", "Reached the top of 20 Towers", AchievementType.Tower, 20)
    };

    #endregion

    #region trackStats
    public float yClimb;
    public float horizontalMovement;
    public float maxSpeed;

    public Transform trackingObject;

    private Vector3 prevPos = Vector3.zero;

    bool drawGizmo;
    public float checkDistance = 1f;

    private void TrackStats()
    {
        if (Vector3.Distance(trackingObject.position, prevPos) < checkDistance)
            return;

        Vector3 difference = trackingObject.position - prevPos;

        if (difference.y > 0)
        {
            yClimb += difference.y;
        }

        if (maxSpeed < difference.magnitude)
        {
            maxSpeed = difference.magnitude;
        }

        difference.y = 0;
        horizontalMovement += difference.magnitude;


        prevPos = trackingObject.position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(prevPos, Vector3.one * checkDistance);
        }
    }
    #endregion
}
