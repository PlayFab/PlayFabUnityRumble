//--------------------------------------------------------------------------------------
// LeaderboardScreen.cs
//
// UI panel for display LeaderboardItem.
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
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LeaderboardScreen : BaseUiScreen
{
    public event Action CloseLeaderBoardScreenEvent;
    public Transform LeaderboardItemTrn;
    public Transform LeaderboardsParent;
    public List<Transform> Leaderboards;
    public ScreenManager ScreenManager;
    public Button CancelButton;

    protected override void OnValidate()
    {
        base.OnValidate();
        Assert.IsNotNull(ScreenManager);
        Assert.IsNotNull(LeaderboardItemTrn);
        Assert.IsNotNull(LeaderboardsParent);
        Assert.IsNotNull(CancelButton);
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnShown()
    {
        base.OnShown();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        GetLeaderboard();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
    }

    protected override void OnHidden()
    {
        base.OnHidden();
    }

    private void WaitForSessionNetworkId()
    {
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
    }

    private void GetLeaderboard()
    {
        AsyncOpUI.Started(@"Geting Leaderboard......");
        LeaderboardManager.Instance.OnLeaderboardsDownloaded += OnLeaderboardScreenOpen;
        LeaderboardManager.Instance.Initialize();
    }

    private void OnLeaderboardScreenOpen()
    {
        for (int i = 0; i < Leaderboards.Count; i++)
        {
            Destroy(Leaderboards[i].gameObject);
        }
        Leaderboards = new List<Transform>();
        List<SteamLeaderboardManager.LeaderboardObject> leaderboardObjects = LeaderboardManager.Instance.LeaderboardObjects;
        for (int i = 0; i < leaderboardObjects.Count; i++)
        {
            SteamLeaderboardManager.LeaderboardObject leaderboardObject = leaderboardObjects[i];
            Transform trn = Instantiate(LeaderboardItemTrn, LeaderboardsParent);
            LeaderboardItem leaderboardItem = trn.GetComponent<LeaderboardItem>();
            leaderboardItem.ItemInit(new Steamworks.CSteamID(leaderboardObject.UserId), leaderboardObject.UserName, leaderboardObject.Rank.ToString(), leaderboardObject.Score.ToString());
            trn.gameObject.SetActive(true);
            Leaderboards.Add(trn);
        }

        AsyncOpUI.Finished();
        LeaderboardManager.Instance.OnLeaderboardsDownloaded -= OnLeaderboardScreenOpen;
    }

    public void HandleCancelButtonPress()
    {
        CloseLeaderBoardScreenEvent?.Invoke();
    }

    public void HandleRefreshButtonPress()
    {
        AsyncOpUI.Started(@"Refreshing Leaderboard......");
        for (int i = 0; i < Leaderboards.Count; i++)
        {
            Destroy(Leaderboards[i].gameObject);
        }
        Leaderboards = new List<Transform>();
        LeaderboardManager.Instance.OnLeaderboardsDownloaded += OnLeaderboardScreenOpen;
        LeaderboardManager.Instance.Initialize();
    }
}
