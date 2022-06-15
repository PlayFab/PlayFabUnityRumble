//--------------------------------------------------------------------------------------
// PlayFabSessionNetwork.cs
//
// Manage PlayFab network functions, receiving and sending messages on the PlayFab network.
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
using PlayFab.Party;

namespace Custom_PlayFab
{
    public class PlayFabSessionNetwork : SessionNetwork
    {
        public PlayFabMultiplayerManager PFMultiplayerManager { get; private set; }

        private Dictionary<ulong, PlayFabPlayer> lobbyPlayerDict = new Dictionary<ulong, PlayFabPlayer>();

        private int disconnectErrorCode = 63;

        private PlayFabSessionNetwork()
        {
            Initialize();
        }

        private void Initialize()
        {
            CurrentNetworkState = NetworkState.NoNetwork;
            PFMultiplayerManager = PlayFabMultiplayerManager.Get();
            AddEventListeners();
        }

        public static void CreateInstance()
        {
            if (Instance == null)
            {
                Instance = new PlayFabSessionNetwork();
            }
        }

        #region fire base class events

        protected override void FireEventHelloNetworkReceived(ulong playerId, string playerName)
        {
            base.FireEventHelloNetworkReceived(playerId, playerName);
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

        protected override void FireEventStartGameStateReceived(ulong playerId, StartGameState startGameState)
        {
            base.FireEventStartGameStateReceived(playerId, startGameState);
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

        protected override void FireEventNetworkStarted(string networkId)
        {
            base.FireEventNetworkStarted(networkId);
        }

        protected override void FireEventNetworkStopped(string networkId)
        {
            base.FireEventNetworkStopped(networkId);
        }

        protected override void FireEventNetworkJoined(string networkId)
        {
            base.FireEventNetworkJoined(networkId);
        }

        protected override void FireEventServersDisconnected()
        {
            base.FireEventServersDisconnected();
        }

        protected override void FireEventError(string errorType, int errorCode, string errorMessage)
        {
            base.FireEventError(errorType, errorCode, errorMessage);
        }

        protected override void FireEventRemotePlayerConnectionStatusChanged(ulong playerId)
        {
            base.FireEventRemotePlayerConnectionStatusChanged(playerId);
        }

        #endregion fire base class events

        private DeliveryOption GetDeliveryOptionForNetObject(BaseNetStateObject netObject)
        {
            switch (netObject.StateType)
            {
                case StateType.Reliable:
                    return DeliveryOption.Guaranteed;

                case StateType.Unreliable:
                    return DeliveryOption.BestEffort;

                default:
                    throw new NotImplementedException($"Unrecognized net state type of {netObject.StateType}");
            }
        }

        private void SendMessageToPlayer(PlayFabPlayer remotePlayer, BaseNetStateObject netState)
        {
            if (CurrentNetworkState != NetworkState.InNetwork)
            {
                return;
            }

            if (netState.StateType == StateType.Reliable)
            {
                Debug.LogFormat("PlayFabSessionNetwork.__SendMessageToPlayer({0}, {1})", remotePlayer.EntityKey.Id, netState.MessageType);
            }

            byte[] messageBytes;
            netState.SerializeTo(out messageBytes);

            PFMultiplayerManager.SendDataMessage(messageBytes, new PlayFabPlayer[] { remotePlayer }, GetDeliveryOptionForNetObject(netState));
        }

        private void SendMessageToAllRemotePlayers(BaseNetStateObject netState)
        {
            if (CurrentNetworkState != NetworkState.InNetwork)
            {
                return;
            }

            if (netState.StateType == StateType.Reliable)
            {
                Debug.LogFormat("PlayFabSessionNetwork.__SendMessageToAllRemotePlayers({0})", netState.MessageType);
            }

            var remotePlayerList = PFMultiplayerManager.RemotePlayers;
            if (remotePlayerList == null)
            {
                return;
            }

            byte[] messageBytes;
            netState.SerializeTo(out messageBytes);

            PFMultiplayerManager.SendDataMessage(messageBytes, remotePlayerList, GetDeliveryOptionForNetObject(netState));
        }

        public override void SendMessageToAll(BaseNetStateObject netState, bool isIncludeSelf = true)
        {
            if (netState.StateType == StateType.Reliable)
            {
                Debug.LogFormat("PlayFabSessionNetwork.SendMessageToAll({0})", netState.MessageType);
            }

            SendMessageToAllRemotePlayers(netState);
        }

        public override void SendMessageToMember(ulong memberUserId, BaseNetStateObject netState)
        {
            if (CurrentNetworkState != NetworkState.InNetwork)
            {
                return;
            }

            if (!lobbyPlayerDict.ContainsKey(memberUserId))
            {
                Debug.LogError("Player can not been found when send data message");
                return;
            }

            byte[] data;
            netState.SerializeTo(out data);
            PlayFabPlayer[] recievers = { lobbyPlayerDict[memberUserId] };

            PFMultiplayerManager.SendDataMessage(data, recievers, DeliveryOption.Guaranteed);
        }

        private void AddEventListeners()
        {
            PFMultiplayerManager.OnNetworkJoined += HandleNetworkJoined;
            PFMultiplayerManager.OnNetworkChanged += HandleNetworkChanged;
            PFMultiplayerManager.OnRemotePlayerJoined += HandleRemotePlayerJoined;
            PFMultiplayerManager.OnRemotePlayerLeft += HandleRemotePlayerLeft;
            PFMultiplayerManager.OnDataMessageReceived += HandleDataMessageReceived;
            PFMultiplayerManager.OnNetworkLeft += HandleNetworkLeft;
            PFMultiplayerManager.OnError += HandleOnError;
        }

        private void RemoveEventListeners()
        {
            PFMultiplayerManager.OnNetworkJoined -= HandleNetworkJoined;
            PFMultiplayerManager.OnNetworkChanged -= HandleNetworkChanged;
            PFMultiplayerManager.OnRemotePlayerJoined -= HandleRemotePlayerJoined;
            PFMultiplayerManager.OnRemotePlayerLeft -= HandleRemotePlayerLeft;
            PFMultiplayerManager.OnDataMessageReceived -= HandleDataMessageReceived;
            PFMultiplayerManager.OnNetworkLeft -= HandleNetworkLeft;
            PFMultiplayerManager.OnError -= HandleOnError;
        }

        public override void Cleanup()
        {
            RemoveEventListeners();
            base.Cleanup();
        }

        public override void StartNetworking(ulong userId, string networkID)
        {
            Debug.Log("PlayFabSessionNetwork.StartNetworking()");

            if (CurrentNetworkState == SessionNetwork.NetworkState.DisconnectedFromNetwork)
            {
                InitializeNetwork();
            }

            Assert.IsTrue(CurrentNetworkState == NetworkState.NoNetwork);

            if (CurrentNetworkState != NetworkState.NoNetwork)
            {
                return;
            }

            CurrentNetworkState = NetworkState.StartingNetwork;

            PFMultiplayerManager.CreateAndJoinNetwork();
        }

        public override void JoinNetwork(string networkID)
        {
            PFMultiplayerManager.JoinNetwork(networkID);
        }

        public override void StopNetworking()
        {
            if (CurrentNetworkState != NetworkState.InNetwork)
            {
                return;
            }

            CurrentNetworkState = NetworkState.StoppingNetwork;

            if (PFMultiplayerManager != null)
            {
                PFMultiplayerManager.LeaveNetwork();
            }
        }

        private void AddPlayerToLobby(PlayFabPlayer player)
        {
            var playerEntityId = PlayFabCustomUtils.GetUlongIdPlayFabPlayer(player);
            if (lobbyPlayerDict.ContainsKey(playerEntityId))
            {
                Debug.LogError($"[PlayFabSessionNetwork.AddPlayerToLobby] player already exists in lobby player dictionary: {playerEntityId}");
                return;
            }

            lobbyPlayerDict.Add(playerEntityId, player);
        }

        public void HandleDataMessageReceived(object sender, PlayFabPlayer from, byte[] buffer)
        {
            if (CurrentNetworkState != NetworkState.InNetwork)
            {
                return;
            }

            var messageType = SessionMessageHandler.ParseMessageType(buffer);

            if (messageType != SessionMessageType.UpdateShipState)
            {
                Debug.LogFormat("PlayFabSessionNetwork.__HandleDataMessageReceived() total bytes of {0} from {1}", buffer.Length, from.EntityKey.Id);
                Debug.LogFormat(">>> message type is: {0}", messageType);
            }

            var expectedMessageSize = SessionMessageHandler.ExpectedMessageSize(messageType);

            if (messageType != SessionMessageType.InvalidMessage && messageType != SessionMessageType.HelloNetwork && expectedMessageSize != buffer.Length - 1)
            {
                Debug.LogFormat(">>> UNEXPECTED MESSAGE SIZE {0}, SHOULD BE {1}", buffer.Length - 1, expectedMessageSize);
                return;
            }

            switch (messageType)
            {
                case SessionMessageType.HelloNetwork:
                    {
                        var helloNetwork = SessionMessageHandler.ParseMessage<HelloNetwork>(buffer);

                        if (PlayFabRuntimeInfos.Instance.GetMyUlongEntityId() == PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from))
                        {
                            return;
                        }

                        AddPlayerToLobby(from);
                        FireEventHelloNetworkReceived(helloNetwork.MyXuid, helloNetwork.PlayerName);
                    }
                    break;

                case SessionMessageType.LobbyState:
                    {
                        var lobbyState = SessionMessageHandler.ParseMessage<LobbyState>(buffer);
                        FireEventLobbyStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), lobbyState);
                    }
                    break;

                case SessionMessageType.PlayerLobbyState:
                    {
                        var playerLobbyState = SessionMessageHandler.ParseMessage<PlayerLobbyState>(buffer);
                        FireEventPlayerLobbyStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), playerLobbyState);
                    }
                    break;

