/*
 * PlayFab Unity SDK
 *
 * Copyright (c) Microsoft Corporation
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */

namespace PlayFab.Multiplayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Lobby
    {
        private static Dictionary<IntPtr, Lobby> lobbyCache = new Dictionary<IntPtr, Lobby>();

        internal Lobby(InteropWrapper.PFLobbyHandle lobbyHandle)
        {
            this.Handle = lobbyHandle;
        }

        /// <summary>
        /// Gets the ID of the Lobby.
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> or
        /// <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return null until
        /// <see cref="PlayFabMultiplayer.ProcessingLobbyStateChanges()" /> provides a successful
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> or <see cref="PlayFabMultiplayer.OnLobbyJoinCompletedStateChange" />
        /// respectively.
        /// </remarks>
        /// <returns>
        /// Gets the ID of the Lobby.
        /// </returns>
        public string Id
        {
            get
            {
                string id;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetLobbyId(this.Handle, out id));
                return id;
            }
        }

        /// <summary>
        /// Gets the max member count of the lobby.
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// 0 until <see cref="PlayFabMultiplayer.ProcessingLobbyStateChanges()" /> provides a
        /// <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> with <see cref="LobbyUpdatedStateChange.maxMembersUpdated" /> set to
        /// true. If joining the lobby succeeds, this field is guaranteed to be populated by the time
        /// Multiplayer.ProcessingLobbyStateChanges() provides a <see cref="OnLobbyJoinCompleted" />.
        /// </remarks>
        /// <returns>
        /// The max member count of the lobby.
        /// </returns>
        public uint MaxMemberCount
        {
            get
            {
                uint maxMemberCount;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetMaxMemberCount(this.Handle, out maxMemberCount));
                return maxMemberCount;
            }
        }

        /// Gets the owner migration policy of the lobby.
        /// <summary>
        /// <remarks>
        /// The owner migration policy cannot change for the lifetime of the lobby.
        /// <para>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// LobbyOwnerMigrationPolicy.None until <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a
        /// <see cref="LobbyUpdatedStateChange" /> with <see cref="LobbyUpdatedStateChange.OwnerMigrationPolicy" /> set to
        /// true. If joining the lobby succeeds, this field is guaranteed to be populated by the time
        /// Multiplayer.ProcessLobbyStateChanges() provides a <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" />.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The owner migration policy of the lobby.
        /// </returns>
        public LobbyOwnerMigrationPolicy OwnerMigrationPolicy
        {
            get
            {
                InteropWrapper.PFLobbyOwnerMigrationPolicy ownerMigrationPolicy;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetOwnerMigrationPolicy(this.Handle, out ownerMigrationPolicy));
                return (LobbyOwnerMigrationPolicy)ownerMigrationPolicy;
            }
        }

        /// <summary>
        /// Gets the access policy of the lobby.
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// LobbyAccessPolicy.Public until <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a
        /// <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> with <see cref="PlayFabMultiplayer.OnLobbyUpdated.AccessPolicyUpdated" /> set to
        /// true. If joining the lobby succeeds, this field is guaranteed to be populated by the time
        /// Multiplayer.ProcessLobbyStateChanges() provides a <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" />.
        /// </remarks>
        /// <returns>
        /// The access policy of the lobby.
        /// </returns>
        public LobbyAccessPolicy AccessPolicy
        {
            get
            {
                InteropWrapper.PFLobbyAccessPolicy accessPolicy;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetAccessPolicy(this.Handle, out accessPolicy));
                return (LobbyAccessPolicy)accessPolicy;
            }
        }

        /// <summary>
        /// Gets whether the lobby's membership is locked.
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// false until <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a
        /// <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> with <see cref="PlayFabMultiplayer.OnLobbyUpdated.MembershipLockUpdated" /> set to
        /// true. If joining the lobby succeeds, this field is guaranteed to be populated by the time
        /// PFMultiplayerStartProcessingLobbyStateChanges() provides a <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" />.
        /// </remarks>
        /// <returns>
        /// Whether the lobby's membership is locked.
        /// </returns>
        public LobbyMembershipLock MembershipLock
        {
            get
            {
                InteropWrapper.PFLobbyMembershipLock lockState;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetMembershipLock(this.Handle, out lockState));
                return (LobbyMembershipLock)lockState;
            }
        }

        /// <summary>
        /// Gets the default connection string associated with the lobby.
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" />, this method will
        /// return null until <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a successful
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" />. If this lobby object was created by calling
        /// <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return null until
        /// <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a successful
        /// <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" />.
        /// </remarks>
        /// <returns>
        /// The default connection string associated with the lobby.
        /// </returns>
        public string ConnectionString
        {
            get
            {
                string connectionString;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetConnectionString(this.Handle, out connectionString));
                return connectionString;
            }
        }

        internal InteropWrapper.PFLobbyHandle Handle { get; set; }

        /// <summary>
        /// Tries to get the current owner of the lobby.  Returns false and sets owner to null if there is no owner on this Lobby
        /// <summary>
        /// <remarks>
        /// If this lobby object was created by calling <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return false and 
        /// the owner will be null until <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges()" /> provides a
        /// <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> with <see cref="PlayFabMultiplayer.OnLobbyUpdated.OwnerUpdated" /> set to true. If
        /// joining the lobby succeeds, this field is guaranteed to be populated by the time
        /// PFMultiplayerStartProcessingLobbyStateChanges() provides a <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" />.
        /// </remarks>
        /// <param name="owner">
        /// The output owner. This value may be null if the owner has left or disconnected from the lobby while the owner
        /// migration policy is <see cref="LobbyOwnerMigrationPolicy.Manual" /> or
        /// <see cref="LobbyOwnerMigrationPolicy.None" />.
        /// </param>
        /// <returns>
        /// true if an owner is found or false otherwise
        /// </returns>
        public bool TryGetOwner(out PFEntityKey owner)
        {
            InteropWrapper.PFEntityKey userHandle;
            if (PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetOwner(this.Handle, out userHandle)) && userHandle != null)
            {
                PFEntityKey lobbyUser = new PFEntityKey(userHandle);
                owner = lobbyUser;
                return true;
            }
            else
            {
                owner = null;
                return false;
            }
        }

        /// <summary>
        /// Gets the list of PlayFab entities currently joined to the lobby as members.
        /// <summary>
        /// <remarks>
        /// If this lobby object is still in the process of asynchronously being created or joined, via a call to either
        /// <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> or <see cref="PlayFabMultiplayer.JoinLobby()" /> respectively, this
        /// method will return no members.
        /// </remarks>
        /// <returns>
        /// The list of PlayFab entities currently joined to the lobby as members.
        /// </returns>
        public IList<PFEntityKey> GetMembers()
        {
            List<PFEntityKey> userList = new List<PFEntityKey>();
            InteropWrapper.PFEntityKey[] userHandles;
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetMembers(this.Handle, out userHandles));
            foreach (var userHandle in userHandles)
            {
                PFEntityKey user = new PFEntityKey(userHandle);
                userList.Add(user);
            }

            return userList;
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Request one local user to leave the lobby.
        /// <summary>
        /// <remarks>
        /// This operation should only fail if the client is experiencing persistent internet connectivity issues. Under these
        /// circumstances, the client will lose their active connection to the lobby and remote lobby members will see their
        /// <see cref="LobbyMemberConnectionStatus" /> as <c>LobbyMemberConnectionStatus.Disconnected</c>. The members
        /// experiencing connectivity issues will remain as members of the lobby unless the lobby owner forcibly removes them.
        /// <para>
        /// This is an asynchronous operation. The local users removed from the lobby via this method will not be removed in the
        /// lists returned by <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes and a
        /// <c>Multiplayer.OnLobbyMemberRemoved</c> is provided by
        /// <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges" />.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// A value to indicate if a specific local user should leave the lobby. 
        /// </param>
        public void Leave(PlayFab.PlayFabAuthenticationContext localUser)
        {
            this.Leave(new PFEntityKey(localUser));
        }
