//--------------------------------------------------------------------------------------
// SteamAchievementManager.cs
//
// The Manager class for Steam Achievement.
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// 
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------
using Steamworks;
using UnityEngine;

public class SteamAchievementManager : AchievementManager
{
    private Callback<UserAchievementIconFetched_t> OnUserAchievementFetchedCallback;

    private Callback<UserAchievementStored_t> OnUserAchievementStoredCallback;

    private Callback<UserStatsUnloaded_t> OnUserStatsUnloadedCallback;

    private Callback<UserStatsReceived_t> OnUserStatsReceivedCallback;

    private Callback<UserStatsStored_t> OnUserStatsStoredCallback;

    private CallResult<UserStatsReceived_t> OnUserStatsReceivedCallResult;

    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamAchievementManager();
            Instance.Initialize();
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        if (!SteamAPI.Init())
        {
            Debug.Log("Steam is not initialized");
            return;
        }
        OnUserAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);

        OnUserAchievementFetchedCallback = Callback<UserAchievementIconFetched_t>.Create(OnUserAchievementIconFetched);

        OnUserStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);

        OnUserStatsStoredCallback = Callback<UserStatsStored_t>.Create(OnUserStatsStored);

        OnUserStatsUnloadedCallback = Callback<UserStatsUnloaded_t>.Create(OnUserStatsUnloaded);

        OnUserStatsReceivedCallResult = CallResult<UserStatsReceived_t>.Create(OnUserStatsReceived);

        SteamUserStats.RequestCurrentStats();

        RequestAllAchievement();
    }

    public override void RequestAllAchievement()
    {
        int achievementLength = (int)SteamUserStats.GetNumAchievements();
        Debug.Log("AchievementLength:----" + achievementLength);
        AchievementNames = new string[achievementLength];
        for (int i = 0; i < achievementLength; i++)
        {
            AchievementNames[i] = SteamUserStats.GetAchievementName((uint)i);
            Debug.LogFormat("Achievement[{0}]: {1}", i, AchievementNames[i]);
        }
        base.RequestAllAchievement();
    }

    public override void SetAchievement(string achievementName)
    {
        if (GetAchievement(achievementName))
        {
            Debug.LogFormat("Achievement: {0} is already", achievementName);
            return;
        }

        bool ret = SteamUserStats.SetAchievement(achievementName);
        Debug.LogFormat("SetAchievement: {0} --- {1}", achievementName, ret);
        if (ret)
        {
            SteamUserStats.StoreStats();
        }
        else
        {
            Debug.Log("Error:Set Achievement is failed!");
        }

        base.SetAchievement(achievementName);
    }

    public override bool GetAchievement(string achievementName)
    {
        bool ret;
        SteamUserStats.GetAchievement(achievementName, out ret);
        Debug.Log($"{achievementName} ---- {ret}");
        return ret;
    }

    // Clear achievement and set stat example 
    public void ClearAchievement()
    {
        for (int i = 0; i < AchievementNames.Length; i++)
        {
            bool ret = SteamUserStats.ClearAchievement(AchievementNames[i]);
            Debug.Log("SteamUserStats.ClearAchievement(" + AchievementNames[i] + ") : " + ret);
        }
        SteamUserStats.SetStat(STAT1, 0);
        SteamUserStats.SetStat(STAT2, 0);
        SteamUserStats.SetStat(STAT3, 0);
        SteamUserStats.StoreStats();
    }

    private void OnUserAchievementStored(UserAchievementStored_t callBack)
    {
        Debug.Log("[" + UserAchievementStored_t.k_iCallback + " - UserAchievementStored] - " + callBack.m_nGameID + " -- " + callBack.m_bGroupAchievement + " -- " + callBack.m_rgchAchievementName + " -- " + callBack.m_nCurProgress + " -- " + callBack.m_nMaxProgress);
    }

    private void OnUserAchievementIconFetched(UserAchievementIconFetched_t callBack)
    {
    }

    private void OnUserStatsReceived(UserStatsReceived_t callBack)
    {
        Debug.Log("[" + UserStatsReceived_t.k_iCallback + " - UserStatsReceived] - " + callBack.m_nGameID + " -- " + callBack.m_eResult + " -- " + callBack.m_steamIDUser);
    }

    private void OnUserStatsReceived(UserStatsReceived_t callBack, bool bIOFailure)
    {
        Debug.Log("[" + UserStatsReceived_t.k_iCallback + " - UserStatsReceived] - " + callBack.m_nGameID + " -- " + callBack.m_eResult + " -- " + callBack.m_steamIDUser);
    }

    private void OnUserStatsUnloaded(UserStatsUnloaded_t callBack)
    {
        Debug.Log("[" + UserStatsUnloaded_t.k_iCallback + " - UserStatsUnloaded] - " + callBack.m_steamIDUser);
    }

    private void OnUserStatsStored(UserStatsStored_t callBack)
    {
        Debug.Log("[" + UserStatsStored_t.k_iCallback + " - UserStatsStored] - " + callBack.m_nGameID + " -- " + callBack.m_eResult);
    }

    public override int GetStat(string statName)
    {
        int myStat;
        bool ret = SteamUserStats.GetStat(statName, out myStat);
        if (ret)
        {
            Debug.Log(statName + ": " + myStat);
        }
        else
        {
            Debug.Log("Error:My stat get failed!");
        }
        return myStat;
    }

    public override void SetStat(string statName, int addStatNumber)
    {
        base.SetStat(statName, addStatNumber);
        int myStat = GetStat(statName);
        bool ret = SteamUserStats.SetStat(statName, myStat + addStatNumber);
        if (ret)
        {
            Debug.Log(statName + ": " + (myStat + addStatNumber));
            SteamUserStats.StoreStats();
        }
        else
        {
            Debug.Log("Error:My stat set failed!");
        }
    }
}
