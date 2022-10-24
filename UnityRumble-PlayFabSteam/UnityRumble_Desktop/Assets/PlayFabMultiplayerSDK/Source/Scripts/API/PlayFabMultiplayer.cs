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
    using System.Diagnostics;
    using System.Linq;
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.Compilation;
#endif
#if UNITY_2017_1_OR_NEWER
    using UnityEngine;
#endif

    public class PlayFabMultiplayer
    {
        /// <summary>
        /// The minimum allowed value for <see cref="CreateAndJoinLobbyConfiguration.MaxMemberCount" /> and
        /// <see cref="LobbyDataUpdate.MaxMemberCount" />.
        /// </summary>
        public const uint LobbyMaxMemberCountLowerLimit = InteropWrapper.PFMultiplayer.PFLobbyMaxMemberCountLowerLimit;

        /// <summary>
        /// The  maximum allowed value for <see cref="CreateAndJoinLobbyConfiguration.MaxMemberCount" /> and
        /// <see cref="LobbyDataUpdate.MaxMemberCount" />.
        /// </summary>
        public const uint LobbyMaxMemberCountUpperLimit = InteropWrapper.PFMultiplayer.PFLobbyMaxMemberCountUpperLimit;

        /// <summary>
        /// The maximum number of concurrent search properties which can be stored for the lobby.
        /// </summary>
        public const uint LobbyMaxSearchPropertyCount = InteropWrapper.PFMultiplayer.PFLobbyMaxSearchPropertyCount;

        /// <summary>
        /// The maximum number of concurrent properties which can be stored for the lobby and which aren't owned by any specific
        /// member.
        /// </summary>
        public const uint LobbyMaxLobbyPropertyCount = InteropWrapper.PFMultiplayer.PFLobbyMaxLobbyPropertyCount;

        /// <summary>
        /// The maximum number of concurrent properties allowed for each member in the lobby.
        /// </summary>
        public const uint LobbyMaxMemberPropertyCount = InteropWrapper.PFMultiplayer.PFLobbyMaxMemberPropertyCount;

        /// <summary>
        /// The maximum number of search results that client-entity callers may request when performing a
        /// <see cref="PlayFabMultiplayer.FindLobbies" /> operation.
        /// </summary>
        public const uint LobbyClientRequestedSearchResultCountUpperLimit = InteropWrapper.PFMultiplayer.PFLobbyClientRequestedSearchResultCountUpperLimit;

        private static PFMultiplayerInitStatus initStatus = PFMultiplayerInitStatus.Uninitialized;

        private static InteropWrapper.PFMultiplayerHandle multiplayerHandle;

        private static LogLevelType logLevel;

        private static InteropWrapper.LobbyStateChangeCollection lobbyStateChanges;

        private static InteropWrapper.MatchmakingStateChangeCollection matchmakingStateChanges;

        /// <summary>
        /// Handler for when there is an error calling another API to be used for debugging purposes. 
        /// </summary>
        /// <param name="args">
        /// The error args containing the error message and error code
        /// </param>
        public delegate void OnErrorEventHandler(PlayFabMultiplayerErrorArgs args);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> completed.
        /// </summary>
        /// <param name="lobby">
        /// The lobby that was created and joined.
        /// </param>
        /// <param name="result">
        /// Indicates that the CreateAndJoinLobby operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbyCreateAndJoinCompletedHandler(Lobby lobby, int result);

        /// <summary>
        /// Handler for when the client has disconnected from a lobby.
        /// </summary>
        /// <param name="lobby">
        /// The lobby that was disconnected.
        /// </param>
        public delegate void OnLobbyDisconnectedHandler(Lobby lobby);

        /// <summary>
        /// Handler for when a local PlayFab entity was added to lobby as a member.
        /// </summary>
        /// <remarks>
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event fires anytime any member is added to the lobby (remote or local).
        /// <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event only fires when you invoke <see cref="PlayFabMultiplayer.AddMember" /> which allows you to add additional members to the lobby.
        /// The first local member was the one who created the lobby, it will fire the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinArrangedLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted" /> event.
        /// </remarks>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="member">
        /// The PlayFab entity which is now a member of the lobby.
        /// </param>
        public delegate void OnLobbyMemberAddedHandler(Lobby lobby, PFEntityKey member);

        /// <summary>
        /// Handler for when a PlayFab entity was removed from a lobby as a member.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="member">
        /// The member entity which has been removed from the lobby.
        /// </param>
        /// <param name="reason">
        /// The reason <c>member</c> was removed from the lobby.
        /// </param>
        public delegate void OnLobbyMemberRemovedHandler(Lobby lobby, PFEntityKey member, LobbyMemberRemovedReason reason);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="Lobby.AddMember" /> completed
        /// </summary>
        /// <remarks>
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event fires anytime any member is added to the lobby (remote or local).
        /// <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event only fires when you invoke <see cref="PlayFabMultiplayer.AddMember" /> which allows you to add additional members to the lobby.
        /// The first local member was the one who created the lobby, it will fire the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinArrangedLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted" /> event.
        /// </remarks>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="localUser">
        /// The member entity which has added to the lobby.
        /// </param>
        /// <param name="result">
        /// Indicates that the AddMember operation succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnAddMemberCompletedHandler(Lobby lobby, PFEntityKey localUser, int result);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabLobby.ForceRemoveMember()" /> completed.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="targetMember">
        /// The member entity which is the target to the force remove.
        /// </param>
        /// <param name="result">
        /// Indicates that the ForceRemoveMember operation succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnForceRemoveMemberCompletedHandler(Lobby lobby, PFEntityKey targetMember, int result);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabMultiplayer.JoinLobby()" /> completed.
        /// </summary>
        /// <remarks>
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event fires anytime any member is added to the lobby (remote or local).
        /// <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event only fires when you invoke <see cref="PlayFabMultiplayer.AddMember" /> which allows you to add additional members to the lobby.
        /// The first local member was the one who created the lobby, it will fire the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinArrangedLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted" /> event.
        /// </remarks>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="newMember">
        /// The local member entity provided to the call associated with this state change which is joining this lobby.
        /// </param>
        /// <param name="result">
        /// Indicates that the JoinArrangedLobby operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbyJoinCompletedHandler(Lobby lobby, PFEntityKey newMember, int result);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="Lobby.Leave()" /> completed.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="localUser">
        /// The local user provided to the call associated with this state change. May be null.
        /// If this value is null it signifies that the title requested all local members leave the specified lobby.
        /// </param>
        public delegate void OnLobbyLeaveCompletedHandler(Lobby lobby, PFEntityKey localUser);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="Lobby.PostUpdate()" /> completed.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="localUser">
        /// The local user provided to the call associated with this state change.
        /// </param>
        /// <param name="result">
        /// Indicates that the update operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbyPostUpdateCompletedHandler(Lobby lobby, PFEntityKey localUser, int result);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabMultiplayer.JoinArrangedLobby()" /> completed.
        /// </summary>
        /// <remarks>
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> event fires anytime any member is added to the lobby (remote or local).
        /// <see cref="PlayFabMultiplayer.OnAddMemberCompleted" /> event only fires when you invoke <see cref="PlayFabMultiplayer.AddMember" /> which allows you to add additional members to the lobby.
        /// The first local member was the one who created the lobby, it will fire the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinCompleted" /> event.
        /// If the local member is using <see cref="PlayFabMultiplayer.JoinArrangedLobby" />, it will fire the <see cref="PlayFabMultiplayer.OnLobbyJoinArrangedLobbyCompleted" /> event.
        /// </remarks>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="newMember">
        /// The local member entity provided to the call associated with this state change which is joining this lobby.
        /// </param>
        /// <param name="result">
        /// Indicates that the JoinArrangedLobby operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbyJoinArrangedLobbyCompletedHandler(Lobby lobby, PFEntityKey newMember, int result);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabMultiplayer.FindLobbies()" /> completed.
        /// </summary>
        /// <param name="searchResults">
        /// The results returned by the search operation.
        /// </param>
        /// <param name="searchingEntity">
        /// The entity provided to the call associated with this state change.
        /// </param>
        /// <param name="result">
        /// Indicates that the search lobbies operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbyFindLobbiesCompletedHandler(IList<LobbySearchResult> searchResults, PFEntityKey searchingEntity, int result);

        /// <summary>
        /// Handler for when a lobby was updated.
        /// <para/>
        /// This state change signifies that the lobby has updated and provides hints as to which values have changed. Multiple
        /// updates may be provided by a single call to <see cref="ProcessingLobbyStateChanges()" />. All
        /// state reflected by these updates will become available simultaneously when ProcessingLobbyStateChanges() is
        /// called, so the updates can be reconciled either individually or as a batch.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="ownerUpdated">
        /// A flag indicating if the lobby's owner was updated.
        /// </param>
        /// <param name="maxMembersUpdated">
        /// A flag indicating if the maximum number of members allowed in the lobby has been updated.
        /// </param>
        /// <param name="accessPolicyUpdated">
        /// A flag indicating if the lobby's access policy was updated.
        /// </param>
        /// <param name="membershipLockUpdated">
        /// A flag indicating if the lobby's membership lock has updated.
        /// </param>
        /// <param name="updatedSearchPropertyKeys">
        /// The keys of the search properties which have been updated.
        /// </param>
        /// <param name="updatedLobbyPropertyKeys">
        /// The keys of the lobby properties which have been updated.
        /// </param>
        /// <param name="memberUpdates">
        /// The set of member updates.
        /// </param>
        public delegate void OnLobbyUpdatedHandler(
            Lobby lobby,
            bool ownerUpdated,
            bool maxMembersUpdated,
            bool accessPolicyUpdated,
            bool membershipLockUpdated,
            IList<string> updatedSearchPropertyKeys,
            IList<string> updatedLobbyPropertyKeys,
            IList<LobbyMemberUpdateSummary> memberUpdates);

        /// <summary>
        /// Handler for when the operation started by a previous call to <see cref="PlayFabMultiplayer.SendInvite()" /> completed.
        /// </summary>
        /// <param name="lobby">
        /// The lobby involved with the operation.
        /// </param>
        /// <param name="sender">
        /// The local user which attempted to send the invite.
        /// </param>
        /// <param name="invitee">
        /// The entity which was invited.
        /// </param>
        /// <param name="result">
        /// Indicates that the SendInvite operation Succeeded or provides the reason that it failed.
        /// </param>
        public delegate void OnLobbySendInviteCompletedHandler(Lobby lobby, PFEntityKey sender, PFEntityKey invitee, int result);

        /// <summary>
        /// Handler for when an entity on this client has received an invite to a lobby.
        /// </summary>
        /// <param name="listeningEntity">
        /// The entity which is listening for invites and which has been invited.
        /// </param>
        /// <param name="invitingEntity">
        /// The entity which has invited the <c>listeningEntity</c> to a lobby.
        /// </param>
        /// <param name="connectionString">
        /// The connection string of the lobby to which the <c>listeningEntity</c> has been invited.
        /// </param>
        public delegate void OnLobbyInviteReceivedHandler(PFEntityKey listeningEntity, PFEntityKey invitingEntity, string connectionString);

        /// <summary>
        /// Handler for when an invite listener's status has changed.
        /// </summary>
        /// <param name="listeningEntity">
        /// The entity associated with the invite listener.
        /// </param>
        /// <param name="newStatus">
        /// Value representing the current status of an invite listener.
        /// </param>
        public delegate void OnLobbyInviteListenerStatusChangedHandler(PFEntityKey listeningEntity, LobbyInviteListenerStatus newStatus);

        /// <summary>
        /// Handler for when a matchmaking ticket status has changed.
        /// </summary>
        /// <param name="ticket">
        /// The matchmaking ticket whose status changed.
        /// </param>
        public delegate void OnMatchmakingTicketStatusChangedHandler(MatchmakingTicket ticket);

        /// <summary>
        /// Handler for when a matchmaking ticket status has changed.
        /// </summary>
        /// <param name="ticket">
        /// The matchmaking ticket whose status changed.
        /// </param>
        /// <param name="result">
        /// Indicates whether the ticket found a match or provides the high-level reason that it failed.
        /// </param>
        public delegate void OnMatchmakingTicketCompletedHandler(MatchmakingTicket ticket, int result);

        /// <summary>
        /// Event triggered when an there is an error calling another API to be used for debugging purposes. 
        /// </summary>
        public static event OnErrorEventHandler OnError;

        /// <summary>
        /// Event triggered when a previous call to <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" /> completed.
        /// </summary>
        public static event OnLobbyCreateAndJoinCompletedHandler OnLobbyCreateAndJoinCompleted;

        /// <summary>
        /// Event triggered when a client has disconnected from a lobby.
        /// </summary>
        public static event OnLobbyDisconnectedHandler OnLobbyDisconnected;

        /// <summary>
        /// Event triggered when a PlayFab entity was added to a lobby as a member.
        /// </summary>
        public static event OnLobbyMemberAddedHandler OnLobbyMemberAdded;

        /// <summary>
        /// Event triggered when a PlayFab entity was removed from a lobby as a member.
        /// </summary>
        public static event OnLobbyMemberRemovedHandler OnLobbyMemberRemoved;

        /// <summary>
        /// Event triggered when a add member is completed.
        /// </summary>
        public static event OnAddMemberCompletedHandler OnAddMemberCompleted;

        /// <summary>
        /// Event triggered when a force remove member is completed.
        /// </summary>
        public static event OnForceRemoveMemberCompletedHandler OnForceRemoveMemberCompleted;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="PlayFabMultiplayer.JoinLobby()" /> completed.
        /// </summary>
        public static event OnLobbyJoinCompletedHandler OnLobbyJoinCompleted;

        /// <summary>
        /// Event triggered when the lobby was updated.
        /// </summary>
        public static event OnLobbyUpdatedHandler OnLobbyUpdated;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="Lobby.PostUpdate()" /> completed.
        /// </summary>
        public static event OnLobbyPostUpdateCompletedHandler OnLobbyPostUpdateCompleted;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="PlayFabMultiplayer.JoinArrangedLobby()" /> completed.
        /// </summary>
        public static event OnLobbyJoinArrangedLobbyCompletedHandler OnLobbyJoinArrangedLobbyCompleted;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="PlayFabMultiplayer.FindLobbies()" /> completed.
        /// </summary>
        public static event OnLobbyFindLobbiesCompletedHandler OnLobbyFindLobbiesCompleted;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="PlayFabMultiplayer.SendInvite()" /> completed.
        /// </summary>
        public static event OnLobbySendInviteCompletedHandler OnLobbySendInviteCompleted;

        /// <summary>
        /// Event triggered when an entity on this client has received an invite to a lobby.
        /// </summary>
        public static event OnLobbyInviteReceivedHandler OnLobbyInviteReceived;

        /// <summary>
        /// Event triggered when the operation started by a previous call to <see cref="Lobby.Leave()" /> completed.
        /// </summary>
        public static event OnLobbyLeaveCompletedHandler OnLobbyLeaveCompleted;

        /// <summary>
        /// Event triggered when an invite listener's status has changed.
        /// </summary>
        public static event OnLobbyInviteListenerStatusChangedHandler OnLobbyInviteListenerStatusChanged;

        /// <summary>
        /// Event triggered when a matchmaking ticket status has changed.
        /// </summary>
        public static event OnMatchmakingTicketStatusChangedHandler OnMatchmakingTicketStatusChanged;

        /// <summary>
        /// Event triggered when a matchmaking ticket status has completed
        /// </summary>
        public static event OnMatchmakingTicketCompletedHandler OnMatchmakingTicketCompleted;

        internal enum PFMultiplayerInitStatus
        {
            Uninitialized,
            Initialized,
            CleanupStarted,
        }

        /// <summary>
        /// Gets or sets the amount of logging currently enabled.
        /// </summary>
        public static LogLevelType LogLevel
        {
            get
            {
                return logLevel;
            }

            set
            {
                logLevel = value;
            }
        }

        /// <summary>
        /// Returns true if the library has been initialized.
        /// </summary>
        public static bool IsInitialized
        {
            get
            {
                return initStatus != PFMultiplayerInitStatus.Uninitialized;
            }
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Sets the token that should be used for authentication when performing library actions on behalf of an entity. If a
        /// token has previously been set for the entity, this replaces its previous token.
        /// </summary>
        /// <remarks>
        /// This method takes a PlayFabAuthenticationContext <paramref name="authContext" />
        /// returned by a PlayFab login method.
        /// When the library performs operations on behalf of an entity that require authentication
        /// or authorization, such as creating or updating a lobby, the library will look up a token associated with the entity
        /// to use for the operation. If no token has previously been set for the entity, the operation will synchronously fail.
        /// During the asynchronous operation, the PlayFab service will validate that the token is valid, is not expired, is
        /// associated with the Entity ID provided, and is authorized to perform the operation. If these conditions aren't met,
        /// the operation will fail.
        /// <para>
        /// A PlayFab Entity Key and Entity Token can be obtained from the output of a PlayFab login operation and then provided
        /// as input to this method.
        /// </para>
        /// <para>
        /// The provided <paramref name="authContext" /> must have been acquired using the same PlayFab
        /// Title ID that was passed to <see cref="PlayFabMultiplayer.Initialize()" />.
        /// </para>
        /// <para>
        /// The Multiplayer library makes a copy of the supplied PlayFab Entity Token for use in subsequent operations that
        /// require authentication or authorization of the local user, such as <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" />. If the token
        /// provided to this call is expired or otherwise invalid, operations that require a valid token will fail. A new, valid
        /// token can be provided to the Multiplayer library by calling this method again using the same entity key.
        /// </para>
        /// <para>
        /// The caller is responsible for monitoring the expiration of the entity token provided to this method. When the token
        /// is nearing or past the expiration time a new token should be obtained by performing a PlayFab login operation and
        /// provided to the Multiplayer library by calling this method again. It is recommended to acquire a new token when the
        /// previously supplied token is halfway through its validity period. On platforms that may enter a low power state or
        /// otherwise cause the application to pause execution for a long time, preventing the token from being refreshed before
        /// it expires, the token should be checked for expiration once execution resumes.
        /// </para>
        /// </remarks>
        /// <param name="localMember">
        /// The PlayFab PlayFabAuthenticationContext containing the entity and token.
        /// </param>
        /// <seealso cref="Initialize" />
        public static void SetEntityToken(PlayFab.PlayFabAuthenticationContext localMember)
        {
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerSetEntityToken(
                multiplayerHandle, 
                new PFEntityKey(localMember).EntityKey,
                localMember.EntityToken));
        }
#endif

        /// <summary>
        /// Sets the token that should be used for authentication when performing library actions on behalf of an entity. If a
        /// token has previously been set for the entity, this replaces its previous token.
        /// </summary>
        /// <remarks>
        /// This method takes a PlayFab Entity Key as <paramref name="localMember" /> and a PlayFab Entity Token as
        /// <paramref name="entityToken" />. When the library performs operations on behalf of an entity that require authentication
        /// or authorization, such as creating or updating a lobby, the library will look up a token associated with the entity
        /// to use for the operation. If no token has previously been set for the entity, the operation will synchronously fail.
        /// During the asynchronous operation, the PlayFab service will validate that the token is valid, is not expired, is
        /// associated with the Entity ID provided, and is authorized to perform the operation. If these conditions aren't met,
        /// the operation will fail.
        /// <para>
        /// A PlayFab Entity Key and Entity Token can be obtained from the output of a PlayFab login operation and then provided
        /// as input to this method.
        /// </para>
        /// <para>
        /// The provided <paramref name="localMember" /> and <paramref name="entityToken" /> must have been acquired using the same PlayFab
        /// Title ID that was passed to <see cref="PlayFabMultiplayer.Initialize()" />.
        /// </para>
        /// <para>
        /// The Multiplayer library makes a copy of the supplied PlayFab Entity Token for use in subsequent operations that
        /// require authentication or authorization of the local user, such as <see cref="PlayFabMultiplayer.CreateAndJoinLobby()" />. If the token
        /// provided to this call is expired or otherwise invalid, operations that require a valid token will fail. A new, valid
        /// token can be provided to the Multiplayer library by calling this method again using the same entity key.
        /// </para>
        /// <para>
        /// The caller is responsible for monitoring the expiration of the entity token provided to this method. When the token
        /// is nearing or past the expiration time a new token should be obtained by performing a PlayFab login operation and
        /// provided to the Multiplayer library by calling this method again. It is recommended to acquire a new token when the
        /// previously supplied token is halfway through its validity period. On platforms that may enter a low power state or
        /// otherwise cause the application to pause execution for a long time, preventing the token from being refreshed before
        /// it expires, the token should be checked for expiration once execution resumes.
        /// </para>
        /// </remarks>
        /// <param name="localMember">
        /// The PlayFab Entity Key to associate with a token.
        /// </param>
        /// <param name="entityToken">
        /// The PlayFab Entity token to associate with an entity.
        /// </param>
        /// <seealso cref="Initialize" />
        public static void SetEntityToken(
            PFEntityKey localMember,
            string entityToken)
        {
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerSetEntityToken(multiplayerHandle, localMember.EntityKey, entityToken));
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Create a new lobby and add the creating PlayFab entity to it.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// </remarks>
        /// <param name="creator">
        /// The local PlayFab entity creating the lobby.
        /// </param>
        /// <param name="createConfiguration">
        /// The initial lobby configuration data used when creating the lobby.
        /// </param>
        /// <param name="joinConfiguration">
        /// The initial configuration data for the member creating and joining the lobby.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby CreateAndJoinLobby(
            PlayFab.PlayFabAuthenticationContext creator,
            LobbyCreateConfiguration createConfiguration,
            LobbyJoinConfiguration joinConfiguration)
        {
            PlayFabMultiplayer.SetEntityToken(creator);
            return CreateAndJoinLobby(new PFEntityKey(creator), createConfiguration, joinConfiguration);
        }