#endif

        /// <summary>
        /// Request one local user to leave the lobby.
        /// <summary>
        /// <remarks>
        /// This operation should only fail if the client is experiencing persistent internet connectivity issues. Under these
        /// circumstances, the client will lose their active connection to the lobby and remote lobby members will see their
        /// <see cref="LobbyMemberConnectionStatus" /> as <c>LobbyMemberConnectionStatus.Disconnected</c>. The members
        /// experiencing connectivity issues will remain as members of the lobby unless the lobby owner forcibly removes them.
        /// <para>
        /// This is an asynchronous operation. The local users removed from the lobby via this method will not be removed in the
        /// lists returned by <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes and a
        /// <c>Multiplayer.OnLobbyMemberRemoved</c> is provided by
        /// <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges" />.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// A value to indicate if a specific local user should leave the lobby. 
        /// </param>
        public void Leave(PFEntityKey localUser)
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyLeave(this.Handle, localUser.EntityKey, null));
        }

        /// <summary>
        /// Request all local users to leave the lobby.
        /// <summary>
        /// <remarks>
        /// This operation should only fail if the client is experiencing persistent internet connectivity issues. Under these
        /// circumstances, the client will lose their active connection to the lobby and remote lobby members will see their
        /// <see cref="LobbyMemberConnectionStatus" /> as <c>LobbyMemberConnectionStatus.Disconnected</c>. The members
        /// experiencing connectivity issues will remain as members of the lobby unless the lobby owner forcibly removes them.
        /// <para>
        /// This is an asynchronous operation. The local users removed from the lobby via this method will not be removed in the
        /// lists returned by <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes and a
        /// <c>Multiplayer.OnLobbyMemberRemoved</c> is provided by
        /// <see cref="PlayFabMultiplayer.ProcessLobbyStateChanges" />.
        /// </para>
        /// </remarks>
        public void LeaveAllLocalUsers()
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyLeave(this.Handle, null, null));
        }

        /// <summary>
        /// Get the dictionary of search property keys and values
        /// <summary>
        /// <remarks>
        /// Search properties are visible to non-members of the lobby as metadata which can be used to filter and sort lobby
        /// search results.
        /// <para>
        /// This will construct a new Dictionary upon each API call so it
        /// should not be called with high frequency
        /// </para>
        /// <para>
        /// If this lobby object is still in the process of asynchronously being created or joined via a call to
        /// <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> or <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// no keys.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The dictionary of search property keys and values
        /// </returns>
        public IDictionary<string, string> GetSearchProperties()
        {
            string[] keys;
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetSearchPropertyKeys(this.Handle, out keys));

            string[] values = new string[keys.Length];
            int index = 0;
            foreach (var key in keys)
            {
                string value;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetSearchProperty(this.Handle, key, out value));
                values[index++] = value;
            }

            Dictionary<string, string> properties = Enumerable.Range(0, keys.Length).ToDictionary(
                i => keys[i],
                i => values[i]);

            return properties;
        }

        /// <summary>
        /// Get the dictionary of lobby property keys and values
        /// <summary>
        /// <remarks>
        /// Lobby properties are only visible to members of the lobby.
        /// <para>
        /// This will construct a new Dictionary upon each API call so it
        /// should not be called with high frequency
        /// </para>
        /// <para>
        /// If this lobby object is still in the process of asynchronously being created or joined via a call to
        /// <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> or <see cref="PlayFabMultiplayer.JoinLobby()" />, this method will return
        /// no keys.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The dictionary of lobby property keys and values
        /// </returns>
        public IDictionary<string, string> GetLobbyProperties()
        {
            string[] keys;
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetLobbyPropertyKeys(this.Handle, out keys));

            string[] values = new string[keys.Length];
            int index = 0;
            foreach (var key in keys)
            {
                string value;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetLobbyProperty(this.Handle, key, out value));
                values[index++] = value;
            }

            Dictionary<string, string> properties = Enumerable.Range(0, keys.Length).ToDictionary(
                i => keys[i],
                i => values[i]);

            return properties;
        }

        /// <summary>
        /// Get the dictionary of member property keys and values for a specific member
        /// <summary>
        /// <remarks>
        /// Per-member properties are only visible to members of the lobby.
        /// <para>
        /// This will construct a new Dictionary upon each API call so it
        /// should not be called with high frequency
        /// </para>
        /// <para>
        /// If the member is still in the process of asynchronously joining this lobby either via
        /// <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" />, <see cref="PlayFabMultiplayer.JoinLobby()" />, or
        /// <see cref="Lobby.AddMember" />, this method will return no keys.
        /// </para>
        /// </remarks>
        /// <param name="member">
        /// The member being queried.
        /// </param>
        /// <returns>
        /// The dictionary of member property keys and values for a specific member
        /// </returns>
        public IDictionary<string, string> GetMemberProperties(PFEntityKey member)
        {
            string[] keys;
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetMemberPropertyKeys(this.Handle, member.EntityKey, out keys));

            string[] values = new string[keys.Length];
            int index = 0;
            foreach (var key in keys)
            {
                string value;
                PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyGetMemberProperty(this.Handle, member.EntityKey, key, out value));
                values[index++] = value;
            }

            Dictionary<string, string> properties = Enumerable.Range(0, keys.Length).ToDictionary(
                i => keys[i],
                i => values[i]);

            return properties;
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// <para>  
        /// The <paramref name="lobbyUpdate" /> contains fields that can only be modified by the owner of the lobby. This method
        /// will fail and PlayFabMultiplayer.OnError will be called if one of those fields is specified and <paramref name="localUser" /> 
        /// is not the owner of the lobby.
        /// </para>
        /// <para>
        /// When both a <paramref name="lobbyUpdate" /> and <paramref name="memberUpdate" /> are supplied to this method on behalf
        /// of a single entity, both updates will happen atomically.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="lobbyUpdate">
        /// An update to apply to the shared portion of the lobby on behalf of <paramref name="localUser" />. If this
        /// is not provided, <paramref name="memberUpdate" /> must be provided.
        /// </param>
        /// <param name="memberProperties">
        /// The member properties to update for the updating member.
        /// </param>        
        public void PostUpdate(
            PlayFab.PlayFabAuthenticationContext localUser,
            LobbyDataUpdate lobbyUpdate,
            IDictionary<string, string> memberProperties)
        {
            PlayFabMultiplayer.SetEntityToken(localUser);
            this.PostUpdate(new PFEntityKey(localUser), lobbyUpdate, memberProperties);
        }
