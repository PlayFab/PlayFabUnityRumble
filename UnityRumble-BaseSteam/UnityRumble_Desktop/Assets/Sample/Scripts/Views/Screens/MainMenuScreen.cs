//--------------------------------------------------------------------------------------
// MainMenuScreen.cs
//
// The UI class for the main menu screen and functionality.
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

public class MainMenuScreen : BaseUiScreen
{
    public event Action FindingGameCompleted;
    public event Action HostingGameCompleted;
    public event Action JoiningGameCompleted;
    public event Action FriendLobbiesObtained;
    public event Action FindingGameCancel;
    public event Action OpenLeaderBoardScreen;

    public Button MatchButton;
    public Button CancelButton;
    public Button HostButton;
    public Button FindLobbyButton;
    public Button LeaderboardButton;

    public Transform LobbyUiParent;
    public Transform UiLobbyTrn;
    public List<Transform> UiLobbyTrns;

    public void HandleFindMatchButtonPressed()
    {
        Disable();
        SteamLobbyManager.Instance.QuickPlay();
        AsyncOpUI.Started(@"Matchmaking into a new game session...");
    }

    public void HandleFindLobbyButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleFindMatchButtonPressed()");

        GetAllLobbyBtnClick();
        // try to matchmake into a new lobby session
        AsyncOpUI.Started(@"Find All Lobbys...");
    }

    public void HandleCancelMatchButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleCancelMatchButtonPressed()");

        Disable();

        Hide();

        FindingGameCancel?.Invoke();

        // TODO: try to cancel matchmaking into a new lobby session
    }

    public void HandleHostButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleHostButtonPressed()");

        Disable();

        // create a new lobby
        SteamLobbyManager.Instance.CreateLobby(LobbyManager.MAX_LOBBY_MEMBER_COUNT, LobbyManager.LobbyType.Public);

        AsyncOpUI.Started(@"Hosting a new game lobby session...");
    }

    public void HandleJoinButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleJoinButtonPressed()");

        // try to get our social group activities
        AsyncOpUI.Started(@"Looking for in-title friend lobbies...");
    }

    public void HandleJoinInviteButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleJoinInviteButtonPressed()");

        // try to join an invited lobby session
        AsyncOpUI.Started(@"Joining an invited game lobby session...");
    }

    public void HandleCreateLobbyButtonPressed()
    {
        Disable();
        AsyncOpUI.Started(@"Creating Lobby...");
        LobbyManager.Instance.CreateLobby(LobbyManager.MAX_LOBBY_MEMBER_COUNT, LobbyManager.LobbyType.Public);
    }

    public void HandleLeaderboardsButtonPressed()
    {
        Disable();
        OpenLeaderBoardScreen?.Invoke();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Assert.IsNotNull(MatchButton);
        Assert.IsNotNull(CancelButton);
        Assert.IsNotNull(HostButton);
        Assert.IsNotNull(FindLobbyButton);
        Assert.IsNotNull(LeaderboardButton);
    }

    protected override void OnEnabled()
    {
        MatchButton.interactable = true;
        CancelButton.interactable = true;
        HostButton.interactable = true;
        FindLobbyButton.interactable = true;
        LeaderboardButton.interactable = true;
        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        MatchButton.interactable = false;
        CancelButton.interactable = false;
        HostButton.interactable = false;
        FindLobbyButton.interactable = false;
        LeaderboardButton.interactable = false;
        base.OnDisabled();
    }

    protected override void OnShown()
    {
        base.OnShown();
        GetAllLobbyBtnClick();
        LobbyManager.Instance.OnJoinedLobbyAction += CreateLobbyComplete;
        LobbyManager.Instance.OnDisplayLobbyListAction += DisplayLobbyList;
        LobbyManager.Instance.IsInGame = false;
        InventoryManager.Instance.OnRequestInventoryItemsCallBack += HandleInventoryRequest;
        InventoryManager.Instance.GetInventoryItems();
    }

    protected override void OnHidden()
    {
        LobbyManager.Instance.OnJoinedLobbyAction -= CreateLobbyComplete;
        LobbyManager.Instance.OnDisplayLobbyListAction -= DisplayLobbyList;
        InventoryManager.Instance.OnRequestInventoryItemsCallBack -= HandleInventoryRequest;
        base.OnHidden();
    }

    private void HandleFriendActivitiesObtained()
    {
        Debug.LogFormat("MainMenuScreen.__HandleFriendActivitiesObtained()");

        AsyncOpUI.Finished();

        FriendLobbiesObtained?.Invoke();
    }

    private void HandleSessionMatchMade()
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionMatchMade()");

        Disable();

        AsyncOpUI.Finished();
    }

    private void WaitForSessionNetworkId()
    {

    }

    private void HandleSessionMatchMakeCancelled()
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionMatchMakeCancelled()");

        AsyncOpUI.Finished();

        Enable();
    }

    private void HandleInviteReceived()
    {
        Debug.LogFormat("MainMenuScreen.__HandleInviteReceived()");
    }

    private void HandleLobbyJoined()
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionJoined()");

        AsyncOpUI.Finished();

        WaitForSessionNetworkId();
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
        if (!string.IsNullOrEmpty(sessionDocument.NetworkID))
        {
            Networking.OnNetworkStarted += HandleNetworkStarted;
            AsyncOpUI.Started(@"Joining session network...");
        }
    }

    private void HandleLobbyCreated()
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionCreated()");

        AsyncOpUI.Finished();

        Networking.OnNetworkStarted += HandleNetworkStarted;
        AsyncOpUI.Started(@"Starting session network...");
    }

    private void HandleNetworkStarted(string networkId)
    {
        Debug.LogFormat("MainMenuScreen.__HandleNetworkStarted()");

        AsyncOpUI.Finished();
        Networking.OnNetworkStarted -= HandleNetworkStarted;
    }

    private void HandleError(string errorMessage, params object[] args)
    {
        AsyncOpUI.Finished();

        Debug.LogFormat(errorMessage, args);
        StatusBarText.text = string.Format(errorMessage, args);

        Enable();
    }

    private void HandleMatchmakeError(string errorMessage, params object[] args)
    {
        AsyncOpUI.Finished();

        Debug.LogFormat(errorMessage, args);
        StatusBarText.text = string.Format(errorMessage, args);

        Disable();
    }

    private void HandleInventoryRequest(List<string> logs)
    {
        PopupView.ShowDebugLogMessage(this, logs);
    }

    public void CreateLobbyComplete()
    {
        AsyncOpUI.Finished();
        HostingGameCompleted?.Invoke();
    }

    public void GetAllLobbyBtnClick()
    {
        Disable();
        LobbyManager.Instance.RequestLobbyList();
    }

    public void DisplayLobbyList()
    {
        foreach (var item in UiLobbyTrns)
        {
            if (item.gameObject != null)
            {
                Destroy(item.gameObject);
            }
        }
        UiLobbyTrns = new List<Transform>();
        for (int i = 0; i < LobbyManager.Instance.LobbyObjects.Length; i++)
        {
            Transform trn = Instantiate(UiLobbyTrn, LobbyUiParent);
            trn.Find("Name").GetComponent<Text>().text = LobbyManager.Instance.LobbyObjects[i].LobbyName;
            trn.Find("LobbyPeopleCount").GetComponent<Text>().text = string.Format("(Now){0}/{1}(Max)",
                LobbyManager.Instance.LobbyObjects[i].LobbyNowPeople,
                LobbyManager.Instance.LobbyObjects[i].LobbyMaxPeople);

            trn.GetComponent<LobbyItem>().LobbyObject = LobbyManager.Instance.LobbyObjects[i];

            trn.GetComponent<LobbyItem>().MainMenu = this;
            UiLobbyTrns.Add(trn);
            trn.gameObject.SetActive(true);
        }
        Enable();
        AsyncOpUI.Finished();
    }
}
