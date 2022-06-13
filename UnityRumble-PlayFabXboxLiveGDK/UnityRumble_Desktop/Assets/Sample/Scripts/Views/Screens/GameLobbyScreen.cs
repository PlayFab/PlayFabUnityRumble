//--------------------------------------------------------------------------------------
// GameLobbyScreen.cs
//
// The UI class for the game lobby screen and functionality.
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Custom_PlayFab;

public class GameLobbyScreen : BaseUiScreen
{
    public event Action LeaveLobbyCompleted;
    public event Action TransitionToGameCompleted;

    public Image ShipButtonImage;
    public Image ColorButtonImage;
    public Button StartGameButton;
    public Button ShipButton;
    public Button ColorButton;
    public Toggle ReadyToggle;
    public Button LeaveButton;
    public LobbyMembersView LobbyMembersList;
    public GameController TheGameController;
    public GameAssetManager TheGameAssetManager;

    public void HandleShipButtonPressed()
    {
        _currentShipImageIndex += 1;
        _currentShipImageIndex %= TheGameAssetManager.PlayerShipSprites.Length;
        RefreshShipAndColor();
    }

    public void HandleColorButtonPressed()
    {
        _currentShipColorIndex += 1;
        _currentShipColorIndex %= TheGameAssetManager.PlayerShipColorChoices.Length;
        RefreshShipAndColor();
    }

    public void HandleReadyTogglePressed()
    {
        RefreshShipAndColor();
        Debug.LogFormat("GameLobbyScreen.HandleReadyTogglePressed({0})", ReadyToggle.isOn);
    }

    public void HandleLeaveButtonPressed()
    {
        Debug.LogFormat("GameLobbyScreen.HandleLeaveButtonPressed()");
        Disable();
        AsyncOpUI.Started("Leaving the lobby...");
        if (Networking.CurrentNetworkState == SessionNetwork.NetworkState.DisconnectedFromNetwork)
        {
            OnLobbyLeaveCompleted();
            return;
        }
        PlayFabLobbyManager.Instance.LeaveLobby();
    }

    public void HandleInviteButtonPressed()
    {
    }

    public void HandleStartButtonPressed()
    {
        var uerId = PlayFabRuntimeInfos.Instance.GetMyUlongEntityId();
        var startGameState = new StartGameState(uerId);
        HandleStartGame(uerId, startGameState);
        Networking.SendMessageToAll(startGameState);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Assert.IsNotNull(StartGameButton);
        Assert.IsNotNull(ShipButton);
        Assert.IsNotNull(ColorButton);
        Assert.IsNotNull(ReadyToggle);
        Assert.IsNotNull(LeaveButton);
        Assert.IsNotNull(LobbyMembersList);
        Assert.IsNotNull(TheGameController);
        Assert.IsNotNull(TheGameAssetManager);
    }
    protected override void OnShown()
    {

        KillExistingCountdown();
        _currentShipImageIndex = 0;
        _currentShipColorIndex = 0;
        ReadyToggle.isOn = false;
        LobbyMembersList.ClearAllMembers();
        LobbyMembersList.AllMembersReady += HandleAllMembersReady;
        Networking.OnNetworkMessage_StartGameState_Received += HandleStartGame;
        Networking.OnNetworkMessage_PlayerLobbyState_Received += HandlePlayerLobbyStateReceived;
        PlayFabRuntimeInfos.Instance.PlayerInfoUpdatedEvent += ReconcileMemberList;
        PlayFabLobbyManager.Instance.AddSelfToLobbyEvent += ReconcileMemberList;
        PlayFabLobbyManager.Instance.OnLobbyLeaveCompletedEvent += OnLobbyLeaveCompleted;
        PlayFabLobbyManager.Instance.OnLobbyMemberRemoveEvent += HandleLobbyMembersRemoved;
        Networking.OnError += HandleOnError;
        PlayFabLobbyManager.Instance.OnErrorEvent += HandleLobbyManagerOnError;
        base.OnShown();
    }