#endif

        /// <summary>
        /// Create a new lobby and add the creating PlayFab entity to it.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// </remarks>
        /// <param name="creator">
        /// The local PlayFab entity creating the lobby.
        /// </param>
        /// <param name="createConfiguration">
        /// The initial configuration data used when creating the lobby.
        /// </param>
        /// <param name="joinConfiguration">
        /// The initial configuration data for the member creating and joining the lobby.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby CreateAndJoinLobby(
            PFEntityKey creator,
            LobbyCreateConfiguration createConfiguration,
            LobbyJoinConfiguration joinConfiguration)
        {
            InteropWrapper.PFLobbyHandle lobbyHandle;
            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerCreateAndJoinLobby(
                multiplayerHandle,
                creator.EntityKey,
                createConfiguration.Config,
                joinConfiguration.Config,
                null,
                out lobbyHandle)))
            {
                return Lobby.GetLobbyUsingCache(lobbyHandle);
            }
            else
            {
                return null;
            }
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Join a lobby as the local PlayFab entity.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// </remarks>
        /// <param name="newMember">
        /// The local entity joining the lobby.
        /// </param>
        /// <param name="connectionString">
        /// The connection string used by the entity to join the lobby.
        /// </param>
        /// <param name="memberKeyValuePairs">
        /// The number of initial member properties for the joiner of the lobby.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby JoinLobby(
            PlayFab.PlayFabAuthenticationContext newMember,
            string connectionString,
            IDictionary<string, string> memberKeyValuePairs)
        {
            PlayFabMultiplayer.SetEntityToken(newMember);
            return JoinLobby(new PFEntityKey(newMember), connectionString, memberKeyValuePairs);
        }
