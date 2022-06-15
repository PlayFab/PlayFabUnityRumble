//--------------------------------------------------------------------------------------
// PlayFabRuntimeInfos.cs
//
// Manage Runtime infomations of users in PlayFab.
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
using PlayFab;
using PlayFab.Multiplayer;

namespace Custom_PlayFab
{
    public class PlayFabRuntimeInfos
    {
        public event Action PlayerInfoUpdatedEvent;
        private static PlayFabRuntimeInfos instance;
        private PFEntityKey lobbyOwnerEntityKey;
        private List<PFEntityKey> entityKeyInLobbyOrGame = new List<PFEntityKey>();
        private Dictionary<ulong, string> lobbyUserNameDict = new Dictionary<ulong, string>();

        public static PlayFabRuntimeInfos Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabRuntimeInfos();
                    instance.Initialize();
                }

                return instance;
            }
        }

        public List<string> PersonalNameInLobbyOrGame
        {
            get
            {
                List<string> entitiesCopy = new List<string>(lobbyUserNameDict.Values);
                return entitiesCopy;
            }
        }

        public bool IsSelfLobbyOwner
        {
            get
            {
                return lobbyOwnerEntityKey.Id == MyEntityContext.EntityId;
            }
        }
        
        public Lobby CurrentLobby { get; private set; }
        public string MySteamUserName { get; private set; }
        public string MyPlayerId { get; private set; }
        public PlayFabAuthenticationContext MyEntityContext { get; private set; }

        private void Initialize()
        {
            SessionNetwork.Instance.OnNetworkMessage_HelloNetwork_Received += HandleRemotePlayerJoined;
            SessionNetwork.Instance.OnNetworkMessage_GoodbyeNetwork_Received += HandleRemotePlayerLeft;
        }

        public bool IsPlayerLobbyOwner(ulong entityId)
        {
            return PlayFabCustomUtils.GetUlongIdByPlayFabPlayerEntityId(lobbyOwnerEntityKey.Id) == entityId;
        }

        public void UpdatePlayFabLobbyMemberInfo(Lobby lobby)
        {
            CurrentLobby = lobby;

            entityKeyInLobbyOrGame.Clear();

            foreach (var item in lobby.GetMembers())
            {
                entityKeyInLobbyOrGame.Add(item);
            }

            PFEntityKey entityKey;
            if (lobby.TryGetOwner(out entityKey))
            {
                lobbyOwnerEntityKey = entityKey;
            }
        }

        public void ClearLobbyInfo()
        {
            CurrentLobby = null;

            entityKeyInLobbyOrGame.Clear();
            lobbyUserNameDict.Clear();

            lobbyOwnerEntityKey = null;
        }

        public void SetSelfPlayerInfo(string myPlayerName, PlayFabAuthenticationContext context)
        {
            MyEntityContext = context;
            MyPlayerId = context.PlayFabId;
            MySteamUserName = myPlayerName;
        }

        public void SetSelfPlayerName(string myPlayerName)
        {
            MySteamUserName = myPlayerName;
        }

        public void AddSelfNameToLobby()
        {
            var entityUlongId = PlayFabCustomUtils.GetUlongIdByPlayFabPlayerEntityId(MyEntityContext.EntityId);
            lobbyUserNameDict.Add(entityUlongId, MySteamUserName);
        }

        private void HandleRemotePlayerJoined(ulong entityId, string playerName)
        {
            if (lobbyUserNameDict.ContainsKey(entityId))
            {
                lobbyUserNameDict.Remove(entityId);
            }

            lobbyUserNameDict.Add(entityId, playerName);
            PlayerInfoUpdatedEvent?.Invoke();
        }
        
        private void HandleRemotePlayerLeft(ulong entityId)
        {
            if (lobbyUserNameDict.ContainsKey(entityId))
            {
                lobbyUserNameDict.Remove(entityId);
                PlayerInfoUpdatedEvent?.Invoke();
            }
        }

        public bool IsPlayerInLobbyOrGame(string playerName)
        {
            foreach (var name in lobbyUserNameDict.Values)
            {
                if (name.Equals(playerName))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsMaxPlayerReached()
        {
            return CurrentLobby.GetMembers().Count >= CurrentLobby.MaxMemberCount;
        }

        public Dictionary<ulong, string> GetLobbyPlayerNames()
        {
            return lobbyUserNameDict;
        }

        public ulong GetMyUlongEntityId()
        {
            return PlayFabCustomUtils.GetUlongIdByPlayFabPlayerEntityId(MyEntityContext.EntityId);
        }

        public void OnApplicationQuit()
        {
            entityKeyInLobbyOrGame.Clear();
            lobbyUserNameDict.Clear();
        }
    }
}