    protected override void OnEnabled()
    {
        StartGameButton.interactable = true;
        ShipButton.interactable = true;
        ColorButton.interactable = true;
        ReadyToggle.interactable = true;
        LeaveButton.interactable = true;
        RefreshStartGameButton();
        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        StartGameButton.interactable = false;
        ShipButton.interactable = false;
        ColorButton.interactable = false;
        ReadyToggle.interactable = false;
        LeaveButton.interactable = false;
    }

    protected override void OnHidden()
    {
        base.OnHidden();
        KillExistingCountdown();
        LobbyMembersList.AllMembersReady -= HandleAllMembersReady;
        Networking.OnNetworkMessage_StartGameState_Received -= HandleStartGame;
        Networking.OnNetworkMessage_PlayerLobbyState_Received -= HandlePlayerLobbyStateReceived;
        PlayFabRuntimeInfos.Instance.PlayerInfoUpdatedEvent -= ReconcileMemberList;
        PlayFabLobbyManager.Instance.AddSelfToLobbyEvent -= ReconcileMemberList;
        PlayFabLobbyManager.Instance.OnLobbyLeaveCompletedEvent -= OnLobbyLeaveCompleted;
        PlayFabLobbyManager.Instance.OnLobbyMemberRemoveEvent -= HandleLobbyMembersRemoved;
        Networking.OnError -= HandleOnError;
        PlayFabLobbyManager.Instance.OnErrorEvent -= HandleLobbyManagerOnError;
        LobbyMembersList.ClearAllMembers();
    }

    private void RefreshStartGameButton()
    {
        StartGameButton.gameObject.SetActive(PlayFabRuntimeInfos.Instance.IsSelfLobbyOwner);
    }

    private void RefreshMemberHost()
    {
        var currentlyDisplayedMembers = LobbyMembersList.GetMemberXuids();
        foreach (var item in currentlyDisplayedMembers)
        {
            if (PlayFabRuntimeInfos.Instance.IsPlayerLobbyOwner(item))
            {
                LobbyMembersList.UpdateMemberHost(item, true);
            }
        }
    }

    private void RefreshShipAndColor()
    {
        ShipButtonImage.sprite = TheGameAssetManager.PlayerShipSprites[_currentShipImageIndex];
        ColorButtonImage.color = TheGameAssetManager.PlayerShipColorChoices[_currentShipColorIndex];
        PlayerLobbyState playerLobbyState = new PlayerLobbyState(ReadyToggle.isOn, _currentShipImageIndex, _currentShipColorIndex);
        Networking.SendMessageToAll(playerLobbyState);
        HandlePlayerLobbyStateReceived(PlayFabRuntimeInfos.Instance.GetMyUlongEntityId(), playerLobbyState);
    }

    private void ReconcileMemberList()
    {
        var memberInfos = PlayFabRuntimeInfos.Instance.GetLobbyPlayerNames();

        var currentlyDisplayedMembers = LobbyMembersList.GetMemberXuids();
        foreach (var currentMember in currentlyDisplayedMembers)
        {
            if (!memberInfos.ContainsKey(currentMember))
            {
                LobbyMembersList.RemoveMember(currentMember);
            }
        }

        HandleLobbyMembersAdded(memberInfos);
    }

    private bool HasGameSessionStarted()
    {
        return true;
    }