#endif

        /// <summary>
        /// Join a lobby as the local PlayFab entity.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyCreateAndJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// </remarks>
        /// <param name="newMember">
        /// The local entity joining the lobby.
        /// </param>
        /// <param name="connectionString">
        /// The connection string used by the entity to join the lobby.
        /// </param>
        /// <param name="memberKeyValuePairs">
        /// The number of initial member properties for the joiner of the lobby.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby JoinLobby(
            PFEntityKey newMember,
            string connectionString,
            IDictionary<string, string> memberKeyValuePairs)
        {
            InteropWrapper.PFLobbyHandle lobbyHandle;
            InteropWrapper.PFLobbyJoinConfiguration configuration = new InteropWrapper.PFLobbyJoinConfiguration();
            if (memberKeyValuePairs != null)
            {
                configuration.MemberProperties = (Dictionary<string, string>)memberKeyValuePairs;
            }

            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerJoinLobby(
                multiplayerHandle,
                newMember.EntityKey,
                connectionString,
                configuration,
                null,
                out lobbyHandle)))
            {
                return Lobby.GetLobbyUsingCache(lobbyHandle);
            }
            else
            {
                return null;
            }
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Joins a lobby using an arrangement string provided by another service, such as matchmaking. If no one has joined the
        /// lobby yet, the lobby is initialized using the configuration parameters.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// When using matchmaking through this library, the <see cref="MatchmakingMatchDetails.LobbyArrangementString" \>
        /// can be used with this method to join a lobby with all of the users that have been matched together.
        /// </para>
        /// </remarks>
        /// <param name="newMember">
        /// The local PlayFab entity joining the lobby.
        /// </param>
        /// <param name="arrangementString">
        /// The arrangement string used by the entity to join the lobby.
        /// </param>
        /// <param name="config">
        /// The initial configuration data used to initialize the lobby, if no one has joined the lobby yet.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby JoinArrangedLobby(
            PlayFab.PlayFabAuthenticationContext newMember,
            string arrangementString,
            LobbyArrangedJoinConfiguration config)
        {
            PlayFabMultiplayer.SetEntityToken(newMember);
            return JoinArrangedLobby(new PFEntityKey(newMember), arrangementString, config);
        }
