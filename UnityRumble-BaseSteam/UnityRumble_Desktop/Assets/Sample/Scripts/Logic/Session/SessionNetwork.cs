﻿//--------------------------------------------------------------------------------------
// SessionNetwork.cs
//
// The game network which wraps the underlying networking APIs and adds messaging.
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
// Advanced Technology Group (ATG)
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class SessionNetwork : ManagerBaseClass<SessionNetwork>
{
    public enum NetworkState : int
    {
        NoNetwork = 0,
        StartingNetwork = 1,
        InNetwork = 2,
        StoppingNetwork = 3,
    }

    // parameters are: sender XUID, message 
    public event Action<ulong> OnNetworkMessage_HelloNetwork_Received;
    public event Action<ulong, LobbyState> OnNetworkMessage_LobbyState_Received;
    public event Action<ulong, PlayerLobbyState> OnNetworkMessage_PlayerLobbyState_Received;
    public event Action<ulong, GameState> OnNetworkMessage_GameState_Received;
    public event Action<ulong, PlayerGameState> OnNetworkMessage_PlayerGameState_Received;
    public event Action<ulong, SpawnAsteroidState> OnNetworkMessage_SpawnAsteroidState_Received;
    public event Action<ulong, UpdateAsteroidState> OnNetworkMessage_UpdateAsteroidState_Received;
    public event Action<ulong, SpawnShipState> OnNetworkMessage_SpawnShipState_Received;
    public event Action<ulong, UpdateShipState> OnNetworkMessage_UpdateShipState_Received;
    public event Action<ulong, DestroyShipState> OnNetworkMessage_DestroyShipState_Received;
    public event Action<ulong> OnNetworkMessage_GoodbyeNetwork_Received;
    public event Action<ulong, VoiceChatState> OnNetworkMessage_VoiceChat_Received;
    // parameters are: the network ID that was started
    public event Action<string> OnNetworkStarted;
    // parameters are: the network ID that was stopped
    public event Action<string> OnNetworkStopped;
    // Myself disconnected servers
    public event Action OnNetworkDisconnected;

    public event Action<ulong> OnRemotePlayerConnectionStatusChanged;

    public NetworkState CurrentNetworkState { get; protected set; }

    public virtual void Tick()
    {
    }

    protected virtual void FireEventHelloNetworkReceived(ulong playerId)
    {
        OnNetworkMessage_HelloNetwork_Received?.Invoke(playerId);
    }

    protected virtual void FireEventLobbyStateReceived(ulong playerId, LobbyState state)
    {
        OnNetworkMessage_LobbyState_Received?.Invoke(playerId, state);
    }

    protected virtual void FireEventPlayerLobbyStateReceived(ulong playerId, PlayerLobbyState playerLobbyState)
    {
        OnNetworkMessage_PlayerLobbyState_Received?.Invoke(playerId, playerLobbyState);
    }

    protected virtual void FireEventGameStateReceived(ulong playerId, GameState gameState)
    {
        OnNetworkMessage_GameState_Received?.Invoke(playerId, gameState);
    }

    protected virtual void FireEventPlayerGameStateReceived(ulong playerId, PlayerGameState playerGameState)
    {
        OnNetworkMessage_PlayerGameState_Received?.Invoke(playerId, playerGameState);
    }

    protected virtual void FireEventSpawnAsteroidStateReceived(ulong playerId, SpawnAsteroidState spawnAsteroidState)
    {
        OnNetworkMessage_SpawnAsteroidState_Received?.Invoke(playerId, spawnAsteroidState);
    }

    protected virtual void FireEventUpdateAsteroidStateReceived(ulong playerId, UpdateAsteroidState updateAsteroidState)
    {
        OnNetworkMessage_UpdateAsteroidState_Received?.Invoke(playerId, updateAsteroidState);
    }

    protected virtual void FireEventSpawnShipStateReceived(ulong playerId, SpawnShipState spawnShipState)
    {
        OnNetworkMessage_SpawnShipState_Received?.Invoke(playerId, spawnShipState);
    }

    protected virtual void FireEventUpdateShipStateReceived(ulong playerId, UpdateShipState updateShipState)
    {
        OnNetworkMessage_UpdateShipState_Received?.Invoke(playerId, updateShipState);
    }

    protected virtual void FireEventDestroyShipStateReceived(ulong playerId, DestroyShipState destroyShipState)
    {
        OnNetworkMessage_DestroyShipState_Received?.Invoke(playerId, destroyShipState);
    }

    protected virtual void FireEventGoodbyeNetworkReceived(ulong playerId)
    {
        OnNetworkMessage_GoodbyeNetwork_Received?.Invoke(playerId);
    }

    protected virtual void FireNetworkStartEvent(string networkId)
    {
        OnNetworkStarted?.Invoke(networkId);
    }

    protected virtual void FireNetworkStopEvent(string networkId)
    {
        OnNetworkStopped?.Invoke(networkId);
    }

    protected virtual void FireEventVoiceChatStateReceived(ulong playerId, VoiceChatState voiceChatState)
    {
        OnNetworkMessage_VoiceChat_Received?.Invoke(playerId, voiceChatState);
    }

    // Myself disconnected servers
    protected virtual void FireEventServersDisconnected()
    {
        OnNetworkDisconnected?.Invoke();
    }

    protected virtual void FireEventRemotePlayerConnectionStatusChanged(ulong playerId)
    {
        OnRemotePlayerConnectionStatusChanged?.Invoke(playerId);
    }

    public virtual void SaveLobbyPlayers()
    { }

    public abstract void StartNetworking(ulong userId, string networkID);

    public abstract void StopNetworking();

    public abstract void SendMessageToMember(ulong memberUserId, BaseNetStateObject netState);

    public abstract void SendMessageToAll(BaseNetStateObject netState, bool isIncludeSelf = true);
}