                case SessionMessageType.GameState:
                    {
                        var gameState = SessionMessageHandler.ParseMessage<GameState>(buffer);
                        FireEventGameStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), gameState);
                    }
                    break;

                case SessionMessageType.PlayerGameState:
                    {
                        var playerGameState = SessionMessageHandler.ParseMessage<PlayerGameState>(buffer);
                        FireEventPlayerGameStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), playerGameState);
                    }
                    break;
                case SessionMessageType.StartGameState:
                    {
                        var startGameState = SessionMessageHandler.ParseMessage<StartGameState>(buffer);
                        FireEventStartGameStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), startGameState);
                    }
                    break;
                case SessionMessageType.SpawnAsteroidState:
                    {
                        var spawnAsteroidState = SessionMessageHandler.ParseMessage<SpawnAsteroidState>(buffer);
                        FireEventSpawnAsteroidStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), spawnAsteroidState);
                    }
                    break;

                case SessionMessageType.UpdateAsteroidState:
                    {
                        var updateAsteroidState = SessionMessageHandler.ParseMessage<UpdateAsteroidState>(buffer);
                        FireEventUpdateAsteroidStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), updateAsteroidState);
                    }
                    break;

                case SessionMessageType.SpawnShipState:
                    {
                        var spawnShipState = SessionMessageHandler.ParseMessage<SpawnShipState>(buffer);
                        FireEventSpawnShipStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), spawnShipState);
                    }
                    break;

                case SessionMessageType.UpdateShipState:
                    {
                        var updateShipState = SessionMessageHandler.ParseMessage<UpdateShipState>(buffer);
                        FireEventUpdateShipStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), updateShipState);
                    }

                    break;

                case SessionMessageType.DestroyShipState:
                    {
                        var destroyShipState = SessionMessageHandler.ParseMessage<DestroyShipState>(buffer);
                        FireEventDestroyShipStateReceived(PlayFabCustomUtils.GetUlongIdPlayFabPlayer(from), destroyShipState);
                    }

                    break;

                case SessionMessageType.GoodbyeNetwork:
                    {
                        var goodbyeNetwork = SessionMessageHandler.ParseMessage<GoodbyeNetwork>(buffer);

                        // delete player from player local list
                        lobbyPlayerDict.Remove(goodbyeNetwork.MyXuid);

                        FireEventGoodbyeNetworkReceived(goodbyeNetwork.MyXuid);

                    }
                    break;

                case SessionMessageType.InvalidMessage:
                    break;

                default:
                    throw new NotImplementedException($"Message type {messageType} is not handled.");
            }
        }

        private void HandleRemotePlayerJoined(object sender, PlayFabPlayer player)
        {
            Debug.LogFormat("PlayFabSessionNetwork.__HandleRemotePlayerJoined({0})", player.EntityKey.Id);

            // tell the remote player what my XUID is
            SendMessageToPlayer(player, new HelloNetwork(PlayFabRuntimeInfos.Instance.GetMyUlongEntityId(), PlayFabRuntimeInfos.Instance.MySteamUserName));
        }

        private void HandleRemotePlayerLeft(object sender, PlayFabPlayer player)
        {
            Debug.Log("PlayFabSessionNetwork.__HandleRemotePlayerLeft()");

            var entityId = PlayFabCustomUtils.GetUlongIdPlayFabPlayer(player);

            if (lobbyPlayerDict.ContainsKey(entityId))
            {
                lobbyPlayerDict.Remove(entityId);
                FireEventGoodbyeNetworkReceived(entityId);
            }
        }

        private void HandleNetworkChanged(object sender, string newNetworkId)
        {
            Debug.LogFormat("PlayFabSessionNetwork.__HandleNetworkChanged({0})", newNetworkId);

            // note: beyond the scope of this sample, but in the event this should happen,
            // the code needs to take action to migrate our local player to the new network
            // by leaving the old one, and joining the new one.
        }

        private void HandleNetworkJoined(object sender, string networkId)
        {
            Debug.LogFormat("PlayFabSessionNetwork.__HandleNetworkJoined({0})", networkId);

            SetNetworkStateAndId(NetworkState.InNetwork, networkId);

            FireEventNetworkJoined(networkId);
        }

        private void HandleNetworkLeft(object sender, string networkId)
        {
            Debug.Log("PlayFabSessionNetwork.__HandleNetworkLeft()");

            lobbyPlayerDict.Clear();

            ClearNetworkStateAndId();

            FireEventNetworkStopped(networkId);
        }

        void HandleOnError(object sender, PlayFabMultiplayerManagerErrorArgs args)
        {
            if (args.Message.Contains("Unmapped"))
            {
                return;
            }
            FireEventError(args.Type.ToString(), args.Code, args.Message);
            if (args.Code == disconnectErrorCode)
            {
                CurrentNetworkState = NetworkState.DisconnectedFromNetwork;
                lobbyPlayerDict.Clear();
                Cleanup();
                PlayFabEnvInitializer.Instance.DestroyPartyPrefab();
            }
        }

        public override void InitializeNetwork()
        {
            if (UnityEngine.Object.FindObjectsOfType<PlayFabMultiplayerManager>().Length == 0)
            {
                PlayFabEnvInitializer.Instance.InstantiatePartyPrefab();
                Initialize();
            }
            base.InitializeNetwork();
        }
    }
}
