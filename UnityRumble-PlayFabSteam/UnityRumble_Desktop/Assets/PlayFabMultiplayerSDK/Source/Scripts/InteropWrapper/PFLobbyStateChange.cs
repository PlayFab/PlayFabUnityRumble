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

namespace PlayFab.Multiplayer.InteropWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using System.Linq;
    using PlayFab.Multiplayer.Interop;

    public class PFLobbyStateChange
    {
        unsafe protected PFLobbyStateChange(PFLobbyStateChangeType StateChangeType, Interop.PFLobbyStateChange* StateChangeId)
        {
            this.StateChangeType = StateChangeType;
            this.StateChangeId = StateChangeId;
            this.useObjectPool = false;
        }

        internal unsafe static PFLobbyStateChange CreateFromPtr(Interop.PFLobbyStateChange* stateChangePtr)
        {
            PFLobbyStateChange result = null;
            PFLobbyStateChangeUnion stateChangeUnion = (PFLobbyStateChangeUnion)Marshal.PtrToStructure(new IntPtr(stateChangePtr), typeof(PFLobbyStateChangeUnion));
            PFLobbyStateChangeType wrapperChangeType = (PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType;
            switch (wrapperChangeType)
            {
                case PFLobbyStateChangeType.CreateAndJoinLobbyCompleted:
                    result = new PFLobbyCreateAndJoinCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.JoinLobbyCompleted:
                    result = new PFLobbyJoinCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.MemberAdded:
                    result = new PFLobbyMemberAddedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.AddMemberCompleted:
                    result = new PFLobbyAddMemberCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.MemberRemoved:
                    result = new PFLobbyMemberRemovedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.ForceRemoveMemberCompleted:
                    result = new PFLobbyForceRemoveMemberCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.LeaveLobbyCompleted:
                    result = new PFLobbyLeaveCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.Updated:
                    result = new PFLobbyUpdatedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.PostUpdateCompleted:
                    result = new PFLobbyPostUpdateCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.Disconnecting:
                    result = new PFLobbyDisconnectingStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.Disconnected:
                    result = new PFLobbyDisconnectedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.JoinArrangedLobbyCompleted:
                    result = new PFLobbyArrangedJoinCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.FindLobbiesCompleted:
                    result = new PFLobbyFindLobbiesCompletedStateChange(stateChangeUnion, stateChangePtr); 
                    break;
                
                case PFLobbyStateChangeType.InviteReceived:
                    result = new PFLobbyInviteReceivedStateChange(stateChangeUnion, stateChangePtr);
                    break;
                
                case PFLobbyStateChangeType.InviteListenerStatusChanged:
                    result = new PFLobbyInviteListenerStatusChangedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                case PFLobbyStateChangeType.SendInviteCompleted:
                    result = new PFLobbySendInviteCompletedStateChange(stateChangeUnion, stateChangePtr);
                    break;

                default:
                    Debug.Write(String.Format("Unhandle type {0}\n", stateChangeUnion.stateChange.stateChangeType));
                    Debugger.Break();
                    result = new PFLobbyStateChange(wrapperChangeType, stateChangePtr);
                    break;
            }

            return result;
        }

        internal virtual void Cleanup()
        {
            if (useObjectPool)
            {
                PFMultiplayer.ObjPool.Return(this);
            }
        }

        public PFLobbyStateChangeType StateChangeType { get; private set; }
        unsafe internal Interop.PFLobbyStateChange* StateChangeId { get; private set; }
        protected bool useObjectPool;
    }

    public class PFLobbyCreateAndJoinCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyCreateAndJoinCompletedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
                Interop.PFLobbyCreateAndJoinLobbyCompletedStateChange stateChangeConverted = stateChangeUnion.createAndJoinCompleted;
                this.result = stateChangeConverted.result;
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
            }
        }

        public int result { get; private set; }
        public Object asyncContext { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
    }

    public class PFLobbyJoinCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyJoinCompletedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyJoinLobbyCompletedStateChange stateChangeConverted = ref stateChangeUnion.joinCompleted;
