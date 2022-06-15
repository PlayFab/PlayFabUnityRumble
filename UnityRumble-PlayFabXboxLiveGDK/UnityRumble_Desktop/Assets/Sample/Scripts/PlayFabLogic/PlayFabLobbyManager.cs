//--------------------------------------------------------------------------------------
// PlayFabLobbyManager.cs
//
// Manage messages and functions in PlayFab lobby.
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
using PlayFab.Multiplayer;

namespace Custom_PlayFab
{
    public class PlayFabLobbyManager
    {
        // A SearchProperties key
        public string PFLOBBY_SEARCH_KEY_LOBBY_NAME = "string_key1";
        // max number of people in the lobby
        public const int MAX_PLAYER_COUNT_IN_LOBBY = 16;
        // Event triggered when Create and join lobby completed
        public event Action<string, Lobby> OnCreateAndJoinCompletedEvent;
        // Event triggered when create and join lobby failure
        public event Action<string> OnCreateAndJoinFailureEvent;
        // Event triggered when a client has disconnected from a lobby
        public event Action OnLobbyDisconnectedEvent;
        // Event triggered when successfully joined the lobby
        public event Action<string, Lobby> OnLobbyJoinCompletedEvent;
        // Event triggered when joining lobby failure
        public event Action<string> OnLobbyJoinFailureEvent;
        // Event triggered when found all lobby successfully
        public event Action<IList<LobbySearchResult>> OnLobbyFindCompletedEvent;
        // Event triggered when found all lobby failure
        public event Action<string> OnLobbyFindFailureEvent;
        // Event triggered when self joined lobby
        public event Action AddSelfToLobbyEvent;
        // Event triggered when the lobby player exits
        public event Action<ulong> OnLobbyMemberRemoveEvent;
        // Event triggered when the local player leaves the lobby to completed
        public event Action OnLobbyLeaveCompletedEvent;
        // Event triggered when the lobby information is updated
        public event Action OnLobbyUpdateEvent;
        // Event triggered when join lobby is completed after match
        public event Action OnJoinMatchmakingLobbyCompletedEvent;
        // Event triggered when PlayFabMultiplayer notifies about an error
        public event Action<int, string> OnErrorEvent;
        // After a player's matchmaking is successful, whether to join the match lobby
        private bool isJoinedMatchmakingLobby = false;