#endif

        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// <para>
        /// The <paramref name="lobbyUpdate" /> contains fields that can only be modified by the owner of the lobby. This method
        /// will fail and PlayFabMultiplayer.OnError will be called if one of those fields is specified and <paramref name="localUser" /> is not the owner of the
        /// lobby.
        /// </para>
        /// <para>
        /// When both a <paramref name="lobbyUpdate" /> and <paramref name="memberUpdate" /> are supplied to this method on behalf
        /// of a single entity, both updates will happen atomically.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="lobbyUpdate">
        /// An update to apply to the shared portion of the lobby on behalf of <paramref name="localUser" />. If this
        /// is not provided, <paramref name="memberUpdate" /> must be provided.
        /// </param>
        /// <param name="memberProperties">
        /// The member properties to update for the updating member.
        /// </param>        
        public void PostUpdate(
            PFEntityKey localUser,
            LobbyDataUpdate lobbyUpdate,
            IDictionary<string, string> memberProperties)
        {
            InteropWrapper.PFLobbyMemberDataUpdate memberDataUpdate = new InteropWrapper.PFLobbyMemberDataUpdate(memberProperties);
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyPostUpdate(this.Handle, localUser.EntityKey, lobbyUpdate.Update, memberDataUpdate, null));
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// <para>
        /// The <paramref name="lobbyUpdate" /> contains fields that can only be modified by the owner of the lobby. This method
        /// will fail and PlayFabMultiplayer.OnError will be called if one of those fields is specified and <paramref name="localUser" /> is not the owner of the
        /// lobby.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="lobbyUpdate">
        /// An optional update to apply to the shared portion of the lobby on behalf of <paramref name="localUser" />. If this
        /// is not provided, <paramref name="memberUpdate" /> must be provided.
        /// </param>
        public void PostUpdate(
            PlayFab.PlayFabAuthenticationContext localUser,
            LobbyDataUpdate lobbyUpdate)
        {
            PlayFabMultiplayer.SetEntityToken(localUser);
            this.PostUpdate(new PFEntityKey(localUser), lobbyUpdate);
        }
