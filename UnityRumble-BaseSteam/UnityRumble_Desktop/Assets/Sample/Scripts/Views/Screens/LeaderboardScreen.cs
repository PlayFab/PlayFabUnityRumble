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
    public LeaderboardItem MyLeaderboardItem;

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
        MyLeaderboardItem.gameObject.SetActive(false);
        LeaderboardManager.Instance.OnLeaderboardsDownloadedEvent -= OnLeaderboardScreenOpen;
        LeaderboardManager.Instance.OnUserDownloadResultEvent -= HandleUserDownloaded;
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
        LeaderboardManager.Instance.OnLeaderboardsDownloadedEvent += OnLeaderboardScreenOpen;
        LeaderboardManager.Instance.OnUserDownloadResultEvent += HandleUserDownloaded;
        LeaderboardManager.Instance.Initialize();
        LeaderboardManager.Instance.GetScore();
    }

    private void HandleUserDownloaded(int score, int rank)
    {
        MyLeaderboardItem.ItemInit(ProfileManager.Instance.GetUserId(), ProfileManager.Instance.GetProfileName(), rank, score);
        MyLeaderboardItem.gameObject.SetActive(true);
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
            leaderboardItem.ItemInit(leaderboardObject.UserId, leaderboardObject.UserName, leaderboardObject.Rank, leaderboardObject.Score);
            trn.gameObject.SetActive(true);
            Leaderboards.Add(trn);
        }
        AsyncOpUI.Finished();
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
        LeaderboardManager.Instance.Initialize();
        LeaderboardManager.Instance.GetScore();
    }
}
