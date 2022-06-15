//--------------------------------------------------------------------------------------
// LobbyManager.cs
//
// The Manager class for the Lobby Manager and all Lobby's base class.
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

public class LobbyManager : ManagerBaseClass<LobbyManager>
{
    public const int MAX_LOBBY_MEMBER_COUNT = 4;

    public class LobbyObject
    {
        public ulong LobbyId { get; set; }
        public string LobbyName { get; set; }
        public int LobbyMaxPeople { get; set; }
        public int LobbyNowPeople { get; set; }
        public bool IsPublic { get; set; }
    }

    [Serializable]
    public class UserObject
    {
        public ulong UserId;
        public string UserName;
        public int AvatarImageId;
        public bool IsReady;
        public int ShipType;
        public int ShipColor;
    }

    public enum LobbyType
    {
        Public,
        Private
    }

    [Serializable]
    public class GameStartMessage
    {
        public ulong SenderSteamId;
    }

    private string myLobbyName = "csi_unity_lobby";

    public string MyLobbyName
    {
        get
        {
            return myLobbyName;
        }
        set
        {
            myLobbyName = value;
        }
    }

    public bool JoinedLobby { get; set; }

    public enum ChatMessageType : byte
    {
        StartGame = 0xC0,
        UserState = 0xC1,
        ChatMessage = 0xC2,
        InvalidMessage = 0xFF,
    }

    private static Dictionary<byte, ChatMessageType> ChatMessageTypes = new Dictionary<byte, ChatMessageType>()
    {
        { (byte)ChatMessageType.StartGame, ChatMessageType.StartGame },
        { (byte)ChatMessageType.UserState, ChatMessageType.UserState },
        { (byte)ChatMessageType.ChatMessage, ChatMessageType.ChatMessage },
        { (byte)ChatMessageType.InvalidMessage, ChatMessageType.InvalidMessage },
    };

    public bool IsOwner { get; set; }

    public bool IsInGame { get; set; }

    public UserObject MyUserObject { get; set; }

    public LobbyObject MyLobbyObject { get; set; }

    public LobbyObject[] LobbyObjects { get; set; }

    public Dictionary<ulong, UserObject> LobbyUserObjects = new Dictionary<ulong, UserObject>();

    public event Action OnJoinedLobbyAction;

    public event Action<ulong> OnUpdateUserStateAction;

    public event Action OnLobbyMemberChangedAction;

    public event Action OnDisplayLobbyListAction;

    public event Action OnUpdateUserReadyStateAction;

    public event Action OnStartGameMessageAction;

    public event Action<ulong, PlayerLobbyState> OnPlayerLobbyStateReceived;

    public event Action OnInvitedLobbyMemberMaxEvent;

    public virtual void Start()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual void RequestLobbyList()
    {

    }

    public virtual void CreateLobby(int memberCount = LobbyManager.MAX_LOBBY_MEMBER_COUNT, LobbyType lobby_Type = LobbyType.Public)
    {

    }

    public virtual void JoinLobby(ulong lobbyId)
    {

    }


    public virtual void LeaveLobby()
    {

    }

    public virtual void LobbyMemberList()
    {

    }

    public virtual void SendLobbyMessage(ChatMessageType chatMessageType, string message)
    {

    }

    public virtual void SendUserState()
    {

    }

    public virtual void Analyze(byte[] message, ulong userId)
    {

    }

    public virtual void InvokeOnJionLobbyCallBack()
    {
        OnJoinedLobbyAction?.Invoke();
    }

    public virtual void UpdateLobbyMemberState(ulong userId)
    {
        OnUpdateUserStateAction?.Invoke(userId);
    }

    public virtual void OnLobbyMemberChanged()
    {
        OnLobbyMemberChangedAction?.Invoke();
    }

    public virtual void DisplayLobby()
    {
        OnDisplayLobbyListAction?.Invoke();
    }

    public virtual void UpdateUserState()
    {
        OnUpdateUserReadyStateAction?.Invoke();
    }

    public virtual void OnStartGame()
    {
        OnStartGameMessageAction?.Invoke();
    }

    public virtual void OnNetworkMessage_PlayerLobbyState_Receive(ulong userId, PlayerLobbyState playerLobbyState)
    {
        OnPlayerLobbyStateReceived?.Invoke(userId, playerLobbyState);
    }

    public virtual bool IsReadyToStart()
    {
        return false;
    }

    public virtual void ActivateGameOverlay()
    {
    }

    public virtual void OnInvitedLobbyMemberMax()
    {
        OnInvitedLobbyMemberMaxEvent?.Invoke();
    }

    public virtual Texture2D GetSteamImageAsTexture2D(int avatarId)
    {
        return null;
    }

    public virtual void QuickPlay()
    {
    }

    public ChatMessageType ParseChatMessageType(byte[] message)
    {
        if (message.Length < 1)
        {
            return ChatMessageType.InvalidMessage;
        }
        var messageTypeByte = message[0];

        if (!ChatMessageTypes.ContainsKey(messageTypeByte))
        {
            return ChatMessageType.InvalidMessage;
        }

        return ChatMessageTypes[messageTypeByte];
    }

    public byte[] AddArray(byte[] bornArray, int index, byte value)
    {
        ArrayList list = new ArrayList(bornArray);
        if (index < 0)
        {
            index = 0;
        }
        if (index > bornArray.Length - 1)
        {
            index = bornArray.Length;
        }
        list.Insert(index, value);
        byte[] des = new byte[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            des[i] = (byte)list[i];
        }
        return des;
    }
}