#else
                Interop.PFLobbyJoinCompletedStateChange stateChangeConverted = stateChangeUnion.joinCompleted;
#endif
                this.result = stateChangeConverted.result;
                Interop.PFEntityKey newMember = stateChangeConverted.newMember;
                this.newMember = new PFEntityKey(&newMember);
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
            }
        }

        public int result { get; private set; }
        public PFEntityKey newMember { get; private set; }
        public Object asyncContext { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
    }

    public class PFLobbyMemberAddedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyMemberAddedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyMemberAddedStateChange stateChangeConverted = ref stateChangeUnion.memberAdded;
#else
                Interop.PFLobbyMemberAddedStateChange stateChangeConverted = stateChangeUnion.memberAdded;
#endif
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey member = stateChangeConverted.member;
                this.member = new PFEntityKey(&member);
            }
        }

        public PFLobbyHandle lobby { get; private set; }
        public PFEntityKey member { get; private set; }
    }

    public class PFLobbyAddMemberCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyAddMemberCompletedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyAddMemberCompletedStateChange stateChangeConverted = ref stateChangeUnion.addMemberCompleted;
#else
                Interop.PFLobbyAddMemberCompletedStateChange stateChangeConverted = stateChangeUnion.addMemberCompleted;
#endif
                this.result = stateChangeConverted.result;
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey localUser = stateChangeConverted.localUser;
                this.localUser = new PFEntityKey(&localUser);
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
            }
        }

        public int result { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
        public PFEntityKey localUser { get; private set; }
        public Object asyncContext { get; private set; }
    }

    public class PFLobbyMemberRemovedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyMemberRemovedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyMemberRemovedStateChange stateChangeConverted = ref stateChangeUnion.memberRemoved;
#else
                Interop.PFLobbyMemberRemovedStateChange stateChangeConverted = stateChangeUnion.memberRemoved;
#endif
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey member = stateChangeConverted.member;
                this.member = new PFEntityKey(&member);
                this.reason = (PFLobbyMemberRemovedReason)stateChangeConverted.reason;
            }
        }

        public PFLobbyHandle lobby { get; private set; }
        public PFEntityKey member { get; private set; }
        public PFLobbyMemberRemovedReason reason { get; private set; }
    }

    public class PFLobbyForceRemoveMemberCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyForceRemoveMemberCompletedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyForceRemoveMemberCompletedStateChange stateChangeConverted = ref stateChangeUnion.forceRemoveMember;
#else
                Interop.PFLobbyForceRemoveMemberCompletedStateChange stateChangeConverted = stateChangeUnion.forceRemoveMember;
#endif
                this.result = stateChangeConverted.result;
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey targetMember = stateChangeConverted.targetMember;
                this.targetMember = new PFEntityKey(&targetMember);
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
            }
        }

        public int result { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
        public PFEntityKey targetMember { get; private set; }
        public Object asyncContext { get; private set; }
    }

    public class PFLobbyLeaveCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyLeaveCompletedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyLeaveLobbyCompletedStateChange stateChangeConverted = ref stateChangeUnion.leaveCompleted;
#else
                Interop.PFLobbyLeaveCompletedStateChange stateChangeConverted = stateChangeUnion.leaveCompleted;
#endif
                Interop.PFEntityKey* localUser = stateChangeConverted.localUser;
                this.localUser = new PFEntityKey(localUser);
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
            }
        }

        public PFEntityKey localUser { get; private set; }
        public Object asyncContext { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
    }

    public class PFLobbyUpdatedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyUpdatedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyUpdatedStateChange stateChangeConverted = ref stateChangeUnion.lobbyUpdated;
#else
                Interop.PFLobbyUpdatedStateChange stateChangeConverted = stateChangeUnion.lobbyUpdated;
