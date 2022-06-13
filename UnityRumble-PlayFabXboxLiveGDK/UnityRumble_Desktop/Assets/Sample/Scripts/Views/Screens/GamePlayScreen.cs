//--------------------------------------------------------------------------------------
// GamePlayScreen.cs
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
using Custom_PlayFab;

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
            AsyncOpUI.Started(@"Stopping the session network...");
            if (Networking.CurrentNetworkState == SessionNetwork.NetworkState.DisconnectedFromNetwork)
            {
                AsyncOpUI.Finished();
                PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
                QuitMatchCompleted?.Invoke();
                return;
            }
            PlayFabLobbyManager.Instance.OnLobbyLeaveCompletedEvent += OnLobbyLeaveCompleted;
            PlayFabLobbyManager.Instance.LeaveLobby();
        });
    }

    private void OnLobbyLeaveCompleted()
    {
        PlayFabLobbyManager.Instance.OnLobbyLeaveCompletedEvent -= OnLobbyLeaveCompleted;
        AsyncOpUI.Finished();
        PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
        QuitMatchCompleted?.Invoke();
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
        foreach (var item in PlayFabRuntimeInfos.Instance.GetLobbyPlayerNames())
        {
            GameMembersList.AddMember(item.Key, item.Value, PlayFabRuntimeInfos.Instance.IsPlayerLobbyOwner(item.Key));
        }
    }

    protected override void OnShown()
    {
        BackgroundPanel.gameObject.SetActive(false);

        GameMembersList.ClearAllMembers();

        AddAllSteamMember();
        GameMembersList.MemberReachedMaxKills += HandleMemberReachedMaxKills;
        Networking.OnNetworkMessage_GoodbyeNetwork_Received += OnRemotePlayerQuitLobby;

        TheGameController.gameObject.SetActive(true);
        TheGameController.OnLocalPlayerRequestQuit += HandleLocalPlayerRequestQuit;
        TheGameController.OnShipDestroyed += HandleShipDestroyed;
        TheGameController.SpawnLocalShip(PlayFabRuntimeInfos.Instance.GetMyUlongEntityId());
        TheGameController.SpawnLocalAsteroids(Configuration.MIN_ASTEROID_COUNT, Configuration.MIN_ASTEROID_COUNT);
        Networking.OnError += HandleOnError;
        PlayFabLobbyManager.Instance.OnErrorEvent += HandleLobbyManagerOnError;
        MiniMapContainer.gameObject.SetActive(true);
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
        GameMembersList.MemberReachedMaxKills -= HandleMemberReachedMaxKills;
        Networking.OnNetworkMessage_GoodbyeNetwork_Received += OnRemotePlayerQuitLobby;

        TheGameController.OnLocalPlayerRequestQuit -= HandleLocalPlayerRequestQuit;
        TheGameController.OnShipDestroyed -= HandleShipDestroyed;
        Networking.OnError -= HandleOnError;
        PlayFabLobbyManager.Instance.OnErrorEvent -= HandleLobbyManagerOnError;
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

        TheGameController.SpawnLocalShip(PlayFabRuntimeInfos.Instance.GetMyUlongEntityId());

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
                    PlayFabLobbyManager.Instance.OnLobbyLeaveCompletedEvent += OnLobbyLeaveCompleted;
                    PlayFabLobbyManager.Instance.LeaveLobby();
                    AsyncOpUI.Started(@"Stopping the session network...");
                });
        }
        bool isLocalPlayer = memberXuid == PlayFabRuntimeInfos.Instance.GetMyUlongEntityId();
    }

    private void HandleLocalPlayerRequestQuit()
    {
        Debug.LogFormat("GamePlayScreen.__HandleLocalPlayerRequestQuit()");

        HandleQuitButtonPressed();
    }

    private void HandleShipDestroyed(ulong destroyedShipXuid, ulong destroyerShipXuid)
    {
        Debug.LogFormat("GamePlayScreen.__HandleShipDestroyed(destroyed={0}, destroyer={1})", destroyedShipXuid, destroyerShipXuid);

        var isLocalShipDestroyed = PlayFabRuntimeInfos.Instance.GetMyUlongEntityId() == destroyedShipXuid;

        // update the scoring
        GameMembersList.IncrementMemberDeathCount(destroyedShipXuid);
        GameMembersList.IncrementMemberKillCount(destroyerShipXuid);

        // if the game is over, then we should not respawn
        if (TheGameController.isActiveAndEnabled && isLocalShipDestroyed)
        {
            RespawnLocalPlayerShip();
        }
    }

    private void OnRemotePlayerQuitLobby(ulong playerEntityId)
    {
        Debug.LogFormat("GamePlayScreen.__OnRemotePlayerQuitLobby()");
        GameMembersList.RemoveMember(playerEntityId);
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

    private void HandleOnError(string errorType, int errorCode, string errorMessage)
    {
        Debug.Log("GamePlayScreen.HandleOnError()");
        string message = string.Format("Error Type: {0}\nError Code: {1}\nError Message: {2}", errorType, errorCode, errorMessage);
        PopupView.ShowMessage(this, message, () =>
        {
        });
    }

    private void HandleLobbyManagerOnError(int errorCode, string errorMessage)
    {
        Debug.Log("GamePlayScreen.HandleLobbyManagerOnError()");
        string message = string.Format("PlayFabMultiplayer error\nError Code: {0}\nError Message: {1}", errorCode, errorMessage);
        PopupView.ShowMessage(this, message, () =>
        {
        });
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
    }

    Coroutine _countdownCoroutine;
}