#endif

        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// <para>
        /// The <paramref name="lobbyUpdate" /> contains fields that can only be modified by the owner of the lobby. This method
        /// will fail and PlayFabMultiplayer.OnError will be called if one of those fields is specified and <paramref name="localUser" /> is not the owner of the
        /// lobby.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="lobbyUpdate">
        /// An optional update to apply to the shared portion of the lobby on behalf of <paramref name="localUser" />. If this
        /// is not provided, <paramref name="memberUpdate" /> must be provided.
        /// </param>
        public void PostUpdate(
            PFEntityKey localUser,
            LobbyDataUpdate lobbyUpdate)
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyPostUpdate(this.Handle, localUser.EntityKey, lobbyUpdate.Update, null, null));
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="memberProperties">
        /// The member properties to update for the updating member.
        /// </param>        
        public void PostUpdate(
            PlayFab.PlayFabAuthenticationContext localUser,
            IDictionary<string, string> memberProperties)
        {
            PlayFabMultiplayer.SetEntityToken(localUser);
            this.PostUpdate(new PFEntityKey(localUser), memberProperties);
        }
#endif

        /// <summary>
        /// Post an update to the lobby.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnPostUpdateCompleted" /> with the
        /// <see cref="LobbyPostUpdateCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyPostUpdateCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.LobbyPostUpdateCompletedStateChange.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />. If applying the update would change the state of the lobby, the title will
        /// be provided a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> sometime afterwards.
        /// <para>
        /// This operation completing successfully only indicates that the Lobby service has accepted the update. The title's
        /// local view of the Lobby state will not reflect this update until a <see cref="PlayFabMultiplayer.OnLobbyUpdated" /> is
        /// provided to the title with the updated state.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user posting the update.
        /// </param>
        /// <param name="memberProperties">
        /// The member properties to update for the updating member.
        /// </param>        
        public void PostUpdate(
            PFEntityKey localUser,
            IDictionary<string, string> memberProperties)
        {
            InteropWrapper.PFLobbyMemberDataUpdate memberDataUpdate = new InteropWrapper.PFLobbyMemberDataUpdate(memberProperties);
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyPostUpdate(this.Handle, localUser.EntityKey, null, memberDataUpdate, null));
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Send an invite to this lobby from the local user to the invited entity.
        /// </summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// The <paramref name="sender" /> must be a local user of this lobby which joined from this client.
        /// </para>
        /// </remarks>
        /// <param name="sender">
        /// The local user sending the invite.
        /// </param>
        /// <param name="invitee">
        /// The invited entity.
        /// </param>
        public void SendInvite(
            PlayFab.PlayFabAuthenticationContext sender,
            PFEntityKey invitee)
        {
            PlayFabMultiplayer.SetEntityToken(sender);
            this.SendInvite(new PFEntityKey(sender), invitee);
        }