#endif
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                this.ownerUpdated = stateChangeConverted.ownerUpdated;
                this.maxMembersUpdated = stateChangeConverted.maxMembersUpdated;
                this.accessPolicyUpdated = stateChangeConverted.accessPolicyUpdated;
                this.membershipLockUpdated = stateChangeConverted.membershipLockUpdated;
                this.updatedSearchPropertyKeys = Converters.StringPtrToArray(stateChangeConverted.updatedSearchPropertyKeys, stateChangeConverted.updatedSearchPropertyCount);
                this.updatedLobbyPropertyKeys = Converters.StringPtrToArray(stateChangeConverted.updatedLobbyPropertyKeys, stateChangeConverted.updatedLobbyPropertyCount);
                this.memberUpdates = new PFLobbyMemberUpdateSummary[stateChangeConverted.memberUpdateCount];
                for (int i = 0; i < stateChangeConverted.memberUpdateCount; i++)
                {
                    this.memberUpdates[i] = new PFLobbyMemberUpdateSummary(stateChangeConverted.memberUpdates[i]);
                }
            }
        }

        public PFLobbyHandle lobby { get; private set; }
        public bool ownerUpdated { get; private set; }
        public bool maxMembersUpdated { get; private set; }
        public bool accessPolicyUpdated { get; private set; }
        public bool membershipLockUpdated { get; private set; }
        public string[] updatedSearchPropertyKeys { get; private set; }
        public string[] updatedLobbyPropertyKeys { get; private set; }
        public PFLobbyMemberUpdateSummary[] memberUpdates { get; private set; }
    }

    public class PFLobbyPostUpdateCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyPostUpdateCompletedStateChange(PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId) :
            base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyPostUpdateCompletedStateChange stateChangeConverted = ref stateChangeUnion.postUpdateCompleted;
#else
                Interop.PFLobbyPostUpdateCompletedStateChange stateChangeConverted = stateChangeUnion.postUpdateCompleted;
#endif
                this.result = stateChangeConverted.result;
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey localUser = stateChangeConverted.localUser;
                this.localUser = new PFEntityKey(&localUser);
                this.asyncContext = null;
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
            }
        }

        public int result;
        public PFLobbyHandle lobby;
        public PFEntityKey localUser;
        public object asyncContext;
    }

    public class PFLobbyDisconnectingStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyDisconnectingStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyDisconnectingStateChange stateChangeConverted = ref stateChangeUnion.disconnecting;
#else
                Interop.PFLobbyDisconnectingStateChange stateChangeConverted = stateChangeUnion.disconnecting;
#endif
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                this.reason = (PFLobbyDisconnectingReason)stateChangeConverted.reason;
            }
        }

        public PFLobbyHandle lobby { get; private set; }
        public PFLobbyDisconnectingReason reason { get; private set; }
    }

    public class PFLobbyDisconnectedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyDisconnectedStateChange(
            PFLobbyStateChangeUnion stateChange, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChange.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyDisconnectedStateChange stateChangeConverted = ref stateChange.disconnected;
#else
                Interop.PFLobbyDisconnectedStateChange stateChangeConverted = stateChange.disconnected;
#endif
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
            }
        }

        public PFLobbyHandle lobby { get; private set; }
    }

    public class PFLobbyArrangedJoinCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyArrangedJoinCompletedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyJoinArrangedLobbyCompletedStateChange stateChangeConverted = ref stateChangeUnion.arrangedJoinCompleted;
#else
                Interop.PFLobbyArrangedJoinCompletedStateChange stateChangeConverted = stateChangeUnion.arrangedJoinCompleted;
#endif
                this.result = stateChangeConverted.result;
                Interop.PFEntityKey newMember = stateChangeConverted.newMember;
                this.newMember = new PFEntityKey(&newMember);
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
            }
        }

        public int result { get; private set; }
        public PFEntityKey newMember { get; private set; }
        public object asyncContext { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
    }

    public class PFLobbyFindLobbiesCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyFindLobbiesCompletedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyFindLobbiesCompletedStateChange stateChangeConverted = ref stateChangeUnion.findLobbiesCompleted;
#else
                Interop.PFLobbyFindLobbiesCompletedStateChange stateChangeConverted = stateChangeUnion.findLobbiesCompleted;
