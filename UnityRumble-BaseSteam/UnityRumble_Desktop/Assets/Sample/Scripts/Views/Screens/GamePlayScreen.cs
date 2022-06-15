//--------------------------------------------------------------------------------------
// ScreenManager.cs
//
// The UI screen class for the game play screen functionality.
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

//using PlayFab.Party;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Steamworks;

public class GamePlayScreen : BaseUiScreen
{
    public event Action GameOver;
    public event Action QuitGameCompleted;
    public event Action QuitMatchCompleted;

    public Button QuitButton;

    public GameMembersView GameMembersList;
    public GameController TheGameController;
    public RectTransform MiniMapContainer;
    public RectTransform BackgroundPanel;

    public void HandleQuitButtonPressed()
    {
        Debug.LogFormat("GameLobbyScreen.HandleQuitButtonPressed()");

        // show a confirmation popup
        PopupView.ShowQuitConfirmationMessage(this, () =>
        {
            Disable();
            Networking.OnNetworkStopped += HandleNetworkStopped;
            AsyncOpUI.Started(@"Stopping the session network...");
            Networking.StopNetworking();
            this.Hide();
            QuitMatchCompleted?.Invoke();
        });
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        Assert.IsNotNull(QuitButton);
        Assert.IsNotNull(GameMembersList);
        Assert.IsNotNull(TheGameController);
        Assert.IsNotNull(MiniMapContainer);
    }

    protected void AddAllSteamMember()
    {
        var localSteamId = SteamUser.GetSteamID();
        foreach (var item in LobbyManager.Instance.LobbyUserObjects.Keys)
        {
            GameMembersList.AddMember(LobbyManager.Instance.LobbyUserObjects[item].UserId, LobbyManager.Instance.LobbyUserObjects[item].UserName, (ulong)localSteamId == LobbyManager.Instance.LobbyUserObjects[item].UserId);
        }
    }

    protected override void OnShown()
    {
        BackgroundPanel.gameObject.SetActive(false);

        GameMembersList.ClearAllMembers();
        AddAllSteamMember();
        GameMembersList.MemberReachedMaxKills += HandleMemberReachedMaxKills;
        Networking.OnNetworkStarted += HandleNetworkStarted;
        Networking.StartNetworking(0, string.Empty);

        VoiceChatManager.Instance.StartVoiceRecording();

        ReconcileMemberList();

        TheGameController.gameObject.SetActive(true);
        TheGameController.OnRemotePlayerDisconnected += HandleRemotePlayerDisconnected;
        TheGameController.OnLocalPlayerRequestQuit += HandleLocalPlayerRequestQuit;
        TheGameController.OnShipDestroyed += HandleShipDestroyed;
        TheGameController.SpawnLocalShip(SteamUser.GetSteamID().m_SteamID);
        TheGameController.SpawnLocalAsteroids(Configuration.MIN_ASTEROID_COUNT, Configuration.MIN_ASTEROID_COUNT);
        SessionNetwork.Instance.OnNetworkMessage_GoodbyeNetwork_Received += HandleGoodbyeNetworkReceived;
        MiniMapContainer.gameObject.SetActive(true);
        AchievementManager.Instance.OnStatUploadCompleted += HandleUploadStatCompleted;
        AchievementManager.Instance.SetStat(AchievementManager.Instance.STAT3, 1);
        Networking.OnNetworkDisconnected += HandleNetworkDisconnected;
        LobbyManager.Instance.IsInGame = true;
        base.OnShown();
    }

    protected override void OnEnabled()
    {
        QuitButton.interactable = true;

        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();

        QuitButton.interactable = false;
    }

    protected override void OnHidden()
    {
        base.OnHidden();
        if (MiniMapContainer != null)
            MiniMapContainer.gameObject.SetActive(false);

        AchievementManager.Instance.OnStatUploadCompleted -= HandleUploadStatCompleted;

        VoiceChatManager.Instance.StopVoiceRecording();
        GameMembersList.MemberReachedMaxKills -= HandleMemberReachedMaxKills;
        TheGameController.OnRemotePlayerDisconnected -= HandleRemotePlayerDisconnected;
        TheGameController.OnLocalPlayerRequestQuit -= HandleLocalPlayerRequestQuit;
        TheGameController.OnShipDestroyed -= HandleShipDestroyed;

        SessionNetwork.Instance.OnNetworkMessage_GoodbyeNetwork_Received -= HandleGoodbyeNetworkReceived;
        SessionNetwork.Instance.OnNetworkDisconnected -= HandleNetworkDisconnected;
        TheGameController.Cleanup();
        TheGameController.gameObject.SetActive(false);

        BackgroundPanel.gameObject.SetActive(true);
    }

    private IEnumerator CountdownAndRespawn()
    {
        Debug.LogFormat("GamePlayScreen.__CountdownAndRespawn()");

        var remainingCountdownSeconds = Configuration.COUNTDOWN_SECONDS;

        while (remainingCountdownSeconds > 0.0F)
        {
            remainingCountdownSeconds -= Time.deltaTime;
            remainingCountdownSeconds = Math.Max(0.0F, remainingCountdownSeconds);
            StatusBarText.text = Math.Ceiling(remainingCountdownSeconds).ToString();
            yield return 0;
        }

        Debug.LogFormat("GamePlayScreen.__CountdownAndSpawn() is completed");

        StatusBarText.text = string.Empty;

        TheGameController.SpawnLocalShip(SteamUser.GetSteamID().m_SteamID);

        yield break;
    }

