//--------------------------------------------------------------------------------------
// SteamLobbyManager.cs
//
// The Manager class for Steam Lobby Manager and functionality.
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
using UnityEngine;
using System.Collections.Generic;
using Steamworks;

public class SteamLobbyManager : LobbyManager
{
    public CallResult<LobbyEnter_t> OnJoinLobbyCallResult = new CallResult<LobbyEnter_t>();//join lobby Callback

    public CallResult<LobbyCreated_t> OnCreatLobbyCallResult = new CallResult<LobbyCreated_t>();//create lobby Callback

    public CallResult<LobbyMatchList_t> OnLobbyMatchListCallResult;

    public Callback<GameLobbyJoinRequested_t> OnLobbyInviteCallResult;

    public Callback<LobbyChatUpdate_t> OnLobbyChatUpdateCallResult;

    public Callback<LobbyChatMsg_t> OnLobbyChatMsgResult;

    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamLobbyManager();
            Instance.Initialize();
        }
    }

    public override void Initialize()
    {
        MyUserObject = new UserObject();
        CSteamID UserSteamID = SteamUser.GetSteamID();
        MyUserObject.UserId = ((ulong)UserSteamID);
        MyUserObject.AvatarImageId = SteamFriends.GetLargeFriendAvatar(UserSteamID);
        MyUserObject.UserName = SteamFriends.GetFriendPersonaName(UserSteamID);
        MyUserObject.IsReady = false;
        MyUserObject.ShipType = 0;
        MyUserObject.ShipColor = 0;
        Debug.Log("Name:" + MyUserObject.UserName + "---ID:" + MyUserObject.UserId);
        OnLobbyInviteCallResult = Callback<GameLobbyJoinRequested_t>.Create(OnInvited);
        OnLobbyChatUpdateCallResult = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
        OnLobbyMatchListCallResult = CallResult<LobbyMatchList_t>.Create(OnLobbyMatchList);
        OnLobbyChatMsgResult = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        LaunchCommandLine();
        base.Initialize();
    }

    private void LaunchCommandLine()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        string input = "";
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "+connect_lobby" && args.Length > i + 1)
            {
                input = args[i + 1];
            }
        }

        if (!string.IsNullOrEmpty(input))
        {
            // Invite accepted, launched game. Join friend's game
            ulong lobbyId = 0;

            if (ulong.TryParse(input, out lobbyId))
            {
                JoinLobby(lobbyId);
            }
        }
    }

    public override void RequestLobbyList()
    {
        Debug.Log("Started get all lobby");
        OnLobbyMatchListCallResult.Set(SteamMatchmaking.RequestLobbyList());
        base.RequestLobbyList();
    }

    public void OnLobbyMatchList(LobbyMatchList_t lobbyMatchList, bool failure)
    {
        if (failure)
        {
            var errorLog = SteamUtils.GetAPICallFailureReason(OnLobbyMatchListCallResult.Handle);
            return;
        }
        Debug.Log("LobbyMatchList_T.m_nLobbiesMatching:" + lobbyMatchList.m_nLobbiesMatching);
        LobbyObjects = new LobbyObject[lobbyMatchList.m_nLobbiesMatching];
        for (int i = 0; i < LobbyObjects.Length; i++)
        {
            CSteamID cSteamID = SteamMatchmaking.GetLobbyByIndex(i);
            LobbyObjects[i] = new LobbyObject();
            LobbyObjects[i].LobbyId = (ulong)cSteamID;
            LobbyObjects[i].LobbyName = SteamMatchmaking.GetLobbyData(cSteamID, "name");
            string.IsNullOrEmpty(LobbyObjects[i].LobbyName);
            LobbyObjects[i].LobbyMaxPeople = SteamMatchmaking.GetLobbyMemberLimit(cSteamID);
            LobbyObjects[i].LobbyNowPeople = SteamMatchmaking.GetNumLobbyMembers(cSteamID);
        }
        DisplayLobby();
    }

    public override void DisplayLobby()
    {
        base.DisplayLobby();
    }

    public override void CreateLobby(LobbyType lobbyType)
    {
        ELobbyType eLobbyType;
        if (lobbyType == LobbyType.Public)
        {
            eLobbyType = ELobbyType.k_ELobbyTypePublic;
        }
        else
        {
            eLobbyType = ELobbyType.k_ELobbyTypePrivate;
        }
        OnCreatLobbyCallResult.Set(SteamMatchmaking.CreateLobby(eLobbyType, Custom_PlayFab.PlayFabLobbyManager.MAX_PLAYER_COUNT_IN_LOBBY), OnLobbyCreated);
        base.CreateLobby();
    }

    public void OnLobbyCreated(LobbyCreated_t lobbyCreated, bool failed)
    {
        if (failed)
        {
            return;
        }

        MyLobbyObject = new LobbyObject();
        MyLobbyObject.LobbyId = lobbyCreated.m_ulSteamIDLobby;
        MyLobbyObject.LobbyName = MyLobbyName + UnityEngine.Random.Range(0, 1000);
        SteamMatchmaking.SetLobbyData(new CSteamID(MyLobbyObject.LobbyId), "name", MyLobbyObject.LobbyName);
        JoinLobby(MyLobbyObject.LobbyId);
    }

    public override void JoinLobby(ulong lobbyId)
    {
        MyLobbyObject = new LobbyObject();
        MyLobbyObject.LobbyId = lobbyId;
        OnJoinLobbyCallResult.Set(SteamMatchmaking.JoinLobby(new CSteamID(lobbyId)), OnLobbyJoined);
        base.JoinLobby(lobbyId);
    }

    public void OnLobbyJoined(LobbyEnter_t lobbyEnter, bool failed)
    {
        JoinedLobby = true;

        LobbyMemberList();

        InvokeOnJoinLobbyCallback();

        MyUserObject.IsReady = false;
    }

    public override void InvokeOnJoinLobbyCallback()
    {
        base.InvokeOnJoinLobbyCallback();
    }

    public void OnInvited(GameLobbyJoinRequested_t joinCallback)
    {
        if (!JoinedLobby)
        {
            MyLobbyObject = new LobbyObject();
            MyLobbyObject.LobbyId = joinCallback.m_steamIDLobby.m_SteamID;
            Debug.Log("From " + SteamFriends.GetFriendPersonaName(joinCallback.m_steamIDFriend));
            JoinLobby(joinCallback.m_steamIDLobby.m_SteamID);
        }
        else
        {
            Debug.Log("User is joined this lobby---" + joinCallback.m_steamIDLobby);
        }
    }

    public override void LeaveLobby()
    {
        if (MyLobbyObject != null)
        {
            SteamMatchmaking.LeaveLobby(new CSteamID(MyLobbyObject.LobbyId));
            Debug.Log($"Leave Lobby {MyLobbyObject.LobbyName}");
        }

        MyUserObject.IsReady = false;
        MyUserObject.ShipType = 0;
        MyUserObject.ShipColor = 0;
        JoinedLobby = false;
        base.LeaveLobby();
    }

    public override void LobbyMemberList()
    {
        LobbyUserObjects = new Dictionary<ulong, UserObject>();
        int memberLength = SteamMatchmaking.GetNumLobbyMembers(new CSteamID(MyLobbyObject.LobbyId));
        for (int i = 0; i < memberLength; i++)
        {
            UserObject userObject = new UserObject();
            userObject.UserId = SteamMatchmaking.GetLobbyMemberByIndex(new CSteamID(MyLobbyObject.LobbyId), i).m_SteamID;
            userObject.AvatarImageId = SteamFriends.GetLargeFriendAvatar(new CSteamID(userObject.UserId));
            userObject.UserName = SteamFriends.GetFriendPersonaName(new CSteamID(userObject.UserId));
            LobbyUserObjects.Add(userObject.UserId, userObject);
        }
        IsOwner = (MyUserObject.UserId == (ulong)SteamMatchmaking.GetLobbyOwner(new CSteamID(MyLobbyObject.LobbyId)));
    }

    public override bool IsReadyToStart()
    {
        foreach (var key in LobbyUserObjects.Keys)
        {
            if (!LobbyUserObjects[key].IsReady)
            {
                return false;
            }
        }
        return true;
    }

    public override void Analyze(byte[] data, ulong steamId)
    {
        ChatMessageType chatMessageType = ParseChatMessageType(data);
        string message = System.Text.Encoding.UTF8.GetString(data, 1, data.Length - 1).TrimEnd('\0');
        switch (chatMessageType)
        {
            case ChatMessageType.StartGame:
                OnStartGame();
                break;
            case ChatMessageType.UserState:
                OnUserStateUpdate(message, steamId);
                break;
            case ChatMessageType.ChatMessage:

                break;
            case ChatMessageType.InvalidMessage:
                Debug.Log("Invalid Message.");
                break;
            default:
                break;
        }

    }

    public override void OnStartGame()
    {
        base.OnStartGame();
    }

    private void OnUserStateUpdate(string message, ulong steamId)
    {
        if (LobbyUserObjects.ContainsKey(steamId))
        {
            UserObject userObject = new UserObject();
            userObject = JsonUtility.FromJson<UserObject>(message);
            LobbyUserObjects[steamId].ShipColor = userObject.ShipColor;
            LobbyUserObjects[steamId].ShipType = userObject.ShipType;
            LobbyUserObjects[steamId].IsReady = userObject.IsReady;
            var state = new PlayerLobbyState(userObject.IsReady, userObject.ShipType, userObject.ShipColor);
            OnNetworkMessage_PlayerLobbyState_Receive(steamId, state);
        }
        UpdateLobbyMemberState(steamId);
    }

    public override void UpdateLobbyMemberState(ulong userId)
    {
        base.UpdateLobbyMemberState(userId);
    }

    public override void OnNetworkMessage_PlayerLobbyState_Receive(ulong userId, PlayerLobbyState playerLobbyState)
    {
        base.OnNetworkMessage_PlayerLobbyState_Receive(userId, playerLobbyState);
    }

    public void OnLobbyChatMsg(LobbyChatMsg_t callBack)
    {
        CSteamID steamID;
        byte[] data = new byte[4096];
        EChatEntryType chatEntryType;
        int ret = SteamMatchmaking.GetLobbyChatEntry((CSteamID)callBack.m_ulSteamIDLobby, (int)callBack.m_iChatID, out steamID, data, data.Length, out chatEntryType);
        Analyze(data, steamID.m_SteamID);
    }

    public void OnLobbyChatUpdate(LobbyChatUpdate_t lobbyChatUpdate)
    {
        LobbyMemberList();
        OnLobbyMemberChanged();
        SendUserState();
    }

    public override void OnLobbyMemberChanged()
    {
        base.OnLobbyMemberChanged();
    }

    public override void SendLobbyMessage(ChatMessageType chatMessageType, string message)
    {
        byte[] messageByte = System.Text.Encoding.UTF8.GetBytes(message);
        messageByte = AddArray(messageByte, 0, (byte)chatMessageType);
        bool ret = SteamMatchmaking.SendLobbyChatMsg(new CSteamID(MyLobbyObject.LobbyId), messageByte, messageByte.Length);
    }

    public override void SendUserState()
    {
        SendLobbyMessage(ChatMessageType.UserState, JsonUtility.ToJson(MyUserObject));
    }

    private void OnApplicationQuit()
    {
        LeaveLobby();
    }
}