#endif
                this.result = stateChangeConverted.result;
                Interop.PFEntityKey searchingEntity = stateChangeConverted.searchingEntity;
                this.searchingEntity = new PFEntityKey(&searchingEntity);
                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }

                if(stateChangeConverted.searchResultCount > 0)
                {
                    PFLobbySearchResult[] searchResultsArray = new PFLobbySearchResult[stateChangeConverted.searchResultCount];
                    for (int i = 0; i < stateChangeConverted.searchResultCount; i++)
                    {
                        searchResultsArray[i] = new PFLobbySearchResult(&stateChangeConverted.searchResults[i]);
                    }
                    this.searchResults = searchResultsArray.ToList();
                }
                else
                {
                    this.searchResults = new List<PFLobbySearchResult>();
                }
            }
        }

        public PFLobbyStateChange stateChange { get; private set; }
        public int result { get; private set; }
        public PFEntityKey searchingEntity { get; private set; }
        public object asyncContext { get; private set; }
        public List<PFLobbySearchResult> searchResults { get; private set; }
    }

    public class PFLobbyInviteListenerStatusChangedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyInviteListenerStatusChangedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyInviteListenerStatusChangedStateChange stateChangeConverted = ref stateChangeUnion.inviteListenerStatusChanged;
#else
                Interop.PFLobbyInviteListenerStatusChangedStateChange stateChangeConverted = stateChangeUnion.inviteListenerStatusChanged;
#endif
                Interop.PFEntityKey listeningEntity = stateChangeConverted.listeningEntity;
                this.listeningEntity = new PFEntityKey(&listeningEntity);
            }
        }

        public PFEntityKey listeningEntity { get; private set; }
    }

    public class PFLobbyInviteReceivedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbyInviteReceivedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbyInviteReceivedStateChange stateChangeConverted = ref stateChangeUnion.inviteReceivedStateChange;
#else
                Interop.PFLobbyInviteReceivedStateChange stateChangeConverted = stateChangeUnion.inviteReceivedStateChange;
#endif
                Interop.PFEntityKey listeningEntity = stateChangeConverted.listeningEntity;
                this.listeningEntity = new PFEntityKey(&listeningEntity);
                Interop.PFEntityKey invitingEntity = stateChangeConverted.invitingEntity;
                this.invitingEntity = new PFEntityKey(&invitingEntity);
                this.connectionString = Converters.PtrToStringUTF8(stateChangeConverted.connectionString);
            }
        }

        public PFEntityKey listeningEntity { get; private set; }
        public PFEntityKey invitingEntity { get; private set; }
        public string connectionString { get; private set; }
    }

    public class PFLobbySendInviteCompletedStateChange : PFLobbyStateChange
    {
        unsafe internal PFLobbySendInviteCompletedStateChange(
            PFLobbyStateChangeUnion stateChangeUnion, Interop.PFLobbyStateChange* StateChangeId
            ) : base((PFLobbyStateChangeType)stateChangeUnion.stateChange.stateChangeType, StateChangeId)
        {
            unsafe
            {
#if CSHARP_7_OR_LATER
                ref readonly Interop.PFLobbySendInviteCompletedStateChange stateChangeConverted = ref stateChangeUnion.sendInviteCompleted;
#else
                Interop.PFLobbySendInviteCompletedStateChange stateChangeConverted = stateChangeUnion.sendInviteCompleted;
#endif
                this.result = stateChangeConverted.result;
                this.lobby = new PFLobbyHandle(stateChangeConverted.lobby);
                Interop.PFEntityKey sender = stateChangeConverted.sender;
                this.sender = new PFEntityKey(&sender);
                Interop.PFEntityKey invitee = stateChangeConverted.invitee;
                this.invitee = new PFEntityKey(&invitee);

                if (stateChangeConverted.asyncContext != null)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(new IntPtr(stateChangeConverted.asyncContext));
                    this.asyncContext = asyncGcHandle.Target;
                    asyncGcHandle.Free();
                }
            }
        }

        public int result { get; private set; }
        public PFLobbyHandle lobby { get; private set; }
        public PFEntityKey sender { get; private set; }
        public PFEntityKey invitee { get; private set; }
        public object asyncContext { get; private set; }
    }
}

