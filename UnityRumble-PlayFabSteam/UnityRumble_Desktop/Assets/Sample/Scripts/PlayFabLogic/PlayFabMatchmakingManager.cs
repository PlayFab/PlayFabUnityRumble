//--------------------------------------------------------------------------------------
// PlayFabMatchmakingManager.cs
//
// Manage PlayFab Matchmaking.
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
using UnityEngine;
using PlayFab.Multiplayer;
using PlayFab.MultiplayerModels;
using Custom_PlayFab;

public class PlayFabMatchmakingManager
{
    public readonly string PLAYFAB_MATCHMAKING_QUEUE_NAME = "Default";

    // Event triggered when matchmaking failed
    public event Action<string> OnMatchmakingFailed;

    private static PlayFabMatchmakingManager instance;
    public static PlayFabMatchmakingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayFabMatchmakingManager();
                instance.Initialize();
            }

            return instance;
        }
    }

    private PlayFabMatchmakingManager()
    {
    }

    private void Initialize()
    {
        PlayFabMultiplayer.OnMatchmakingTicketStatusChanged += HandleMatchmakingTicketStatusChanged;
    }

    private void HandleMatchmakingTicketStatusChanged(MatchmakingTicket ticket)
    {
        if (ticket.Status == MatchmakingTicketStatus.Matched)
        {
            Debug.Log(@"Matchmaking succeeded");
            var ticketDetail = ticket.GetMatchDetails();
            PlayFabLobbyManager.Instance.JoinMatchmakingLobby(ticketDetail.LobbyArrangementString);
        }
        else if (ticket.Status == MatchmakingTicketStatus.Failed)
        {
            OnMatchmakingFailed?.Invoke(ticket.Status.ToString());
        }
    }

    public void StartPlayFabMatchmaking()
    {
        var userKey = new PFEntityKey(PlayFabRuntimeInfos.Instance.MyEntityContext);
        var attributes = new MatchmakingPlayerAttributes
        {
            // User-Defined Data Types,for example user data and matching rules
            DataObject = new
            {
                UserDefined = "user-defined"
            },
        };
        PlayFabMultiplayer.CreateMatchmakingTicket(new MatchUser(userKey, JsonUtility.ToJson(attributes)), PLAYFAB_MATCHMAKING_QUEUE_NAME);
    }

    public void RemoveEventListeners()
    {
        PlayFabMultiplayer.OnMatchmakingTicketStatusChanged -= HandleMatchmakingTicketStatusChanged;
    }
}
