//--------------------------------------------------------------------------------------
// SteamSessionNetwork.cs
//
// The class for the Steam Session Network Manager and functionality.
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
using System.Runtime.InteropServices;
using UnityEngine;
using Steamworks;

public class SteamSessionNetwork : SessionNetwork
{
    List<ulong> inGamePlayerList = new List<ulong>();
    private const int MAX_RECEIVE_MESSAGE_COUNT = 10;
    private const int MESSAGE_CHANNEL_ID = 3;
    private IntPtr[] ReceivedMessageBuffer;
    Callback<SteamNetworkingMessagesSessionFailed_t> SessionFailedCallback;
    Callback<SteamServersDisconnected_t> ServersDisconnectedCallback;
    Callback<SteamNetConnectionStatusChangedCallback_t> SteamNetConnectionStatusChangedCallback;

    private SteamSessionNetwork()
    {
        ReceivedMessageBuffer = new IntPtr[MAX_RECEIVE_MESSAGE_COUNT];
        CurrentNetworkState = NetworkState.InNetwork;
        SessionFailedCallback = Callback<SteamNetworkingMessagesSessionFailed_t>.Create(OnSessionFailed);
        ServersDisconnectedCallback = Callback<SteamServersDisconnected_t>.Create(OnServersDisconnectedCallback);
        SteamNetConnectionStatusChangedCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnRemotePlayerConnectionStatusChangedCallBack);
    }

    public static void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = new SteamSessionNetwork();
        }
    }

    public override void Tick()
    {
        int receivedCount = SteamNetworkingMessages.ReceiveMessagesOnChannel(MESSAGE_CHANNEL_ID, ReceivedMessageBuffer, MAX_RECEIVE_MESSAGE_COUNT);
        if (receivedCount <= 0)
        {
            return;
        }
        HandleReceivedMessages(ReceivedMessageBuffer, receivedCount);
        Array.Clear(ReceivedMessageBuffer, 0, ReceivedMessageBuffer.Length);
    }

    private void HandleReceivedMessages(IntPtr[] receivedMessages, int messageCount)
    {
        for (int i = 0; i < messageCount; i++)
        {
            IntPtr msgPtr = receivedMessages[i];
            if (msgPtr == IntPtr.Zero)
            {
                continue;
            }

            var msg = SteamNetworkingMessage_t.FromIntPtr(msgPtr);
            if (msg.m_cbSize > 0)
            {
                byte[] msgDataBuf = IntPtrToByteArray(msg.m_pData, msg.m_cbSize);
                Analyze(msgDataBuf, msg.m_identityPeer.GetSteamID64());
            }

            SteamNetworkingMessage_t.Release(msgPtr);
        }
    }

    public virtual void Analyze(byte[] buffer, ulong remoteSteamId)
    {
        var messageType = SessionMessageHandler.ParseMessageType(buffer);

        if (messageType != SessionMessageType.UpdateShipState)
        {
            Debug.LogFormat("SessionNetwork.__HandleDataMessageReceived() total bytes of {0} from {1}", buffer.Length, remoteSteamId);
            Debug.LogFormat(">>> message type is: {0}", messageType);
        }

        var expectedMessageSize = SessionMessageHandler.ExpectedMessageSize(messageType);

        if (messageType != SessionMessageType.InvalidMessage && messageType != SessionMessageType.VoiceChatState && expectedMessageSize != buffer.Length - 1)
        {
            Debug.LogFormat(">>> UNEXPECTED MESSAGE SIZE {0}, SHOULD BE {1}", buffer.Length - 1, expectedMessageSize);
            return;
        }

        switch (messageType)
        {
            case SessionMessageType.HelloNetwork:
                {
                    var helloNetwork = SessionMessageHandler.ParseMessage<HelloNetwork>(buffer);
                    FireEventHelloNetworkReceived(helloNetwork.MyXuid);
                }
                break;

            case SessionMessageType.LobbyState:
                {
                    var lobbyState = SessionMessageHandler.ParseMessage<LobbyState>(buffer);
                    FireEventLobbyStateReceived(remoteSteamId, lobbyState);
                }
                break;

            case SessionMessageType.PlayerLobbyState:
                {
                    var playerLobbyState = SessionMessageHandler.ParseMessage<PlayerLobbyState>(buffer);
                    FireEventPlayerLobbyStateReceived(remoteSteamId, playerLobbyState);
                }
                break;

            case SessionMessageType.GameState:
                {
                    var gameState = SessionMessageHandler.ParseMessage<GameState>(buffer);
                    FireEventGameStateReceived(remoteSteamId, gameState);
                }
                break;

            case SessionMessageType.PlayerGameState:
                {
                    var playerGameState = SessionMessageHandler.ParseMessage<PlayerGameState>(buffer);
                    FireEventPlayerGameStateReceived(remoteSteamId, playerGameState);
                }
                break;

            case SessionMessageType.SpawnAsteroidState:
                {
                    var spawnAsteroidState = SessionMessageHandler.ParseMessage<SpawnAsteroidState>(buffer);
                    FireEventSpawnAsteroidStateReceived(remoteSteamId, spawnAsteroidState);
                }
                break;

            case SessionMessageType.UpdateAsteroidState:
                {
                    var updateAsteroidState = SessionMessageHandler.ParseMessage<UpdateAsteroidState>(buffer);
                    FireEventUpdateAsteroidStateReceived(remoteSteamId, updateAsteroidState);
                }
                break;

            case SessionMessageType.SpawnShipState:
                {
                    var spawnShipState = SessionMessageHandler.ParseMessage<SpawnShipState>(buffer);
                    FireEventSpawnShipStateReceived(remoteSteamId, spawnShipState);
                }
                break;

            case SessionMessageType.UpdateShipState:
                {
                    var updateShipState = SessionMessageHandler.ParseMessage<UpdateShipState>(buffer);
                    FireEventUpdateShipStateReceived(remoteSteamId, updateShipState);
                }

                break;

            case SessionMessageType.DestroyShipState:
                {
                    var destroyShipState = SessionMessageHandler.ParseMessage<DestroyShipState>(buffer);
                    FireEventDestroyShipStateReceived(remoteSteamId, destroyShipState);
                }

                break;

            case SessionMessageType.GoodbyeNetwork:
                {
                    var goodbyeNetwork = SessionMessageHandler.ParseMessage<GoodbyeNetwork>(buffer);
                    // delete player from player local list
                    inGamePlayerList.Remove(goodbyeNetwork.MyXuid);
                    FireEventGoodbyeNetworkReceived(goodbyeNetwork.MyXuid);

                }
                break;
            case SessionMessageType.VoiceChatState:
                {
                    var voiceChatState = SessionMessageHandler.ParseMessage<VoiceChatState>(buffer);
                    FireEventVoiceChatStateReceived(remoteSteamId, voiceChatState);
                }
                break;

            case SessionMessageType.InvalidMessage:
                // for now, do nothing with invalid messages since they could be noisy?
                break;

            default:
                throw new NotImplementedException($"Message type {messageType} not handled.");
        }
    }

    private void OnSessionFailed(SteamNetworkingMessagesSessionFailed_t sessionFailedInfo)
    {
        Debug.LogFormat("send message failed : {0}", sessionFailedInfo.m_info.m_szConnectionDescription);
    }

    private void OnServersDisconnectedCallback(SteamServersDisconnected_t steamServersDisconnected)
    {
        Debug.Log("OnSteamServersDisconnected: " + steamServersDisconnected.m_eResult);
        FireEventServersDisconnected();
    }

    private void OnRemotePlayerConnectionStatusChangedCallBack(SteamNetConnectionStatusChangedCallback_t steamNetConnectionStatusChangedCallback)
    {
        string id = steamNetConnectionStatusChangedCallback.m_info.m_szConnectionDescription;
        if (id.Contains("steamid"))
        {
            id = id.Split(':')[1];
            id = id.Replace("rem msg vport", "");
            id = id.Replace(" ", "");
            FireEventRemotePlayerConnectionStatusChanged(ulong.Parse(id));
            Debug.LogFormat("{0} Connection Status Changed", id);
        }

    }

    public override void StartNetworking(ulong localUserId, string networkID)
    {
        Debug.Log("SessionNetwork.StartNetworking()");

        CurrentNetworkState = NetworkState.InNetwork;

        FireNetworkStartEvent(null);

        LobbyManager.Instance.LeaveLobby();
    }

    #region Fire event of base class
    protected override void FireEventHelloNetworkReceived(ulong playerId)
    {
        base.FireEventHelloNetworkReceived(playerId);
    }

    protected override void FireEventLobbyStateReceived(ulong playerId, LobbyState state)
    {
        base.FireEventLobbyStateReceived(playerId, state);
    }

    protected override void FireEventPlayerLobbyStateReceived(ulong playerId, PlayerLobbyState playerLobbyState)
    {
        base.FireEventPlayerLobbyStateReceived(playerId, playerLobbyState);
    }

    protected override void FireEventGameStateReceived(ulong playerId, GameState gameState)
    {
        base.FireEventGameStateReceived(playerId, gameState);
    }

    protected override void FireEventPlayerGameStateReceived(ulong playerId, PlayerGameState playerGameState)
    {
        base.FireEventPlayerGameStateReceived(playerId, playerGameState);
    }

    protected override void FireEventSpawnAsteroidStateReceived(ulong playerId, SpawnAsteroidState spawnAsteroidState)
    {
        base.FireEventSpawnAsteroidStateReceived(playerId, spawnAsteroidState);
    }

    protected override void FireEventUpdateAsteroidStateReceived(ulong playerId, UpdateAsteroidState updateAsteroidState)
    {
        base.FireEventUpdateAsteroidStateReceived(playerId, updateAsteroidState);
    }

    protected override void FireEventSpawnShipStateReceived(ulong playerId, SpawnShipState spawnShipState)
    {
        base.FireEventSpawnShipStateReceived(playerId, spawnShipState);
    }

    protected override void FireEventUpdateShipStateReceived(ulong playerId, UpdateShipState updateShipState)
    {
        base.FireEventUpdateShipStateReceived(playerId, updateShipState);
    }

    protected override void FireEventDestroyShipStateReceived(ulong playerId, DestroyShipState destroyShipState)
    {
        base.FireEventDestroyShipStateReceived(playerId, destroyShipState);
    }

    protected override void FireEventGoodbyeNetworkReceived(ulong playerId)
    {
        base.FireEventGoodbyeNetworkReceived(playerId);
    }

    protected override void FireNetworkStartEvent(string networkId)
    {
        base.FireNetworkStartEvent(networkId);
    }

    protected override void FireNetworkStopEvent(string networkId)
    {
        base.FireNetworkStopEvent(networkId);
    }

    protected override void FireEventVoiceChatStateReceived(ulong playerId, VoiceChatState voiceChatState)
    {
        base.FireEventVoiceChatStateReceived(playerId, voiceChatState);
    }

    protected override void FireEventServersDisconnected()
    {
        base.FireEventServersDisconnected();
    }

    protected override void FireEventRemotePlayerConnectionStatusChanged(ulong playerId)
    {
        base.FireEventRemotePlayerConnectionStatusChanged(playerId);
    }
    #endregion Fire event of base class

    public override void SaveLobbyPlayers()
    {
        foreach (var userObj in LobbyManager.Instance.LobbyUserObjects.Keys)
        {
            inGamePlayerList.Add(userObj);
        }

        base.SaveLobbyPlayers();
    }

    private bool ConvertNetStateToIntPtr(BaseNetStateObject stateObject, out IntPtr messageIntPtr, out int messageSize)
    {
        byte[] messageBytes;
        stateObject.SerializeTo(out messageBytes);
        messageSize = messageBytes.Length;
        messageIntPtr = Marshal.AllocHGlobal(messageSize);
        try
        {
            Marshal.Copy(messageBytes, 0, messageIntPtr, messageSize);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private byte[] IntPtrToByteArray(IntPtr intptr, int size)
    {
        byte[] result = new byte[size];
        Marshal.Copy(intptr, result, 0, size);

        return result;
    }

    private int GetNetStateSteamReliableValue(BaseNetStateObject netstate)
    {
        switch (netstate.StateType)
        {
            case StateType.Reliable:
                return Constants.k_nSteamNetworkingSend_ReliableNoNagle;
            case StateType.Unreliable:
                return Constants.k_nSteamNetworkingSend_UnreliableNoDelay;
            default:
                return Constants.k_nSteamNetworkingSend_ReliableNoNagle;
        }
    }

    private void SendMessageToAllRemotePlayers(BaseNetStateObject netState, bool isIncludeSelf = true)
    {
        if (CurrentNetworkState != NetworkState.InNetwork)
        {
            return;
        }

        if (netState.StateType == StateType.Reliable)
        {
            Debug.LogFormat("SessionNetwork.__SendMessageToAllRemotePlayers({0})", netState.MessageType);
        }

        // convert netstate to intptr
        IntPtr msgPtr;
        int msgSize;
        if (!ConvertNetStateToIntPtr(netState, out msgPtr, out msgSize))
        {
            return;
        }

        foreach (var playerSteamId in inGamePlayerList)
        {
            if (!isIncludeSelf && playerSteamId == SteamUser.GetSteamID().m_SteamID)
            {
                continue;
            }

            SteamNetworkingIdentity identity = new SteamNetworkingIdentity();
            identity.SetSteamID64(playerSteamId);

            SteamNetworkingMessages.SendMessageToUser(ref identity, msgPtr, (uint)msgSize, GetNetStateSteamReliableValue(netState), MESSAGE_CHANNEL_ID);
        }

        Marshal.FreeHGlobal(msgPtr);
    }

    public override void SendMessageToAll(BaseNetStateObject netState, bool isIncludeSelf = true)
    {
        if (netState.StateType == StateType.Reliable)
        {
            Debug.LogFormat("SessionNetwork.SendMessageToAll({0})", netState.MessageType);
        }

        SendMessageToAllRemotePlayers(netState, isIncludeSelf);
    }

    public override void SendMessageToMember(ulong memberUserId, BaseNetStateObject netState)
    {
        if (CurrentNetworkState != NetworkState.InNetwork)
        {
            return;
        }

        IntPtr msgPtr;
        int msgSize;
        if (!ConvertNetStateToIntPtr(netState, out msgPtr, out msgSize))
        {
            return;
        }

        if (netState.StateType == StateType.Reliable)
        {
            Debug.LogFormat("SessionNetwork.SendMessageToAll({0})", netState.MessageType);
        }

        SteamNetworkingIdentity identity = new SteamNetworkingIdentity();
        identity.SetSteamID64(memberUserId);

        SteamNetworkingMessages.SendMessageToUser(ref identity, msgPtr, (uint)msgSize, GetNetStateSteamReliableValue(netState), MESSAGE_CHANNEL_ID);

        Marshal.FreeHGlobal(msgPtr);
    }

    public override void StopNetworking()
    {
        if (CurrentNetworkState != NetworkState.InNetwork)
        {
            return;
        }

        GoodbyeNetwork goodbye = new GoodbyeNetwork(LobbyManager.Instance.MyUserObject.UserId);
        SendMessageToAll(goodbye, false);
        inGamePlayerList.Clear();
        FireNetworkStopEvent(null);
        CurrentNetworkState = NetworkState.NoNetwork;
    }
}