#endif

        /// <summary>
        /// Send an invite to this lobby from the local user to the invited entity.
        /// </summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbySendInviteCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// The <paramref name="sender" /> must be a local user of this lobby which joined from this client.
        /// </para>
        /// </remarks>
        /// <param name="sender">
        /// The local user sending the invite.
        /// </param>
        /// <param name="invitee">
        /// The invited entity.
        /// </param>
        public void SendInvite(
            PFEntityKey sender,
            PFEntityKey invitee)
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbySendInvite(this.Handle, sender.EntityKey, invitee.EntityKey, null));
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Add a local user as a member to the lobby.
        /// </summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event followed by a <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event with
        /// the result field set to Succeeded (0). Upon a failed completion,
        /// the result field will be set to Failed (negative value).
        /// <para>
        /// This method is used to add an additional local PlayFab entity to a pre-existing lobby object. Because the lobby
        /// object, must have already been created either via a call to <see cref="PlayFabMultiplayer.CreateAndJoinLobby" /> or
        /// <see cref="PlayFabMultiplayer.JoinLobby" />, this method is primarily useful for multiple local user scenarios.
        /// </para>
        /// <para>
        /// This is an asynchronous operation. The member added via this method will not be reflected in the lists returned by
        /// <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The PlayFab Entity Key of the local user to add to the lobby as a member.
        /// </param>
        /// <param name="memberProperties">
        /// The initial member properties to set for this user when they join the lobby.
        /// </param>
        public void AddMember(
           PlayFab.PlayFabAuthenticationContext localUser,
           IDictionary<string, string> memberProperties)
        {
            PlayFabMultiplayer.SetEntityToken(localUser);
            this.AddMember(new PFEntityKey(localUser), memberProperties);
        }