    private IEnumerator Countdown()
    {
        Disable();
        var remainingCountdownSeconds = Configuration.COUNTDOWN_SECONDS;

        while (remainingCountdownSeconds > 0.0F || !HasGameSessionStarted())
        {
            remainingCountdownSeconds -= Time.deltaTime;
            remainingCountdownSeconds = Math.Max(0.0F, remainingCountdownSeconds);
            StatusBarText.text = Math.Ceiling(remainingCountdownSeconds).ToString();
            yield return 0;
        }

        StatusBarText.text = string.Empty;
        TransitionToGameCompleted?.Invoke();

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

    private void TransitionToGame()
    {
        // we initialize the game controller early since players may get into
        // the game well ahead of us...

        TheGameController.Initialize(_currentShipImageIndex, _currentShipColorIndex);

        // set lobby access to private
        PlayFabLobbyManager.Instance.SetLobbyAccessToPrivate();

        KillExistingCountdown();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    private void ResetReadyState()
    {
        ReadyToggle.isOn = false;
    }

    private void HandleAllMembersReady(bool isAllReady)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleAllMembersReady()");

        StartGameButton.interactable = isAllReady;
    }

    private void HandleGameStartError(string errorMessage, int hresult)
    {
        ResetReadyState();

        KillExistingCountdown();

        var errorText = string.Format("Error: {0}, hresult = {1}", errorMessage, hresult.ToString("X8"));

        Debug.Log(errorText);

        StatusBarText.text = errorText;

        Enable();
    }

    private void HandlePlayerLobbyStateReceived(ulong senderXuid, PlayerLobbyState playerLobbyState)
    {
        Debug.LogFormat("GameLobbyScreen.__HandlePlayerLobbyStateReceived() from {0}", senderXuid);

        if (playerLobbyState.IsReady)
        {
            LobbyMembersList.MakeMemberReady(senderXuid);
        }
        else
        {
            LobbyMembersList.MakeMemberNotReady(senderXuid);
        }

        LobbyMembersList.SetMemberShipIndex(senderXuid, TheGameAssetManager, playerLobbyState.ShipIndex);
        LobbyMembersList.SetMemberColorIndex(senderXuid, TheGameAssetManager, playerLobbyState.ColorIndex);
    }

    private void HandleLobbyStateReceived(ulong senderXuid, LobbyState lobbyState)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyStateReceived()");

        // if the lobby is ready, proceed to the game start countdown!
        if (lobbyState.IsReady)
        {
            TransitionToGame();
        }
    }

    private void HandleStartGame(ulong xuid, StartGameState startGameState)
    {
        TransitionToGame();
    }

    private void HandleLobbyMembersAdded(Dictionary<ulong, string> memberNameMap)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyMembersAdded()");
        foreach (var item in memberNameMap)
        {
            if (!LobbyMembersList.HasMember(item.Key))
            {
                Debug.LogFormat("LobbyMembersAdded({0})", item.Value);
                LobbyMembersList.AddMember(item.Key, item.Value, PlayFabRuntimeInfos.Instance.IsPlayerLobbyOwner(item.Key));
            }
        }
        RefreshShipAndColor();
        RefreshStartGameButton();
    }

    private void HandleLobbyMembersRemoved(ulong xuid)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyMembersRemoved({0})", xuid);
        LobbyMembersList.RemoveMember(xuid);
        RefreshMemberHost();
        RefreshStartGameButton();
    }

    private void HandleNetworkStopped(string networkId)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleNetworkStopped()");

        AsyncOpUI.Finished();
        Networking.OnNetworkStopped -= HandleNetworkStopped;

        // try to leave the current lobby session

        AsyncOpUI.Started(@"Leaving the current game lobby session...");
    }

    private void OnLobbyLeaveCompleted()
    {
        LeaveLobbyCompleted?.Invoke();
    }

    private void HandleOnError(string errorType, int errorCode, string errorMessage)
    {
        Debug.Log("GameLobbyScreen.HandleOnError()");
        string message = string.Format("Error Type: {0}\nError Code: {1}\nError Message: {2}", errorType, errorCode, errorMessage);
        PopupView.ShowMessage(this, message, () =>
        {
        });
    }

    private void HandleLobbyManagerOnError(int errorCode, string errorMessage)
    {
        Debug.Log("GameLobbyScreen.HandleLobbyManagerOnError()");
        string message = string.Format("PlayFabMultiplayer error\nError Code: {0}\nError Message: {1}", errorCode, errorMessage);
        PopupView.ShowMessage(this, message, () =>
        {
        });
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
        RefreshStartGameButton();
    }

    Coroutine _countdownCoroutine;
    int _currentShipImageIndex = 0;
    int _currentShipColorIndex = 0;

    private void OnApplicationQuit()
    {
        PlayFabLobbyManager.Instance.LeaveLobbyOnApplicationQuit();
        Networking.StopNetworking();
    }
}