    private void KillExistingCountdown()
    {
        if (null != _countdownCoroutine)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
    }

    private void RespawnLocalPlayerShip()
    {
        KillExistingCountdown();
        _countdownCoroutine = StartCoroutine(CountdownAndRespawn());
    }

    private void ReconcileMemberList()
    {
    }

    private void HandleUploadStatCompleted(List<string> logs)
    {
        PopupView.ShowDebugLogMessage(this, logs);
    }

    private void HandleGoodbyeNetworkReceived(ulong xuid)
    {
        GameMembersList.RemoveMember(xuid);
    }

    private void HandleRemotePlayerDisconnected(ulong xuid)
    {
        GameMembersList.RemoveMember(xuid);
    }

    private void HandleHostChanged()
    {

    }

    private void HandleNetworkDisconnected()
    {
        Networking.StopNetworking();
        QuitMatchCompleted?.Invoke();
    }

    private void HandleMemberReachedMaxKills(ulong memberXuid)
    {
        Debug.LogFormat("GamePlayScreen.__HandleMemberReachedMaxKills({0})", memberXuid);

        var member = GameMembersList.GetMember(memberXuid);
        {
            TheGameController.gameObject.SetActive(false);
            PopupView.ShowGameOverMessage(
                this,
                member.GetGamertag(),
                () =>
                {
                    Disable();
                    Networking.OnNetworkStopped += HandleNetworkStopped;
                    AsyncOpUI.Started(@"Stopping the session network...");
                    Networking.StopNetworking();
                    this.Hide();
                    GameOver?.Invoke();
                });
        }
        bool isLocalPlayer = memberXuid == SteamUser.GetSteamID().m_SteamID;
        if (isLocalPlayer)
        {
            AchievementManager.Instance.SetStat(AchievementManager.Instance.STAT2, 1);
            LeaderboardManager.Instance.UploadScore(500);
        }
    }

    private void HandleLocalPlayerRequestQuit()
    {
        Debug.LogFormat("GamePlayScreen.__HandleLocalPlayerRequestQuit()");

        HandleQuitButtonPressed();
    }

    private void HandleShipDestroyed(ulong destroyedShipXuid, ulong destroyerShipXuid)
    {
        Debug.LogFormat("GamePlayScreen.__HandleShipDestroyed(destroyed={0}, destroyer={1})", destroyedShipXuid, destroyerShipXuid);

        var isLocalShipDestroyed = SteamUser.GetSteamID().m_SteamID == destroyedShipXuid;

        // update the scoring
        GameMembersList.IncrementMemberDeathCount(destroyedShipXuid);
        GameMembersList.IncrementMemberKillCount(destroyerShipXuid);

        // if the game is over, then we should not respawn
        if (TheGameController.isActiveAndEnabled && isLocalShipDestroyed)
        {
            AchievementManager.Instance.SetStat(AchievementManager.Instance.STAT1, 1);
            AchievementManager.Instance.SetAchievement(AchievementManager.Instance.ACHFIRSTDEATH);
            RespawnLocalPlayerShip();
        }
    }

    private void HandleGameMembersAdded(ulong[] xuids)
    {
        Debug.LogFormat("GamePlayScreen.__HandleGameMembersAdded()");
    }

    private void HandleGameMembersRemoved(ulong[] xuids)
    {
        Debug.LogFormat("GamePlayScreen.__HandleLobbyMembersRemoved()");
    }

    private void HandleNetworkStarted(string networkId)
    {
        Debug.LogFormat("GamePlayScreen.__HandleNetworkStarted()");
        AsyncOpUI.Finished();
        Networking.OnNetworkStarted -= HandleNetworkStarted;
    }

    private void HandleNetworkStopped(string networkId)
    {
        Debug.LogFormat("GamePlayScreen.__HandleNetworkStopped()");

        AsyncOpUI.Finished();
        Networking.OnNetworkStopped -= HandleNetworkStopped;

        // try to leave the current lobby session

        AsyncOpUI.Started(@"Quitting the current game play session...");
    }

    private void HandleGameLeft()
    {
        Debug.LogFormat("MainMenuScreen.__HandleGameLeft()");
    }

    private void HandleGameLobbyLeft()
    {
        Debug.LogFormat("MainMenuScreen.__HandleGameLobbyLeft()");

        AsyncOpUI.Finished();

        QuitGameCompleted?.Invoke();
    }

    private void HandleMatchLeft()
    {
        Debug.LogFormat("MainMenuScreen.__HandleMatchLeft()");
    }

    private void HandleMatchLobbyLeft()
    {
        Debug.LogFormat("MainMenuScreen.__HandleMatchLobbyLeft()");

        AsyncOpUI.Finished();

        QuitMatchCompleted?.Invoke();
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
    }

    Coroutine _countdownCoroutine;
}