#endif

        /// <summary>
        /// Add a local user as a member to the lobby.
        /// </summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event followed by a <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event with
        /// the result field set to Succeeded (0). Upon a failed completion,
        /// the result field will be set to Failed (negative value).
        /// <para>
        /// This method is used to add an additional local PlayFab entity to a pre-existing lobby object. Because the lobby
        /// object, must have already been created either via a call to <see cref="PlayFabMultiplayer.CreateAndJoinLobby" /> or
        /// <see cref="PlayFabMultiplayer.JoinLobby" />, this method is primarily useful for multiple local user scenarios.
        /// </para>
        /// <para>
        /// This is an asynchronous operation. The member added via this method will not be reflected in the lists returned by
        /// <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The PlayFab Entity Key of the local user to add to the lobby as a member.
        /// </param>
        /// <param name="memberProperties">
        /// The initial member properties to set for this user when they join the lobby.
        /// </param>
        public void AddMember(
           PFEntityKey localUser,
           IDictionary<string, string> memberProperties)
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyAddMember(this.Handle, localUser.EntityKey, memberProperties, null));
        }

        /// <summary>
        /// Forcibly remove an entity from the lobby.
        /// </summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberRemoved" /> event followed by a
        /// <see cref="PlayFabMultiplayer.OnForceRemoveMemberCompleted" /> event with the result field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnForceRemoveMemberCompleted" /> event with the result field set to a failure
        /// <para>
        /// One of the local PlayFab entities present in this lobby must be the owner for this operation to succeed. If the local
        /// owning entity who initiated this operation loses their ownership status while the operation is in progress, the
        /// operation will fail asynchronously and the provided
        /// <see cref="PlayFabMultiplayer.OnForceRemoveMemberCompleted" /> event result which will be set to
        /// <see cref="LobbyStateChangeResult.UserNotAuthorized" />
        /// </para>
        /// <para>
        /// This is an asynchronous operation. The member removed via this method will not be removed from the lists returned by
        /// <see cref="Lobby.GetMembers" /> until the asynchronous operation successfully completes and a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberRemoved" /> event 
        /// </para>
        /// </remarks>
        /// <param name="targetMember">
        /// The member to forcibly remove.
        /// </param>
        /// <param name="preventRejoin">
        /// A flag indicating whether <paramref name="targetMember" /> will be prevented from rejoining the lobby after being
        /// removed.
        /// </param>
        public void ForceRemoveMember(
            PFEntityKey targetMember,
            bool preventRejoin)
        {
            PlayFabMultiplayer.Succeeded(InteropWrapper.PFMultiplayer.PFLobbyForceRemoveMember(this.Handle, targetMember.EntityKey, preventRejoin, null));
        }

        internal static Lobby GetLobbyUsingCache(InteropWrapper.PFLobbyHandle handle)
        {
            Lobby ticket;
            bool found = lobbyCache.TryGetValue(handle.InteropHandleIntPtr, out ticket);
            if (found)
            {
                return ticket;
            }
            else
            {
                ticket = new Lobby(handle);
                lobbyCache[handle.InteropHandleIntPtr] = ticket;
                return ticket;
            }
        }

        internal static void ClearLobbyFromCache(InteropWrapper.PFLobbyHandle handle)
        {
            if (lobbyCache.ContainsKey(handle.InteropHandleIntPtr))
            {
                lobbyCache.Remove(handle.InteropHandleIntPtr);
            }
        }
    }
}