#endif

        /// <summary>
        /// Joins a lobby using an arrangement string provided by another service, such as matchmaking. If no one has joined the
        /// lobby yet, the lobby is initialized using the configuration parameters.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyMemberAdded" /> followed by a <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted" />
        /// with the <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyArrangedJoinCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// When using matchmaking through this library, the <see cref="MatchmakingMatchDetails.LobbyArrangementString" \>
        /// can be used with this method to join a lobby with all of the users that have been matched together.
        /// </para>
        /// </remarks>
        /// <param name="newMember">
        /// The local PlayFab entity joining the lobby.
        /// </param>
        /// <param name="arrangementString">
        /// The arrangement string used by the entity to join the lobby.
        /// </param>
        /// <param name="config">
        /// The initial configuration data used to initialize the lobby, if no one has joined the lobby yet.
        /// </param>
        /// <returns>
        /// Output lobby object which can be used to queue operations for immediate execution of this operation
        /// </returns>
        public static Lobby JoinArrangedLobby(
            PFEntityKey newMember,
            string arrangementString,
            LobbyArrangedJoinConfiguration config)
        {
            InteropWrapper.PFLobbyHandle lobbyHandle;
            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerJoinArrangedLobby(
                multiplayerHandle,
                newMember.EntityKey,
                arrangementString,
                config.Config,
                null,
                out lobbyHandle)))
            {
                return Lobby.GetLobbyUsingCache(lobbyHandle);
            }
            else
            {
                return null;
            }
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Search for lobbies on behalf of the local user.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompletedS.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// </para>
        /// </remarks>
        /// <param name="searchingEntity">
        /// The PlayFab entity performing the search.
        /// </param>
        /// <param name="searchConfiguration">
        /// The configuration used to filter and sort the searched lobbies.
        /// </param>
        public static void FindLobbies(
            PlayFab.PlayFabAuthenticationContext searchingEntity,
            LobbySearchConfiguration searchConfiguration)
        {
            PlayFabMultiplayer.SetEntityToken(searchingEntity);
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerFindLobbies(
                multiplayerHandle,
                new PFEntityKey(searchingEntity).EntityKey,
                searchConfiguration.SearchConfig,
                null));
        }
