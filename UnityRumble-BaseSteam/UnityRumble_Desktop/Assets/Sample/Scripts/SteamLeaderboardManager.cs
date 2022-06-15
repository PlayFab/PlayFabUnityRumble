//--------------------------------------------------------------------------------------
// SteamLeaderboardManager.cs
//
// The Manager class for the SteamLeaderboard Manager and functionality.
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

using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamLeaderboardManager : LeaderboardManager
{
    SteamLeaderboard_t SteamLeaderboard_T;
    private CallResult<LeaderboardFindResult_t> OnleanderboardFindResultCallResult;
    private CallResult<LeaderboardScoresDownloaded_t> UserDownloadResult;
    private CallResult<LeaderboardScoresDownloaded_t> DownloadResult;
    private CallResult<LeaderboardScoreUploaded_t> UploadResult;
    private int Entries = 10;
    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamLeaderboardManager();
            Instance.Initialize();
        }
    }

    public override void Initialize()
    {
        OnleanderboardFindResultCallResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
        SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard(LeaderboardName, ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
        OnleanderboardFindResultCallResult.Set(handle, OnLeaderboardFindResult);
        UserDownloadResult = new CallResult<LeaderboardScoresDownloaded_t>();
        DownloadResult = new CallResult<LeaderboardScoresDownloaded_t>();
        UploadResult = new CallResult<LeaderboardScoreUploaded_t>();
        base.Initialize();
    }

    public override void GetScore()
    {
        base.GetScore();
        CSteamID[] userIds = { SteamUser.GetSteamID() };
        SteamAPICall_t steamAPICall_T = SteamUserStats.DownloadLeaderboardEntriesForUsers(SteamLeaderboard_T, userIds, userIds.Length);
        UserDownloadResult.Set(steamAPICall_T, OnUserDownloadResult);
    }

    public void OnUserDownloadResult(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        if (failure)
        {
            Debug.Log("Failed to get score");
            return;
        }
        for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            LeaderboardEntry_t leaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderboardEntry, null, 0);
            base.OnUserDownloadResult(leaderboardEntry.m_nScore, leaderboardEntry.m_nGlobalRank);
        }
    }

    public override void UploadScore(int score)
    {
        base.UploadScore(score);
        if (!LeaderboardReady)
        {
            Debug.Log("Leaderboard not initialized");
            return;
        }
        SteamAPICall_t steamAPICall_T = SteamUserStats.UploadLeaderboardScore(SteamLeaderboard_T, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, score, null, 0);
        Debug.LogFormat("Upload Score is {0}", score);
        UploadResult.Set(steamAPICall_T, OnUploadScoreResult);
    }

    void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        if (failure || pCallback.m_bLeaderboardFound == 0)
        {
            Debug.Log("Leaderboard is not initialized");
            return;
        }
        SteamLeaderboard_T = pCallback.m_hSteamLeaderboard;
        LeaderboardReady = true;
        GetLeaderBoards(ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser);
    }

    void GetLeaderBoards(ELeaderboardDataRequest type = ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal)
    {
        SteamAPICall_t steamAPICall_T;
        switch (type)
        {
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal:
                steamAPICall_T = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboard_T, type, 1, Entries);
                DownloadResult.Set(steamAPICall_T, OnleaderboardDownloadResult);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser:
                steamAPICall_T = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboard_T, type, -(Entries / 2), Entries);
                DownloadResult.Set(steamAPICall_T, OnleaderboardDownloadResult);
                break;
            case ELeaderboardDataRequest.k_ELeaderboardDataRequestFriends:
                steamAPICall_T = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboard_T, type, 1, Entries);
                DownloadResult.Set(steamAPICall_T, OnleaderboardDownloadResult);
                break;
            default:
                break;
        }
    }

    void OnleaderboardDownloadResult(LeaderboardScoresDownloaded_t pCallback, bool failure)
    {
        if (failure)
        {
            Debug.Log("Download leaderboard failed");
            return;
        }
        LeaderboardObjects = new List<LeaderboardObject>();
        int count = SteamUserStats.GetLeaderboardEntryCount(SteamLeaderboard_T);
        for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            LeaderboardEntry_t leaderboardEntry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out leaderboardEntry, null, 0);
            LeaderboardObject leaderboardObject = new LeaderboardObject();
            leaderboardObject.UserId = leaderboardEntry.m_steamIDUser.m_SteamID;
            leaderboardObject.UserName = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);
            leaderboardObject.Rank = (leaderboardEntry.m_nGlobalRank);
            leaderboardObject.Score = leaderboardEntry.m_nScore;
            LeaderboardObjects.Add(leaderboardObject);
        }
        Debug.Log("LeaderboardCount: " + pCallback.m_cEntryCount);
        LeaderboardsDownloaded();
    }

    void OnUploadScoreResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log($"Score upload result : {failure}");
    }
}
