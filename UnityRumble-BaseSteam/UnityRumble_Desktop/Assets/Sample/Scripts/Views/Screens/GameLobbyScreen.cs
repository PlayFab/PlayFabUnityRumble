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
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameLobbyScreen : BaseUiScreen
{
    public event Action LeaveLobbyCompleted;
    public event Action TransitionToGameCompleted;

    public Image ShipButtonImage;
    public Image ColorButtonImage;

    public Button ShipButton;
    public Button ColorButton;
    public Toggle ReadyToggle;
    public Button LeaveButton;
    public Button InviteButton;
    public LobbyMembersView LobbyMembersList;
    public GameController TheGameController;
    public GameAssetManager TheGameAssetManager;
    public LobbyManager LobbyManager;

    public void HandleShipButtonPressed()
    {
        _currentShipImageIndex += 1;
        _currentShipImageIndex %= TheGameAssetManager.PlayerShipSprites.Length;
        RefreshShipAndColor();
        LobbyManager.Instance.MyUserObject.ShipType = _currentShipImageIndex;
        LobbyManager.Instance.SendUserState();
    }

    public void HandleColorButtonPressed()
    {
        _currentShipColorIndex += 1;
        _currentShipColorIndex %= TheGameAssetManager.PlayerShipColorChoices.Length;

        RefreshShipAndColor();
        LobbyManager.Instance.MyUserObject.ShipColor = _currentShipColorIndex;
        LobbyManager.Instance.SendUserState();
    }

    public void HandleReadyTogglePressed()
    {
        LobbyManager.Instance.MyUserObject.IsReady = ReadyToggle.isOn;
        LobbyManager.Instance.SendUserState();
        Debug.LogFormat("GameLobbyScreen.HandleReadyTogglePressed({0})", ReadyToggle.isOn);
    }

    public void HandleLeaveButtonPressed()
    {
        Debug.LogFormat("GameLobbyScreen.HandleLeaveButtonPressed()");
        LeaveLobbyCompleted?.Invoke();
        LobbyManager.Instance.LeaveLobby();
    }

    public void HandleInviteButtonPressed()
    {
        LobbyManager.Instance.ActivateGameOverlay();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        Assert.IsNotNull(ShipButton);
        Assert.IsNotNull(ColorButton);
        Assert.IsNotNull(ReadyToggle);
        Assert.IsNotNull(LeaveButton);
        Assert.IsNotNull(InviteButton);
        Assert.IsNotNull(LobbyMembersList);
        Assert.IsNotNull(TheGameController);
        Assert.IsNotNull(TheGameAssetManager);
    }

    protected override void OnShown()
    {
        KillExistingCountdown();

        ReadyToggle.isOn = false;

        LobbyMembersList.ClearAllMembers();
        LobbyMembersList.AllMembersReady += HandleAllMembersReady;

        LobbyManager.Instance.OnPlayerLobbyStateReceived += HandlePlayerLobbyStateReceived;
        LobbyManager.Instance.OnStartGameMessageAction += HandleStartGame;
        Networking.OnNetworkDisconnected += HandleLeaveButtonPressed;
        ReconcileMemberList();
        RefreshShipAndColor();

        base.OnShown();
    }

    protected override void OnEnabled()
    {
        ShipButton.interactable = true;
        ColorButton.interactable = true;
        ReadyToggle.interactable = true;
        LeaveButton.interactable = true;
        _currentShipImageIndex = 0;
        _currentShipColorIndex = 0;
        RefreshInviteButton();
        DisplayMemberList();
        LobbyManager.Instance.OnUpdateUserStateAction += UpdateMemberList;
        LobbyManager.Instance.OnLobbyMemberChangedAction += DisplayMemberList;
        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        if (ShipButton != null)
        {
            ShipButton.interactable = false;
        }
        if (ShipButton != null)
        {
            ColorButton.interactable = false;
        }
        if (ShipButton != null)
        {
            ReadyToggle.interactable = false;
        }
        if (ShipButton != null)
        {
            LeaveButton.interactable = false;
        }
        if (ShipButton != null)
        {
            InviteButton.interactable = false;
        }
        LobbyManager.Instance.OnUpdateUserStateAction -= UpdateMemberList;
        LobbyManager.Instance.OnLobbyMemberChangedAction -= DisplayMemberList;
    }

    protected override void OnHidden()
    {
        base.OnHidden();

        KillExistingCountdown();

        LobbyMembersList.AllMembersReady -= HandleAllMembersReady;

        LobbyManager.Instance.OnPlayerLobbyStateReceived -= HandlePlayerLobbyStateReceived;
        LobbyManager.Instance.OnStartGameMessageAction -= HandleStartGame;
        Networking.OnNetworkDisconnected -= HandleLeaveButtonPressed;
    }

    private void RefreshInviteButton()
    {
        var hasMaxMembers = LobbyMembersList.HasMaxMembers();
        InviteButton.interactable = IsEnabled() && !hasMaxMembers;
    }

    private void RefreshShipAndColor()
    {
        ShipButtonImage.sprite = TheGameAssetManager.PlayerShipSprites[_currentShipImageIndex];

        ColorButtonImage.color = TheGameAssetManager.PlayerShipColorChoices[_currentShipColorIndex];
    }

    private void ReconcileMemberList()
    {
        ulong[] members = null;

        var currentlyDisplayedMembers = LobbyMembersList.GetMemberXuids();

        foreach (var currentMember in currentlyDisplayedMembers)
        {
            if (!members.Contains(currentMember))
            {
                LobbyMembersList.RemoveMember(currentMember);
            }
        }

        HandleLobbyMembersAdded(members);
    }

    private bool HasGameSessionStarted()
    {
        return true;
    }

    private IEnumerator Countdown()
    {
        var remainingCountdownSeconds = Configuration.COUNTDOWN_SECONDS;

        while (remainingCountdownSeconds > 0.0F || !HasGameSessionStarted())
        {
            remainingCountdownSeconds -= Time.deltaTime;
            remainingCountdownSeconds = Math.Max(0.0F, remainingCountdownSeconds);
            StatusBarText.text = Math.Ceiling(remainingCountdownSeconds).ToString();
            yield return 0;
        }
        LobbyManager.Instance.LobbyMemberList();
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
        Networking.SaveLobbyPlayers();

        KillExistingCountdown();
        _countdownCoroutine = StartCoroutine(Countdown());
    }

    private void ResetReadyState()
    {
        ReadyToggle.isOn = false;
    }

    private void HandleAllMembersReady()
    {
        Debug.LogFormat("GameLobbyScreen.__HandleAllMembersReady()");

        Disable();

        LobbyMembersList.AllMembersReady -= HandleAllMembersReady;
    }

    private void HandleGameStartError(string errorMessage, int hresult)
    {
        ResetReadyState();

        KillExistingCountdown();

        LobbyMembersList.AllMembersReady += HandleAllMembersReady;

        var errorText = string.Format("Error: {0}, hresult = {1}", errorMessage, hresult.ToString("X8"));

        Debug.Log(errorText);
        StatusBarText.text = errorText;

        Enable();
    }

    private void HandleHelloNetworkReceived(ulong senderXuid)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleHelloNetworkReceived() from {0}", senderXuid);
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

    private void HandleStartGame()
    {
        Disable();
        TransitionToGame();
    }

    private void HandleLobbyMembersAdded(ulong[] xuids)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyMembersAdded()");

        RefreshInviteButton();
    }

    private void HandleLobbyMembersRemoved(ulong[] xuids)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyMembersRemoved()");

        RefreshInviteButton();
    }

    private void HandleNetworkStopped(string networkId)
    {
        Debug.LogFormat("GameLobbyScreen.__HandleNetworkStopped()");

        AsyncOpUI.Finished();
        Networking.OnNetworkStopped -= HandleNetworkStopped;

        // try to leave the current lobby session

        AsyncOpUI.Started(@"Leaving the current game lobby session...");
    }

    private void HandleLobbyLeft()
    {
        Debug.LogFormat("GameLobbyScreen.__HandleLobbyLeft()");

        AsyncOpUI.Finished();

        LeaveLobbyCompleted?.Invoke();
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
        RefreshInviteButton();
    }

    Coroutine _countdownCoroutine;
    int _currentShipImageIndex = 0;
    int _currentShipColorIndex = 0;


    public List<Transform> memberTrns = new List<Transform>();

    public Transform memberItemTrn;

    public Transform memberParentTrn;

    public Button startGameButton;

    public void DisplayMemberList()
    {
        startGameButton.gameObject.SetActive(LobbyManager.Instance.IsOwner);
        foreach (var item in memberTrns)
        {
            Destroy(item.gameObject);
        }
        Debug.Log("started display memberlist");
        memberTrns = new List<Transform>();
        foreach (var key in LobbyManager.Instance.LobbyUserObjects.Keys)
        {
            Transform trn = Instantiate(memberItemTrn, memberParentTrn);
            LobbyMemberItem lobbyMemberItem = trn.GetComponent<LobbyMemberItem>();
            LobbyManager.UserObject userObject = LobbyManager.Instance.LobbyUserObjects[key];
            lobbyMemberItem.GameLobbyScreen = this;
            lobbyMemberItem.Show(userObject.UserId, userObject.UserName, userObject.IsReady, userObject.ShipType, userObject.ShipColor);
            memberTrns.Add(trn);
        }
    }

    public void UpdateMemberList(ulong userId)
    {
        Debug.Log("Update Member List.");
        foreach (Transform item in memberTrns)
        {
            LobbyMemberItem lobbyMemberItem = item.GetComponent<LobbyMemberItem>();
            if (lobbyMemberItem.UserId == userId)
            {
                LobbyManager.UserObject userObject = LobbyManager.Instance.LobbyUserObjects[userId];
                lobbyMemberItem.Show(userObject.UserId, userObject.UserName, userObject.IsReady, userObject.ShipType, userObject.ShipColor);
            }
        }
    }

    public void StartGameBtnPress()
    {
        if (LobbyManager.Instance.IsReadyToStart())
        {
            LobbyManager.Instance.SendLobbyMessage(LobbyManager.ChatMessageType.StartGame, "StartGame");
            Disable();
        }
    }

}