        private readonly string arrangedLobbyUpdateNetworkKey = "networkId";
        private static PlayFabLobbyManager instance;
        public static PlayFabLobbyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabLobbyManager();
                    instance.Initialize();
                }
                return instance;
            }
        }

        private PlayFabLobbyManager()
        {
        }

        private void Initialize()
        {
            PlayFabMultiplayer.OnLobbyMemberAdded += this.OnSelfAddCompletedCallback;
            PlayFabMultiplayer.OnLobbyMemberRemoved += this.OnMemberRemoveCompletedCallback;
            PlayFabMultiplayer.OnLobbyUpdated += this.OnLobbyUpdateCallback;
            PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted += this.OnCreateAndJoinLobbyCallback;
            PlayFabMultiplayer.OnLobbyDisconnected += this.OnLobbyDisconnectedCallback;
            PlayFabMultiplayer.OnLobbyJoinCompleted += this.OnJoinedLobbyCallback;
            PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted += OnJoinMatchmakingLobbyCompletedCallback;
            PlayFabMultiplayer.OnLobbyLeaveCompleted += this.OnLobbyLeaveCompletedCallback;
            PlayFabMultiplayer.OnLobbyFindLobbiesCompleted += this.OnFindLobbiesCallback;
            PlayFabMultiplayer.OnError += OnError;
        }

        public void CreateAndJoinLobby(string networkId)
        {
            PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
            var createConfig = new LobbyCreateConfiguration()
            {
                MaxMemberCount = MAX_PLAYER_COUNT_IN_LOBBY,
                OwnerMigrationPolicy = LobbyOwnerMigrationPolicy.Automatic,
                AccessPolicy = LobbyAccessPolicy.Public
            };

            // A SearchProperties key must be in "string_key1,string_key2" format
            createConfig.SearchProperties.Add(PFLOBBY_SEARCH_KEY_LOBBY_NAME, string.Format("PMLP_UNITY_LOBBY_{0}", UnityEngine.Random.Range(0, 1000)));
            createConfig.LobbyProperties.Add("NETWORKID", networkId);
            Debug.Log(networkId);
            var joinConfig = new LobbyJoinConfiguration();

            PlayFabMultiplayer.CreateAndJoinLobby(
                PlayFabRuntimeInfos.Instance.MyEntityContext,
                createConfig,
                joinConfig);
        }

        private void OnCreateAndJoinLobbyCallback(Lobby lobby, int result)
        {
            if (LobbyError.SUCCEEDED(result))
            {
                // Lobby was successfully created
                Debug.Log("Create and join lobby succeeded, lobby connection string:" + lobby.ConnectionString);
                OnCreateAndJoinCompletedEvent?.Invoke(lobby.ConnectionString, lobby);
                StoreMemberEntityIdAndFireMemberChangedEvent(lobby);
            }
            else
            {
                // Error creating a lobby
                Debug.Log("Error creating a lobby");
                OnCreateAndJoinFailureEvent?.Invoke("Error creating a lobby");
            }
        }

        private void OnLobbyDisconnectedCallback(Lobby lobby)
        {
            // Disconnected from lobby
            Debug.Log("Disconnected from lobby!");
            OnLobbyDisconnectedEvent?.Invoke();
        }

        public void JoinLobby(string connectionString)
        {
            PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
            var joinConfig = new LobbyJoinConfiguration();
            PlayFabMultiplayer.JoinLobby(
                    PlayFabRuntimeInfos.Instance.MyEntityContext,
                    connectionString,
                    null);
        }

        private void OnJoinedLobbyCallback(Lobby lobby, PFEntityKey newMember, int result)
        {
            if (LobbyError.SUCCEEDED(result))
            {
                // Successfully joined a lobby
                Debug.Log("Joined a lobby, MaxMemberCount is: " + lobby.MaxMemberCount + ",current member count is:  " + lobby.GetMembers().Count);
                StoreMemberEntityIdAndFireMemberChangedEvent(lobby);
                SessionNetwork.Instance.OnNetworkJoined += this.OnNetworkJoined;
                SessionNetwork.Instance.JoinNetwork(lobby.GetLobbyProperties()["NETWORKID"]);
            }
            else
            {
                // Error joining a lobby
                Debug.Log("Error joining a lobby");
                OnLobbyJoinFailureEvent?.Invoke("Error joining a lobby: " + LobbyError.FAILED(result));
            }
        }

        private void OnNetworkJoined(string networkId)
        {
            SessionNetwork.Instance.OnNetworkJoined -= this.OnNetworkJoined;
            OnLobbyJoinCompletedEvent?.Invoke(PlayFabRuntimeInfos.Instance.CurrentLobby.ConnectionString, PlayFabRuntimeInfos.Instance.CurrentLobby);
        }

        public void JoinMatchmakingLobby(string lobbyArrangementString)
        {
            PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
            isJoinedMatchmakingLobby = true;
            var arrangedJoinConfig = new LobbyArrangedJoinConfiguration
            {
                MaxMemberCount = PlayFabLobbyManager.MAX_PLAYER_COUNT_IN_LOBBY,
                AccessPolicy = LobbyAccessPolicy.Private,
                OwnerMigrationPolicy = LobbyOwnerMigrationPolicy.Automatic
            };

            PlayFabMultiplayer.JoinArrangedLobby(PlayFabRuntimeInfos.Instance.MyEntityContext, lobbyArrangementString, arrangedJoinConfig);
        }

        private void OnJoinMatchmakingLobbyCompletedCallback(Lobby lobby, PFEntityKey entityKey, int result)
        {
            if (LobbyError.SUCCEEDED(result))
            {
                OnJoinMatchmakingLobbyCompletedEvent?.Invoke();
                StoreMemberEntityIdAndFireMemberChangedEvent(lobby);
            }
        }

        public void LeaveLobby()
        {
            SessionNetwork.Instance.OnNetworkStopped += this.OnNetworkStoppedCallback;
            SessionNetwork.Instance.StopNetworking();
        }

        private void OnNetworkStoppedCallback(string obj)
        {
            SessionNetwork.Instance.OnNetworkStopped -= OnNetworkStoppedCallback;

            var currentLobby = PlayFabRuntimeInfos.Instance.CurrentLobby;
            if (currentLobby != null)
            {
                if (PlayFabRuntimeInfos.Instance.CurrentLobby.GetMembers().Count == 0)
                {
                    SetLobbyAccessToPrivate();
                }
                currentLobby.Leave(PlayFabRuntimeInfos.Instance.MyEntityContext);
            }
        }

        public void LeaveLobbyOnApplicationQuit()
        {
            PlayFabRuntimeInfos.Instance.CurrentLobby?.Leave(PlayFabRuntimeInfos.Instance.MyEntityContext);
        }

        private void OnLobbyLeaveCompletedCallback(Lobby lobby, PFEntityKey localUser)
        {
            Debug.Log("Lobby leave completed");
            OnLobbyLeaveCompletedEvent?.Invoke();
            PlayFabRuntimeInfos.Instance.ClearLobbyInfo();
        }

        // When a lobby member is added
        private void OnSelfAddCompletedCallback(Lobby lobby, PFEntityKey addedMemberKey)
        {
            Debug.LogFormat("Member {0} is added to lobby {1}", addedMemberKey.Id, lobby.ConnectionString);
            PlayFabRuntimeInfos.Instance.UpdatePlayFabLobbyMemberInfo(lobby);
            if (!isJoinedMatchmakingLobby || addedMemberKey.Id != PlayFabRuntimeInfos.Instance.MyEntityContext.EntityId)
            {
                return;
            }

            if (PlayFabRuntimeInfos.Instance.IsSelfLobbyOwner)
            {
                // lobby owner creates a network
                SessionNetwork.Instance.OnNetworkJoined += OnMatchmakingNetworkStarted;
                SessionNetwork.Instance.TryCreateNetworkAndGetNetworkId();
                isJoinedMatchmakingLobby = false;
            }
            else
            {
                // If a player isn't lobby owner and lobby properties contain network key then join the network, otherwise wait for the owner to create a network and update lobby properties
                if (lobby.GetLobbyProperties().ContainsKey(arrangedLobbyUpdateNetworkKey))
                {
                    SessionNetwork.Instance.OnNetworkJoined += OnArrangedLobbyNetworkJoinedCallback;
                    SessionNetwork.Instance.JoinNetwork(lobby.GetLobbyProperties()[arrangedLobbyUpdateNetworkKey]);
                }
            }
        }

        private void OnMatchmakingNetworkStarted(string networkingId)
        {
            SessionNetwork.Instance.OnNetworkJoined -= OnMatchmakingNetworkStarted;

            var entityKey = new PFEntityKey(PlayFabRuntimeInfos.Instance.MyEntityContext);

            PlayFabRuntimeInfos.Instance.CurrentLobby.PostUpdate(entityKey,
                new LobbyDataUpdate
                {
                    LobbyProperties = new Dictionary<string, string>
                    {
                        { arrangedLobbyUpdateNetworkKey, networkingId }
                    }
                });
        }

        // When a member leaves lobby
        private void OnMemberRemoveCompletedCallback(Lobby lobby, PFEntityKey removedMemberKey, LobbyMemberRemovedReason reason)
        {
            Debug.LogFormat("Member {0} is removed from lobby {1}", removedMemberKey.Id, lobby.Id);
            PlayFabRuntimeInfos.Instance.UpdatePlayFabLobbyMemberInfo(lobby);
            OnLobbyMemberRemoveEvent?.Invoke(PlayFabCustomUtils.GetUlongIdByPlayFabPlayerEntityId(removedMemberKey.Id));
        }

        // Lobby Info Changed
        private void OnLobbyUpdateCallback(Lobby lobby,
            bool ownerUpdated,
            bool maxMembersUpdated,
            bool accessPolicyUpdated,
            bool membershipLockUpdated,
            IList<string> updatedSearchPropertyKeys,
            IList<string> updatedLobbyPropertyKeys,
            IList<LobbyMemberUpdateSummary> memberUpdates)
        {
            OnLobbyUpdateEvent?.Invoke();

            // When updating networking info and self is not lobby's owner then join the session network
            if (updatedLobbyPropertyKeys.Contains(arrangedLobbyUpdateNetworkKey))
            {
                if (SessionNetwork.Instance.CurrentNetworkState != SessionNetwork.NetworkState.NoNetwork)
                {
                    return;
                }

                PFEntityKey ownerEntityKey;
                if (lobby.TryGetOwner(out ownerEntityKey) && ownerEntityKey != null && PlayFabRuntimeInfos.Instance.MyEntityContext.EntityId != ownerEntityKey.Id)
                {
                    SessionNetwork.Instance.OnNetworkJoined += this.OnArrangedLobbyNetworkJoinedCallback;
                    SessionNetwork.Instance.JoinNetwork(lobby.GetLobbyProperties()[arrangedLobbyUpdateNetworkKey]);
                }
            }
        }

        private void OnArrangedLobbyNetworkJoinedCallback(string networkId)
        {
            SessionNetwork.Instance.OnNetworkJoined -= OnArrangedLobbyNetworkJoinedCallback;

            OnJoinMatchmakingLobbyCompletedEvent?.Invoke();
        }

        public void FindLobbyList(string filterString = null)
        {
            LobbySearchConfiguration config = new LobbySearchConfiguration();
            if (filterString != null)
            {
                config.FilterString = string.Format("string_key1 eq '{0}'", filterString);
            }
            PlayFabMultiplayer.FindLobbies(PlayFabRuntimeInfos.Instance.MyEntityContext, config);
        }

        private void OnFindLobbiesCallback(IList<LobbySearchResult> searchResults, PFEntityKey ownerMember, int reason)
        {
            if (LobbyError.SUCCEEDED(reason))
            {
                foreach (var item in searchResults)
                {
                    Debug.Log("Found LobbyObject: " + item.MaxMemberCount + "======" + item.CurrentMemberCount + "======" + item.SearchProperties.Count);
                }
                // Successfully found lobbies
                Debug.Log("Found lobbies, lobbies length is :" + searchResults.Count + "-----" + ownerMember.Id);
                OnLobbyFindCompletedEvent?.Invoke(searchResults);
            }
            else
            {
                // Error finding lobbies
                Debug.Log("Error finding lobbies");
                OnLobbyFindFailureEvent?.Invoke("Error finding lobbies: " + LobbyError.FAILED(reason));
            }
        }

        public ulong[] GetLobbyMemberInfos(Lobby lobby)
        {
            IList<PFEntityKey> EntityKeys = lobby.GetMembers();
            ulong[] lobbyMemberInfos = new ulong[EntityKeys.Count];
            for (int i = 0; i < EntityKeys.Count; i++)
            {
                lobbyMemberInfos[i] = PlayFabCustomUtils.GetUlongIdByPlayFabPlayerEntityId(EntityKeys[i].Id);
            }
            return lobbyMemberInfos;
        }

        // called when self joined lobby
        public void StoreMemberEntityIdAndFireMemberChangedEvent(Lobby lobby)
        {
            PlayFabRuntimeInfos.Instance.UpdatePlayFabLobbyMemberInfo(lobby);
            PlayFabRuntimeInfos.Instance.AddSelfNameToLobby();
            AddSelfToLobbyEvent?.Invoke();
        }

        public void SetLobbyAccessToPrivate()
        {
            if (!PlayFabRuntimeInfos.Instance.IsSelfLobbyOwner)
            {
                return;
            }

            var entityKey = new PFEntityKey(PlayFabRuntimeInfos.Instance.MyEntityContext);

            PlayFabRuntimeInfos.Instance.CurrentLobby.PostUpdate(entityKey,
                new LobbyDataUpdate
                {
                    AccessPolicy = LobbyAccessPolicy.Private
                });
        }

        private void OnError(PlayFabMultiplayerErrorArgs args)
        {
            if (args.Message.Contains("Unmapped"))
            {
                return;
            }
            Debug.Log("PlayFabMultiplayer error");
            OnErrorEvent?.Invoke(args.Code, args.Message);
        }

        public void RemoveEventListeners()
        {
            PlayFabMultiplayer.OnLobbyMemberAdded -= this.OnSelfAddCompletedCallback;
            PlayFabMultiplayer.OnLobbyMemberRemoved -= this.OnMemberRemoveCompletedCallback;
            PlayFabMultiplayer.OnLobbyUpdated -= this.OnLobbyUpdateCallback;
            PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted -= this.OnCreateAndJoinLobbyCallback;
            PlayFabMultiplayer.OnLobbyDisconnected -= this.OnLobbyDisconnectedCallback;
            PlayFabMultiplayer.OnLobbyJoinCompleted -= this.OnJoinedLobbyCallback;
            PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted -= OnJoinMatchmakingLobbyCompletedCallback;
            PlayFabMultiplayer.OnLobbyLeaveCompleted -= this.OnLobbyLeaveCompletedCallback;
            PlayFabMultiplayer.OnLobbyFindLobbiesCompleted -= this.OnFindLobbiesCallback;
            PlayFabMultiplayer.OnError -= OnError;
        }
    }
}
