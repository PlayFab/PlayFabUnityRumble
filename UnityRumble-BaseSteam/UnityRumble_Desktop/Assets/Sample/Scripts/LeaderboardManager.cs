//--------------------------------------------------------------------------------------
// LeaderboardManager.cs
//
// The Manager class for the Leaderboard Manager and all Leaderboard's base class.
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : ManagerBaseClass<LeaderboardManager>
{
    [SerializeField, Serializable]
    public class LeaderboardObject
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }

    private bool leaderboardReady;

    public bool LeaderboardReady
    {
        get
        {
            return leaderboardReady;
        }
        set
        {
            leaderboardReady = value;
        }
    }

    private int myScore;

    public int MyScore
    {
        get
        {
            return myScore;
        }
        set
        {
            myScore = value;
        }
    }

    private int myRank;

    public int MyRank
    {
        get
        {
            return myRank;
        }
        set
        {
            myRank = value;
        }
    }

    public const string LeaderboardName = "Entry Game Times";

    public List<LeaderboardObject> LeaderboardObjects;

    public event Action OnLeaderboardsDownloadedEvent;

    public event Action<int, int> OnUserDownloadResultEvent;

    public virtual void Initialize()
    {
    }

    public virtual void LeaderboardsDownloaded()
    {
        OnLeaderboardsDownloadedEvent?.Invoke();
    }

    public virtual void GetScore()
    {
    }

    public virtual void OnUserDownloadResult(int score, int rank)
    {
        OnUserDownloadResultEvent?.Invoke(score, rank);
    }

    public virtual void UploadScore(int score)
    {
    }
}
