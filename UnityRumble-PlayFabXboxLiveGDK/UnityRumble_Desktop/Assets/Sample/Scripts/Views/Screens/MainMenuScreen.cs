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
using Custom_PlayFab;

public class MainMenuScreen : BaseUiScreen
{
    public event Action FindingGameCompleted;
    public event Action FriendLobbiesObtained;
    public event Action JoinedLobbyCompleted;
    public event Action RequestOpenLeaderboard;
    public event Action Return;

    public Button MatchButton;
    public Button FindLobbiesButton;
    public Button HostButton;

    public Transform LobbiesParent;

    public Transform LobbyItemView;

    public void HandleFindMatchButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleFindMatchButtonPressed()");

        AsyncOpUI.Started(@"Start Matchmaking");

        PlayFabMatchmakingManager.Instance.StartPlayFabMatchmaking();
    }

    public void HandleFindLobbyButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleFindLobbyButtonPressed()");
        Disable();
        // try to find existing lobbies
        PlayFabLobbyManager.Instance.FindLobbyList();
        AsyncOpUI.Started(@"Finding Lobbies......");
    }

    public void HandleHostButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleHostButtonPressed()");
        Disable();
        // try to create a new lobby session
        Networking.OnNetworkJoined += HandleCreateNetworkCompleted;
        Networking.TryCreateNetworkAndGetNetworkId();
        AsyncOpUI.Started(@"Hosting a new game lobby session...");
    }

    public void HandleLeaderboardButtonPressed()
    {
        Debug.LogFormat("MainMenuScreen.HandleLeaderboardButtonPressed()");
        Disable();
        // show leaderboard view
        RequestOpenLeaderboard?.Invoke();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        Assert.IsNotNull(MatchButton);
        Assert.IsNotNull(FindLobbiesButton);
        Assert.IsNotNull(HostButton);
    }

    protected override void OnShown()
    {
        PlayFabLobbyManager.Instance.OnLobbyFindCompletedEvent += HandleFindLobbyCompleted;
        PlayFabLobbyManager.Instance.OnLobbyFindFailureEvent += HandleError;
        PlayFabLobbyManager.Instance.OnCreateAndJoinCompletedEvent += HandleLobbyJoined;
        PlayFabLobbyManager.Instance.OnCreateAndJoinFailureEvent += HandleError;
        PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent += HandleLobbyJoined;
        PlayFabLobbyManager.Instance.OnLobbyJoinFailureEvent += HandleError;
        PlayFabLobbyManager.Instance.OnJoinMatchmakingLobbyCompletedEvent += HandleArrangedLobbyJoined;
        PlayFabLobbyManager.Instance.OnLobbyDisconnectedEvent += HandleOnLobbyDisconnected;
        PlayFabLobbyManager.Instance.OnErrorEvent += HandleLobbyManagerError;
        PlayFabMatchmakingManager.Instance.OnMatchmakingFailed += HandleMatchmakingFailed;
        HandleFindLobbyButtonPressed();
        base.OnShown();

        // return to user startup screen after a network disconnect
        if (Networking.CurrentNetworkState == SessionNetwork.NetworkState.DisconnectedFromNetwork)
        {
            AsyncOpUI.Finished();
            PlayFabSessionNetwork.Instance.InitializeNetwork();
            Return?.Invoke();
        }
    }

    protected override void OnHidden()
    {
        PlayFabLobbyManager.Instance.OnLobbyFindCompletedEvent -= HandleFindLobbyCompleted;
        PlayFabLobbyManager.Instance.OnLobbyFindFailureEvent -= HandleError;
        PlayFabLobbyManager.Instance.OnCreateAndJoinCompletedEvent -= HandleLobbyJoined;
        PlayFabLobbyManager.Instance.OnCreateAndJoinFailureEvent -= HandleError;
        PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent -= HandleLobbyJoined;
        PlayFabLobbyManager.Instance.OnLobbyJoinFailureEvent -= HandleError;
        PlayFabLobbyManager.Instance.OnJoinMatchmakingLobbyCompletedEvent -= HandleArrangedLobbyJoined;
        PlayFabLobbyManager.Instance.OnLobbyDisconnectedEvent -= HandleOnLobbyDisconnected;
        PlayFabLobbyManager.Instance.OnErrorEvent -= HandleLobbyManagerError;
        PlayFabMatchmakingManager.Instance.OnMatchmakingFailed -= HandleMatchmakingFailed;
        base.OnHidden();
    }

    protected override void OnEnabled()
    {
        base.OnEnabled();
        MatchButton.interactable = true;
        FindLobbiesButton.interactable = true;
        HostButton.interactable = true;
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        MatchButton.interactable = false;
        FindLobbiesButton.interactable = false;
        HostButton.interactable = false;
    }

    private void HandleCreateNetworkCompleted(string networkingId)
    {
        AsyncOpUI.Finished();
        AsyncOpUI.Started("Creating Lobby...");
        Networking.OnNetworkStarted -= HandleCreateNetworkCompleted;
        Networking.OnNetworkJoined -= HandleCreateNetworkCompleted;
        PlayFabLobbyManager.Instance.CreateAndJoinLobby(networkingId);
    }

    public void HandleJoinLobby(string connectString)
    {
        AsyncOpUI.Started("Joining Lobby...");
        PlayFabLobbyManager.Instance.JoinLobby(connectString);
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
        FindingGameCompleted?.Invoke();
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

    private void HandleLobbyJoined(string connectString, PlayFab.Multiplayer.Lobby lobby)
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionJoined()");
        AsyncOpUI.Finished();
        Enable();

        JoinedLobbyCompleted?.Invoke();
    }

    private void HandleArrangedLobbyJoined()
    {
        Debug.LogFormat("MainMenuScreen.__HandleArrangedLobbyJoined");
        AsyncOpUI.Finished();

        JoinedLobbyCompleted?.Invoke();
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
        if (!string.IsNullOrEmpty(sessionDocument.NetworkID))
        {
            Networking.OnNetworkStarted += HandleNetworkStarted;
            AsyncOpUI.Started(@"Joining session network...");
        }
    }

    private void HandleLobbyCreated(string connectString)
    {
        Debug.LogFormat("MainMenuScreen.__HandleSessionCreated()");
        AsyncOpUI.Finished();

        AsyncOpUI.Started(@"Starting session network...");
    }

    private void HandleNetworkStarted(string networkId)
    {
        Debug.LogFormat("MainMenuScreen.__HandleNetworkStarted()");
        AsyncOpUI.Finished();
        Networking.OnNetworkStarted -= HandleNetworkStarted;
    }

    public void HandleFindLobbyCompleted(IList<PlayFab.Multiplayer.LobbySearchResult> searchResults)
    {
        for (int i = 0; i < LobbiesParent.childCount; i++)
        {
            Destroy(LobbiesParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < searchResults.Count; i++)
        {
            Transform trn = Instantiate(LobbyItemView, LobbiesParent);
            if (searchResults[i].SearchProperties.ContainsKey(PlayFabLobbyManager.Instance.PFLOBBY_SEARCH_KEY_LOBBY_NAME))
            {
                trn.Find("Name").GetComponent<Text>().text = searchResults[i].SearchProperties[PlayFabLobbyManager.Instance.PFLOBBY_SEARCH_KEY_LOBBY_NAME];
            }
            trn.Find("LobbyPeopleCount").GetComponent<Text>().text = string.Format("(Now){0}/{1}(Max)", searchResults[i].CurrentMemberCount, searchResults[i].MaxMemberCount);
            trn.GetComponent<LobbyItem>().MainMenu = this;
            trn.GetComponent<LobbyItem>().ConnectString = searchResults[i].ConnectionString;
            trn.gameObject.SetActive(true);
        }
        AsyncOpUI.Finished();
        Enable();
    }

    private void HandleOnLobbyDisconnected()
    {
        HandleFindLobbyButtonPressed();
    }

    private void HandleError(string errorMessage)
    {
        AsyncOpUI.Finished();

        Debug.LogFormat(errorMessage);
        StatusBarText.text = string.Format(errorMessage);

        Enable();
    }

    private void HandleLobbyManagerError(int errorCode, string errorMessage)
    {
        AsyncOpUI.Finished();

        string message = string.Format("PlayFabMultiplayer error, code: {0}, message: {1}", errorCode, errorMessage);
        Debug.LogFormat(message);
        StatusBarText.text = message;

        Enable();
    }

    private void HandleMatchmakingFailed(string matchmakingTicket)
    {
        AsyncOpUI.Finished();

        string message = string.Format("Matchmaking failed, MatchmakingTicket: " + matchmakingTicket);
        Debug.LogFormat(message);
        StatusBarText.text = message;

        Enable();
    }
}