#endif

        /// <summary>
        /// Search for lobbies on behalf of the local user.
        /// <summary>
        /// <remarks>
        /// This is an asynchronous operation. Upon successful completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompletedS.result" /> field set to
        /// <c>LobbyStateChangeResult.Succeeded</c>. Upon a failed completion, the title will be provided a
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted" /> with the
        /// <see cref="PlayFabMultiplayer.OnLobbyFindLobbiesCompleted.result" /> field set to a failure
        /// <see cref="LobbyStateChangeResult" />.
        /// <para>
        /// </para>
        /// </remarks>
        /// <param name="searchingEntity">
        /// The PlayFab entity performing the search.
        /// </param>
        /// <param name="searchConfiguration">
        /// The configuration used to filter and sort the searched lobbies.
        /// </param>
        public static void FindLobbies(
            PFEntityKey searchingEntity,
            LobbySearchConfiguration searchConfiguration)
        {
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerFindLobbies(
                multiplayerHandle,
                searchingEntity.EntityKey,
                searchConfiguration.SearchConfig,
                null));
        }

        /// <summary>
        /// Creates a matchmaking ticket for one or more local users.
        /// </summary>
        /// <remarks>
        /// The library automatically, and asynchronously, will submit all local users on a ticket to the matchmaking service.
        /// Each time the ticket status changes, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided.
        /// The ticket status can be queried at any time via <see cref="MatchmakingTicket.Status" />. The ticket
        /// immediately starts in the <c>MatchmakingTicketStatus.Creating</c> state.
        /// <para>
        /// When the ticket has completed, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided. At
        /// that point, a match will have been found or the ticket stopped due to failure. On success, the match that was found
        /// can be queried via <see cref="MatchmakingTicket.GetMatchDetails()" />.
        /// </para>
        /// <para>
        /// All existing tickets in which a local user is a member will be canceled as part of this operation.
        /// </para>
        /// <para>
        /// A match can't be found until all remote users specified in the <c>membersToMatchWith</c> field of the
        /// <c>configuration</c> parameter have joined the ticket via <see cref="PlayFabMultiplayer.JoinMatchmakingTicketFromId()" />.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user along with local user attributes to include in the ticket.
        /// </param>
        /// <param name="queueName">
        /// The queue to which the ticket belongs.
        /// </param>
        /// <param name="timeoutInSeconds">
        /// How long to attempt matchmaking the ticket, in seconds.  Defaults to 120 seconds
        /// </param>
        /// <returns>
        /// The resulting ticket object.
        /// </returns>
        public static MatchmakingTicket CreateMatchmakingTicket(
            MatchUser localUser,
            string queueName,
            uint timeoutInSeconds = 120)
        {
            return CreateMatchmakingTicket(
                new List<MatchUser>() { localUser }, 
                queueName,
                new List<PFEntityKey>(),
                timeoutInSeconds);
        }

        /// <summary>
        /// Creates a matchmaking ticket for one or more local users.
        /// </summary>
        /// <remarks>
        /// The library automatically, and asynchronously, will submit all local users on a ticket to the matchmaking service.
        /// Each time the ticket status changes, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided.
        /// The ticket status can be queried at any time via <see cref="MatchmakingTicket.Status" />. The ticket
        /// immediately starts in the <c>MatchmakingTicketStatus.Creating</c> state.
        /// <para>
        /// When the ticket has completed, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided. At
        /// that point, a match will have been found or the ticket stopped due to failure. On success, the match that was found
        /// can be queried via <see cref="MatchmakingTicket.GetMatchDetails()" />.
        /// </para>
        /// <para>
        /// All existing tickets in which a local user is a member will be canceled as part of this operation.
        /// </para>
        /// <para>
        /// A match can't be found until all remote users specified in the <c>membersToMatchWith</c> field of the
        /// <c>configuration</c> parameter have joined the ticket via <see cref="PlayFabMultiplayer.JoinMatchmakingTicketFromId()" />.
        /// </para>
        /// </remarks>
        /// <param name="localUsers">
        /// The array of local users along with local user attributes to include in the ticket.
        /// </param>
        /// <param name="queueName">
        /// The queue to which the ticket belongs.
        /// </param>
        /// <param name="timeoutInSeconds">
        /// How long to attempt matchmaking the ticket, in seconds.  Defaults to 120 seconds
        /// </param>
        /// <returns>
        /// The resulting ticket object.
        /// </returns>
        public static MatchmakingTicket CreateMatchmakingTicket(
            IList<MatchUser> localUsers,
            string queueName,
            uint timeoutInSeconds = 120)
        {
            return CreateMatchmakingTicket(
                localUsers, 
                queueName,
                new List<PFEntityKey>(),
                timeoutInSeconds);
        }

        /// <summary>
        /// Creates a matchmaking ticket for one or more local users.
        /// </summary>
        /// <remarks>
        /// The library automatically, and asynchronously, will submit all local users on a ticket to the matchmaking service.
        /// Each time the ticket status changes, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided.
        /// The ticket status can be queried at any time via <see cref="MatchmakingTicket.Status" />. The ticket
        /// immediately starts in the <c>MatchmakingTicketStatus.Creating</c> state.
        /// <para>
        /// When the ticket has completed, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided. At
        /// that point, a match will have been found or the ticket stopped due to failure. On success, the match that was found
        /// can be queried via <see cref="MatchmakingTicket.GetMatchDetails()" />.
        /// </para>
        /// <para>
        /// All existing tickets in which a local user is a member will be canceled as part of this operation.
        /// </para>
        /// <para>
        /// A match can't be found until all remote users specified in the <c>membersToMatchWith</c> field of the
        /// <c>configuration</c> parameter have joined the ticket via <see cref="PlayFabMultiplayer.JoinMatchmakingTicketFromId()" />.
        /// </para>
        /// </remarks>
        /// <param name="localUsers">
        /// The array of local users along with local user attributes to include in the ticket.
        /// </param>
        /// <param name="queueName">
        /// The queue to which the ticket belongs.
        /// </param>
        /// <param name="membersToMatchWith">
        /// The other specific users expected to join the ticket.
        /// </param>
        /// <param name="timeoutInSeconds">
        /// How long to attempt matchmaking the ticket, in seconds.  Defaults to 120 seconds
        /// </param>
        /// <returns>
        /// The resulting ticket object.
        /// </returns>
        public static MatchmakingTicket CreateMatchmakingTicket(
            IList<MatchUser> localUsers,
            string queueName,
            List<PFEntityKey> membersToMatchWith,
            uint timeoutInSeconds = 120)
        {
            var internalMembersToMatchWith = membersToMatchWith.Select(x => x.EntityKey).ToList();
            var internalLocalUsers = localUsers.Select(x => x.LocalUser.EntityKey).ToList();
            var localUserJsonAttributesList = localUsers.Select(x => x.LocalUserJsonAttributesJSON).ToList();

            InteropWrapper.PFMatchmakingTicketHandle handle;
            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerCreateMatchmakingTicket(
                multiplayerHandle,
                internalLocalUsers.ToArray(),
                localUserJsonAttributesList.ToArray(),
                new InteropWrapper.PFMatchmakingTicketConfiguration(timeoutInSeconds, queueName, internalMembersToMatchWith),
                null,
                out handle)))
            {
                return MatchmakingTicket.GetMatchmakingTicketUsingCache(handle);
            }

            return null;
        }

        /// <summary>
        /// Joins one or more multiple local users to a matchmaking ticket using a ticket ID and queue name.
        /// </summary>
        /// <remarks>
        /// The library automatically, and asynchronously, will submit all local users to join the ticket on the matchmaking
        /// service. Each time the ticket status changes, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be
        /// provided. The ticket status can be quered at any time via <see cref="MatchmakingTicket.Status" />. The ticket
        /// immediately starts in the <c>MatchmakingTicketStatus.Joining</c> state.
        /// <para>
        /// When the ticket has completed, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided. At
        /// that point, a match will have been found or the ticket stopped due to failure. On success, the match that was found
        /// can be queried via <see cref="MatchmakingTicket.GetMatchDetails()" />.
        /// </para>
        /// <para>
        /// All existing tickets in which a local user is a member will be canceled as part of this operation.
        /// </para>
        /// </remarks>
        /// <param name="localUser">
        /// The local user along with local user attributes to include in the ticket.
        /// </param>
        /// <param name="ticketId">
        /// The ID of the ticket to join.
        /// </param>
        /// <param name="queueName">
        /// The queue to which the ticket belongs.
        /// </param>
        /// <param name="membersToMatchWith">
        /// The other specific users expected to join the ticket.
        /// </param>
        /// <returns>
        /// The resulting ticket object.
        /// </returns>
        public static MatchmakingTicket JoinMatchmakingTicketFromId(
            MatchUser localUser,
            string ticketId,
            string queueName,
            IList<PFEntityKey> membersToMatchWith)
        {
            return JoinMatchmakingTicketFromId(
                new List<MatchUser>() { localUser },
                ticketId,
                queueName,
                membersToMatchWith);
        }

        /// <summary>
        /// Joins one or more multiple local users to a matchmaking ticket using a ticket ID and queue name.
        /// </summary>
        /// <remarks>
        /// The library automatically, and asynchronously, will submit all local users to join the ticket on the matchmaking
        /// service. Each time the ticket status changes, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be
        /// provided. The ticket status can be quered at any time via <see cref="MatchmakingTicket.Status" />. The ticket
        /// immediately starts in the <c>MatchmakingTicketStatus.Joining</c> state.
        /// <para>
        /// When the ticket has completed, a <see cref="PlayFabMultiplayer.OnMatchmakingTicketStatusChanged" /> will be provided. At
        /// that point, a match will have been found or the ticket stopped due to failure. On success, the match that was found
        /// can be queried via <see cref="MatchmakingTicket.GetMatchDetails()" />.
        /// </para>
        /// <para>
        /// All existing tickets in which a local user is a member will be canceled as part of this operation.
        /// </para>
        /// </remarks>
        /// <param name="localUsers">
        /// The array of local users along with local user attributes to include in the ticket.
        /// </param>
        /// <param name="ticketId">
        /// The ID of the ticket to join.
        /// </param>
        /// <param name="queueName">
        /// The queue to which the ticket belongs.
        /// </param>
        /// <param name="membersToMatchWith">
        /// The other specific users expected to join the ticket.
        /// </param>
        /// <returns>
        /// The resulting ticket object.
        /// </returns>
        public static MatchmakingTicket JoinMatchmakingTicketFromId(
            IList<MatchUser> localUsers,
            string ticketId,
            string queueName,
            IList<PFEntityKey> membersToMatchWith)
        {
            var internalMembersToMatchWith = membersToMatchWith.Select(x => x.EntityKey).ToList();
            var internalLocalUsers = localUsers.Select(x => x.LocalUser.EntityKey).ToList();
            var localUserJsonAttributesList = localUsers.Select(x => x.LocalUserJsonAttributesJSON).ToList();

            InteropWrapper.PFMatchmakingTicketHandle handle;
            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerJoinMatchmakingTicketFromId(
                multiplayerHandle,
                internalLocalUsers.ToArray(),
                localUserJsonAttributesList.ToArray(),
                ticketId,
                queueName,
                null,
                out handle)))
            {
                return MatchmakingTicket.GetMatchmakingTicketUsingCache(handle);
            }

            return null;
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Enables the Lobby invite listener for a given entity.
        /// </summary>
        /// <remarks>
        /// This operation will synchronously start listening for invites on behalf of the provided entity. When invites
        /// are received, they will be provided via <see cref="PlayFabMultiplayer.OnLobbyInviteReceived" /> events. When the status of
        /// the invite listener changes, notifications will be provided via
        /// <see cref="PlayFabMultiplayer.OnLobbyInviteListenerStatusChanged" /> events.
        /// <para>
        /// Only invites sent after the listener has been started will be received on this client. Invites sent while
        /// this listener is not active will not be queued.
        /// </para>
        /// <para>
        /// Invite listening is, by default, disabled for all entities. This method should be called for each local entity
        /// that the title wants to receive Lobby invites.
        /// </para>
        /// <para>
        /// Lobby invites and this invite listener are unrelated to and unaffected by platform invite mechanisms.
        /// </para>
        /// <para>
        /// This method may only be called if the Lobby invite listener is not already enabled for the given entity.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which will listen for invites.
        /// </param>
        public static void StartListeningForLobbyInvites(PlayFab.PlayFabAuthenticationContext listeningEntity)
        {
            PlayFabMultiplayer.SetEntityToken(listeningEntity);
            StartListeningForLobbyInvites(new PFEntityKey(listeningEntity));
        }

        /// <summary>
        /// Disables the Lobby invite listener for a given entity.
        /// </summary>
        /// <remarks>
        /// This operation will synchronously stop listening for invites on behalf of the provided entity.
        /// <para>
        /// Invite notifications which have already been queued internally will still be provided via the next call to
        /// <see cref="PlayFabMultiplayer.ProcessingLobbyStateChanges()" />.
        /// </para>
        /// <para>
        /// Lobby invites and this invite listener are unrelated to and unaffected by platform invite mechanisms.
        /// </para>
        /// <para>
        /// This method may only be called if the Lobby invite listener is already enabled for the given entity.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which is listening for invites.
        /// </param>
        public static void StopListeningForLobbyInvites(PlayFab.PlayFabAuthenticationContext listeningEntity)
        {
            StopListeningForLobbyInvites(new PFEntityKey(listeningEntity));
        }

        /// <summary>
        /// Retrieve the status of the entity's invite listener.
        /// </summary>
        /// <remarks>
        /// This value is used to understand the state of an entity's invite listener. If the invite listener encounters
        /// a fatal error, non-fatal error, or diagnostic change, the listener's status value will reflect it.
        /// <para>
        /// When the invite listener's status changes, a <see cref="PlayFabMultiplayer.OnLobbyInviteListenerStatusChanged" />
        /// struct will be provided by <see cref="PlayFabMultiplayer.OnMultiplayerStartProcessingLobby" />. This method can then be
        /// called to retrieve the latest status and act accordingly.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which is listening for invites.
        /// </param>
        /// <returns>
        /// The output status value.
        /// </returns>
        public static LobbyInviteListenerStatus GetLobbyInviteListenerStatus(PlayFab.PlayFabAuthenticationContext listeningEntity)
        {
            PlayFabMultiplayer.SetEntityToken(listeningEntity);
            return GetLobbyInviteListenerStatus(new PFEntityKey(listeningEntity));
        }
#endif

        /// <summary>
        /// Enables the Lobby invite listener for a given entity.
        /// </summary>
        /// <remarks>
        /// This operation will synchronously start listening for invites on behalf of the provided entity. When invites
        /// are received, they will be provided via <see cref="PlayFabMultiplayer.OnLobbyInviteReceived" /> events. When the status of
        /// the invite listener changes, notifications will be provided via
        /// <see cref="PlayFabMultiplayer.OnLobbyInviteListenerStatusChanged" /> events.
        /// <para>
        /// Only invites sent after the listener has been started will be received on this client. Invites sent while
        /// this listener is not active will not be queued.
        /// </para>
        /// <para>
        /// Invite listening is, by default, disabled for all entities. This method should be called for each local entity
        /// that the title wants to receive Lobby invites.
        /// </para>
        /// <para>
        /// Lobby invites and this invite listener are unrelated to and unaffected by platform invite mechanisms.
        /// </para>
        /// <para>
        /// This method may only be called if the Lobby invite listener is not already enabled for the given entity.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which will listen for invites.
        /// </param>
        public static void StartListeningForLobbyInvites(PFEntityKey listeningEntity)
        {
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerStartListeningForLobbyInvites(
                multiplayerHandle,
                listeningEntity.EntityKey));
        }

        /// <summary>
        /// Disables the Lobby invite listener for a given entity.
        /// </summary>
        /// <remarks>
        /// This operation will synchronously stop listening for invites on behalf of the provided entity.
        /// <para>
        /// Invite notifications which have already been queued internally will still be provided via the next call to
        /// <see cref="PlayFabMultiplayer.ProcessingLobbyStateChanges()" />.
        /// </para>
        /// <para>
        /// Lobby invites and this invite listener are unrelated to and unaffected by platform invite mechanisms.
        /// </para>
        /// <para>
        /// This method may only be called if the Lobby invite listener is already enabled for the given entity.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which is listening for invites.
        /// </param>
        public static void StopListeningForLobbyInvites(PFEntityKey listeningEntity)
        {
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerStopListeningForLobbyInvites(
                multiplayerHandle,
                listeningEntity.EntityKey));
        }

        /// <summary>
        /// Retrieve the status of the entity's invite listener.
        /// </summary>
        /// <remarks>
        /// This value is used to understand the state of an entity's invite listener. If the invite listener encounters
        /// a fatal error, non-fatal error, or diagnostic change, the listener's status value will reflect it.
        /// <para>
        /// When the invite listener's status changes, a <see cref="PlayFabMultiplayer.OnLobbyInviteListenerStatusChanged" />
        /// struct will be provided by <see cref="PlayFabMultiplayer.OnMultiplayerStartProcessingLobby" />. This method can then be
        /// called to retrieve the latest status and act accordingly.
        /// </para>
        /// </remarks>
        /// <param name="listeningEntity">
        /// The entity which is listening for invites.
        /// </param>
        /// <returns>
        /// The output status value.
        /// </returns>
        public static LobbyInviteListenerStatus GetLobbyInviteListenerStatus(PFEntityKey listeningEntity)
        {
            InteropWrapper.PFLobbyInviteListenerStatus status;
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerGetLobbyInviteListenerStatus(
                multiplayerHandle,
                listeningEntity.EntityKey,
                out status));
            return (LobbyInviteListenerStatus)status;
        }

#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Initializes an instance of the PlayFab Multiplayer library.
        /// </summary>
        /// <remarks>
        /// Initialize() cannot be called again without a subsequent <see cref="PlayFabMultiplayer.Uninitialize()" /> call.
        /// <para>
        /// Every call to Initialize() should have a corresponding Uninitialize() call.
        /// </para>
        /// <para>
        /// The playFabTitleId is read from PlayFab's static PlayFabSettings asset.  It can be changed 
        /// Using Unity menu, PlayFab | MakePlayFabSharedSettings menu.  
        /// It must be the same PlayFab Title ID used to acquire the PlayFab Entity
        /// Keys and Entity Tokens that will be passed to <see cref="PlayFabMultiplayer.SetEntityToken()" />.
        /// </para>
        /// </remarks>
        /// <seealso cref="Uninitialize" />
        public static void Initialize()
        {
            string playFabTitleId = PlayFab.PlayFabSettings.TitleId;
            if (string.IsNullOrEmpty(playFabTitleId))
            {
                throw new PlayFabException(PlayFabExceptionCode.TitleNotSet, $"PlayFab.PlayFabSettings.TitleId must be set");
            }
#else
        /// <summary>
        /// Initializes an instance of the PlayFab Multiplayer library.
        /// </summary>
        /// <remarks>
        /// Initialize() cannot be called again without a subsequent <see cref="PlayFabMultiplayer.Uninitialize()" /> call.
        /// <para>
        /// Every call to Initialize() should have a corresponding Uninitialize() call.
        /// </para>
        /// <para>
        /// It must be the same PlayFab Title ID used to acquire the PlayFab Entity
        /// Keys and Entity Tokens that will be passed to <see cref="PlayFabMultiplayer.SetEntityToken()" />.
        /// </para>
        /// </remarks>
        /// <param name="playFabTitleId">
        /// The app's PlayFab Title ID.
        /// </param>
        public static void Initialize(string playFabTitleId)
        {
            if (string.IsNullOrEmpty(playFabTitleId))
            {
                throw new Exception($"playFabTitleId must be set");
            }
#endif

            if (initStatus != PFMultiplayerInitStatus.Uninitialized)
            {
                LogInfo("PlayFabMultiplayer already initialized");
                return;
            }

            logLevel = LogLevelType.Minimal;

            int result = InteropWrapper.PFMultiplayer.PFMultiplayerInitialize(playFabTitleId, out multiplayerHandle);
            Succeeded(result); // log failures
            if (LobbyError.FAILED(result))
            {
                string errorMessage = InteropWrapper.PFMultiplayer.PFMultiplayerGetErrorMessage(result);
                throw new Exception($"PlayFabMultiplayer.Initialize failed. {errorMessage}");
            }
            else
            {
                initStatus = PFMultiplayerInitStatus.Initialized;
            }
        }

        /// <summary>
        /// Immediately reclaims all resources associated with all Multiplayer library objects.
        /// </summary>
        /// <remarks>
        /// If local users were participating in a Lobby, they are removed (it appears to remote lobby clients as if network
        /// connectivity to these users has been lost), so best practice is to call <see cref="Lobby.Leave()" /> on all lobbies
        /// and wait for the corresponding <see cref="PlayFabMultiplayer.OnLobbyLeaveCompleted" /> event to have the local users exit any
        /// existing lobbies.
        /// <para>
        /// This method is not thread-safe and may not be called concurrently with other Multiplayer library methods. After
        /// calling this method, all Multiplayer library state is invalidated.
        /// </para>
        /// <para>
        /// Titles using the Microsoft Game Core version of the Multiplayer library must listen for app state notifications via
        /// the RegisterAppStateChangeNotification API. When the app is suspended, the title must call
        /// Uninitialize(). When the app is resumed, the title must wait for the Game Core networking stack to be
        /// ready and then re-initialize the Multiplayer library by calling Initialize().
        /// </para>
        /// <para>
        /// Every call to <see cref="PlayFabMultiplayer.Initialize()" /> should have a corresponding Uninitialize() call.
        /// </para>
        /// </remarks>
        /// <seealso cref="PlayFabMultiplayer.Initialize" />
        public static void Uninitialize()
        {
            if (initStatus != PFMultiplayerInitStatus.Initialized)
            {
                LogInfo("PlayFabMultiplayer not initialized");
                return;
            }

            LogInfo("PlayFabMultiplayer.Uninitialize");
            initStatus = PFMultiplayerInitStatus.CleanupStarted;
            Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerUninitialize(multiplayerHandle));
            multiplayerHandle = null;
            initStatus = PFMultiplayerInitStatus.Uninitialized;
        }

        /// <summary>
        /// Triggers all the lobby related PlayFabMultiplayer.OnLobby* events since the last such call.
        /// </summary>
        /// <remarks>
        /// This method provides the Lobby library an opportunity to synchronize state with remote devices or services
        /// <para>
        /// Lobby library state exposed by the library can change during this call, so you must be thread-safe in your use of
        /// it. For example, invoking ProcessLobbyStateChanges() on your UI thread at the same time a separate worker
        /// thread is looping through the list of endpoints returned by <see cref="Lobby.GetMembers()" /> may result in crashes
        /// because ProcessLobbyStateChanges() can alter the memory associated with the member list.
        /// ProcessLobbyStateChanges() should be called frequently-- at least once per graphics frame. It's designed to
        /// execute and return quickly such that it can be called on your main UI thread with negligible impact. For best
        /// results, you should also minimize the time you spend handling state events.
        /// </para>
        public static void ProcessLobbyStateChanges()
        {
            if (multiplayerHandle == null)
            {
                return;
            }

            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerStartProcessingLobbyStateChanges(
                multiplayerHandle,
                out lobbyStateChanges)))
            {
                foreach (InteropWrapper.PFLobbyStateChange stateChange in lobbyStateChanges.StateChanges)
                {
                    LogInfo("Lobby State change: " + stateChange.StateChangeType.ToString());
                    switch (stateChange.StateChangeType)
                    {
                        case InteropWrapper.PFLobbyStateChangeType.CreateAndJoinLobbyCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyCreateAndJoinCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);
                                OnLobbyCreateAndJoinCompleted?.Invoke(
                                    Lobby.GetLobbyUsingCache(stateChangeConverted.lobby),
                                    stateChangeConverted.result);
                                break;
                            }
                        
                        case InteropWrapper.PFLobbyStateChangeType.JoinLobbyCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyJoinCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);
                                OnLobbyJoinCompleted?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.newMember), stateChangeConverted.result);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.MemberAdded:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyMemberAddedStateChange)stateChange;
                                OnLobbyMemberAdded?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.member));
                                break;
                            }
                        
                        case InteropWrapper.PFLobbyStateChangeType.AddMemberCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyAddMemberCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);
                                OnAddMemberCompleted?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.localUser), stateChangeConverted.result);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.MemberRemoved:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyMemberRemovedStateChange)stateChange;
                                OnLobbyMemberRemoved?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.member), (LobbyMemberRemovedReason)stateChangeConverted.reason);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.ForceRemoveMemberCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyForceRemoveMemberCompletedStateChange)stateChange;
                                OnForceRemoveMemberCompleted?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.targetMember), stateChangeConverted.result);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.Updated:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyUpdatedStateChange)stateChange;
                                List<LobbyMemberUpdateSummary> memberUpdates = new List<LobbyMemberUpdateSummary>();
                                foreach (var memberUpdate in stateChangeConverted.memberUpdates)
                                {
                                    memberUpdates.Add(new LobbyMemberUpdateSummary(memberUpdate));
                                }

                                OnLobbyUpdated?.Invoke(
                                    Lobby.GetLobbyUsingCache(stateChangeConverted.lobby),
                                    stateChangeConverted.ownerUpdated,
                                    stateChangeConverted.maxMembersUpdated,
                                    stateChangeConverted.accessPolicyUpdated,
                                    stateChangeConverted.membershipLockUpdated,
                                    stateChangeConverted.updatedSearchPropertyKeys.ToList(),
                                    stateChangeConverted.updatedLobbyPropertyKeys.ToList(),
                                    memberUpdates);
                                break;
                            }
                        
                        case InteropWrapper.PFLobbyStateChangeType.PostUpdateCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyPostUpdateCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);
                                OnLobbyPostUpdateCompleted?.Invoke(
                                    Lobby.GetLobbyUsingCache(stateChangeConverted.lobby),
                                    new PFEntityKey(stateChangeConverted.localUser),
                                    stateChangeConverted.result);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.LeaveLobbyCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyLeaveCompletedStateChange)stateChange;
                                OnLobbyLeaveCompleted?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby), new PFEntityKey(stateChangeConverted.localUser));
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.Disconnecting:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyDisconnectingStateChange)stateChange;
                                LogInfo("LobbyDisconnecting due to " + stateChangeConverted.reason.ToString());

                                // Other than logging, event ignored and not passed along to caller 
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.Disconnected:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyDisconnectedStateChange)stateChange;
                                OnLobbyDisconnected?.Invoke(Lobby.GetLobbyUsingCache(stateChangeConverted.lobby));
                                Lobby.ClearLobbyFromCache(stateChangeConverted.lobby);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.JoinArrangedLobbyCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyArrangedJoinCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);
                                OnLobbyJoinArrangedLobbyCompleted?.Invoke(
                                    Lobby.GetLobbyUsingCache(stateChangeConverted.lobby),
                                    new PFEntityKey(stateChangeConverted.newMember),
                                    stateChangeConverted.result);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.FindLobbiesCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyFindLobbiesCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);

                                List<LobbySearchResult> searchResults = new List<LobbySearchResult>();
                                foreach (var searchResult in stateChangeConverted.searchResults)
                                {
                                    searchResults.Add(new LobbySearchResult(searchResult));
                                }

                                OnLobbyFindLobbiesCompleted?.Invoke(
                                    searchResults,
                                    new PFEntityKey(stateChangeConverted.searchingEntity),
                                    stateChangeConverted.result);
                                break;
                            }
                        
                        case InteropWrapper.PFLobbyStateChangeType.InviteReceived:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyInviteReceivedStateChange)stateChange;

                                OnLobbyInviteReceived?.Invoke(
                                    new PFEntityKey(stateChangeConverted.listeningEntity),
                                    new PFEntityKey(stateChangeConverted.invitingEntity),
                                    stateChangeConverted.connectionString);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.InviteListenerStatusChanged:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbyInviteListenerStatusChangedStateChange)stateChange;
                                var listeningEntity = new PFEntityKey(stateChangeConverted.listeningEntity);
                                LobbyInviteListenerStatus newStatus = GetLobbyInviteListenerStatus(listeningEntity);
                                OnLobbyInviteListenerStatusChanged?.Invoke(
                                    listeningEntity,
                                    newStatus);
                                break;
                            }

                        case InteropWrapper.PFLobbyStateChangeType.SendInviteCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFLobbySendInviteCompletedStateChange)stateChange;
                                Succeeded(stateChangeConverted.result);

                                OnLobbySendInviteCompleted?.Invoke(
                                    Lobby.GetLobbyUsingCache(stateChangeConverted.lobby),
                                    new PFEntityKey(stateChangeConverted.sender),
                                    new PFEntityKey(stateChangeConverted.invitee),
                                    stateChangeConverted.result);
                                break;
                            }
                    }
                }

                Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerFinishProcessingLobbyStateChanges(
                    multiplayerHandle,
                    lobbyStateChanges));
            }
        }

        /// <summary>
        /// Triggers all the lobby related PlayFabMultiplayer.OnMatchmaking* events since the last such call.
        /// </summary>
        /// <remarks>
        /// This method provides the Lobby library an opportunity to synchronize state with remote devices or services
        /// <para>
        /// Lobby library state exposed by the library can change during this call, so you must be thread-safe in your use of
        /// it. For example, invoking ProcessLobbyStateChanges() on your UI thread at the same time a separate worker
        /// thread is looping through the list of endpoints returned by <see cref="Lobby.GetMembers()" /> may result in crashes
        /// because ProcessLobbyStateChanges() can alter the memory associated with the member list.
        /// ProcessLobbyStateChanges() should be called frequently-- at least once per graphics frame. It's designed to
        /// execute and return quickly such that it can be called on your main UI thread with negligible impact. For best
        /// results, you should also minimize the time you spend handling state events.
        /// </para>
        public static void ProcessMatchmakingStateChanges()
        {
            if (multiplayerHandle == null)
            {
                return;
            }

            if (Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerStartProcessingMatchmakingStateChanges(
                multiplayerHandle,
                out matchmakingStateChanges)))
            {
                foreach (InteropWrapper.PFMatchmakingStateChange stateChange in matchmakingStateChanges.StateChanges)
                {
                    LogInfo("Matchmaking State change: " + stateChange.StateChangeType.ToString());
                    switch (stateChange.StateChangeType)
                    {
                        case InteropWrapper.PFMatchmakingStateChangeType.TicketStatusChanged:
                            {
                                var stateChangeConverted = (InteropWrapper.PFMatchmakingTicketStatusChangedStateChange)stateChange;
                                var ticket = MatchmakingTicket.GetMatchmakingTicketUsingCache(stateChangeConverted.Ticket);
                                OnMatchmakingTicketStatusChanged?.Invoke(ticket);
                                break;
                            }

                        case InteropWrapper.PFMatchmakingStateChangeType.TicketCompleted:
                            {
                                var stateChangeConverted = (InteropWrapper.PFMatchmakingTicketCompletedStateChange)stateChange;

                                if (LobbyError.FAILED(stateChangeConverted.Result))
                                {
                                    // Log error detail
                                    LogError(stateChangeConverted.Result.ToString());
                                    Succeeded(stateChangeConverted.Result);
                                }

                                OnMatchmakingTicketCompleted?.Invoke(MatchmakingTicket.GetMatchmakingTicketUsingCache(stateChangeConverted.Ticket), stateChangeConverted.Result);
                                MatchmakingTicket.ClearMatchmakingTicketFromCache(stateChangeConverted.Ticket);

                                // Done with ticket so destroy it now
                                InteropWrapper.PFMultiplayer.PFMultiplayerDestroyMatchmakingTicket(multiplayerHandle, stateChangeConverted.Ticket);
                                break;
                            }
                    }
                }

                Succeeded(InteropWrapper.PFMultiplayer.PFMultiplayerFinishProcessingMatchmakingStateChanges(
                    multiplayerHandle,
                    matchmakingStateChanges));
            }
        }

        internal static void LogError(string message)
        {
            if (initStatus != PFMultiplayerInitStatus.CleanupStarted &&
                logLevel != LogLevelType.None)
            {
#if UNITY_2017_1_OR_NEWER
                UnityEngine.Debug.LogError(message);
#else
                Debug.WriteLine(message);
#endif
            }
        }

        internal static void LogError(int code)
        {
            string errorMessage = InteropWrapper.PFMultiplayer.PFMultiplayerGetErrorMessage(code);
            if (errorMessage == null)
            {
                errorMessage = "Unknown error";
            }

            errorMessage += string.Format(" 0x{0:X}", (uint)code);

            // If we hit an error while cleaning up the PlayFabMultiplayer, don't attempt to raise the
            // error event because it will fail.
            if (initStatus != PFMultiplayerInitStatus.CleanupStarted)
            {
                if (PlayFabMultiplayer.OnError != null)
                {
                    PlayFabMultiplayerErrorArgs args = new PlayFabMultiplayerErrorArgs((int)code, errorMessage);
                    PlayFabMultiplayer.OnError(args);
                }
            }

            LogError(errorMessage);
        }

        internal static void LogWarning(string warningMessage)
        {
            if (logLevel < LogLevelType.Verbose)
            {
                return;
            }
#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.LogWarning(warningMessage);
#else
            Debug.WriteLine(warningMessage);
#endif
        }

        internal static void LogInfo(string infoMessage)
        {
            if (logLevel < LogLevelType.Verbose)
            {
                return;
            }

#if UNITY_2017_1_OR_NEWER
            UnityEngine.Debug.Log(infoMessage);
#else
            Debug.WriteLine(infoMessage);
#endif
        }

        internal static bool Succeeded(int errorCode)
        {
            bool succeeded = false;
            if (LobbyError.FAILED(errorCode))
            {
                LogError(errorCode);
            }
            else
            {
                succeeded = true;
            }

            return succeeded;
        }
    }
}
