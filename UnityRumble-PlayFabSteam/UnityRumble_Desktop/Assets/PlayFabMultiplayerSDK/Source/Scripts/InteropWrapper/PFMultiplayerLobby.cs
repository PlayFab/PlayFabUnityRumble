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
    using System.Linq;
    using System.Runtime.InteropServices;
    using PlayFab.Multiplayer.Interop;

    public struct LobbyStateChangeCollection
    {
        public List<PFLobbyStateChange> StateChanges;
        public uint StateChangeCount;
        internal unsafe PlayFab.Multiplayer.Interop.PFLobbyStateChange** RawStateChanges;
    }

    public partial class PFMultiplayer
    {
        static PFMultiplayer()
        {
            ObjPool = new ObjectPool();

            // Limit is arbitrary
            ObjPool.AddEntry<List<PFLobbyStateChange>>(4, new Type[] { });
            ObjPool.AddEntry<List<PFMatchmakingStateChange>>(4, new Type[] { });
        }

        // For storing high frequency objects
        internal static ObjectPool ObjPool { get; set; }

        public static int PFMultiplayerStartProcessingLobbyStateChanges(
            PFMultiplayerHandle handle,
            out LobbyStateChangeCollection collection)
        {
            uint stateChangeCount = 0;

            collection.StateChanges = ObjPool.Retrieve<List<PFLobbyStateChange>>();
            unsafe
            {
                PlayFab.Multiplayer.Interop.PFLobbyStateChange** rawStateChanges = null;

                int err = Methods.PFMultiplayerStartProcessingLobbyStateChanges(
                    handle.InteropHandle,
                    &stateChangeCount,
                    &rawStateChanges);

                collection.RawStateChanges = rawStateChanges;
                collection.StateChangeCount = stateChangeCount;

                if (LobbyError.SUCCEEDED(err) && stateChangeCount > 0)
                {
                    for (int i = 0; i < stateChangeCount; i++)
                    {
                        PFLobbyStateChange stateChangeObj = PFLobbyStateChange.CreateFromPtr(rawStateChanges[i]);
                        if (stateChangeObj.GetType() != typeof(PFLobbyStateChange))
                        {
                            collection.StateChanges.Add(stateChangeObj);
                        }
                    }
                }

                return err;
            }
        }

        public static unsafe int PFMultiplayerFinishProcessingLobbyStateChanges(
            PFMultiplayerHandle handle,
            LobbyStateChangeCollection collection)
        {
            if (handle == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                collection.StateChanges.Clear();
                ObjPool.Return(collection.StateChanges);

                int err = Methods.PFMultiplayerFinishProcessingLobbyStateChanges(
                    handle.InteropHandle,
                    collection.StateChangeCount,
                    collection.RawStateChanges);
                return err;
            }
        }

        public static int PFMultiplayerCreateAndJoinLobby(
            PFMultiplayerHandle handle,
            PFEntityKey creator,
            PFLobbyCreateConfiguration createConfiguration,
            PFLobbyJoinConfiguration joinConfiguration,
            object asyncIdentifier,
            out PFLobbyHandle lobby)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    IntPtr asyncId = IntPtr.Zero;
                    if (asyncIdentifier != null)
                    {
                        asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                    }

                    PFLobby* lobbyPtr = null;
                    void* asyncContext = asyncId.ToPointer();
                    int err = Methods.PFMultiplayerCreateAndJoinLobby(
                        handle.InteropHandle,
                        creator.ToPointer(dc),
                        createConfiguration.ToPointer(dc),
                        joinConfiguration.ToPointer(dc),
                        asyncContext,
                        &lobbyPtr);
                    if (LobbyError.FAILED(err))
                    {
                        if (asyncId != IntPtr.Zero)
                        {
                            GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                            asyncGcHandle.Free();
                        }
                    }

                    lobby = new PFLobbyHandle(lobbyPtr);
                    return err;
                }
            }
        }

        public static int PFLobbyForceRemoveMember(
            PFLobbyHandle lobby,
            PFEntityKey targetMember,
            bool preventRejoin,
            object asyncContext)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    IntPtr asyncContextPtr = IntPtr.Zero;
                    if (asyncContext != null)
                    {
                        asyncContextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(asyncContext));
                    }

                    int err = Methods.PFLobbyForceRemoveMember(
                        lobby.InteropHandle,
                        targetMember.ToPointer(dc),
                        (byte)(preventRejoin ? 1 : 0),
                        asyncContextPtr.ToPointer());
                    return err;
                }
            }
        }

        public static int PFLobbyAddMember(
            PFLobbyHandle lobby,
            PFEntityKey localUser,
            IDictionary<string, string> memberProperties,
            object asyncContext)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    IntPtr asyncContextPtr = IntPtr.Zero;
                    if (asyncContext != null)
                    {
                        asyncContextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(asyncContext));
                    }

                    SizeT count;
                    uint memberPropertyCount = Convert.ToUInt32(memberProperties.Count);
                    var memberPropertyKeys = (sbyte**)Converters.StringArrayToUTF8StringArray(memberProperties.Keys.ToArray(), dc, out count);
                    var memberPropertyValues = (sbyte**)Converters.StringArrayToUTF8StringArray(memberProperties.Values.ToArray(), dc, out count);

                    int err = Methods.PFLobbyAddMember(
                        lobby.InteropHandle,
                        localUser.ToPointer(dc),
                        memberPropertyCount,
                        memberPropertyKeys,
                        memberPropertyValues,
                        asyncContextPtr.ToPointer());
                    return err;
                }
            }
        }

        public static int PFMultiplayerJoinLobby(
            PFMultiplayerHandle handle,
            PFEntityKey newMember,
            string connectionString,
            PFLobbyJoinConfiguration configuration,
            object asyncContext,
            out PFLobbyHandle lobby)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    IntPtr asyncContextPtr = IntPtr.Zero;
                    if (asyncContext != null)
                    {
                        asyncContextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(asyncContext));
                    }

                    UTF8StringPtr connectionStringPtr = new UTF8StringPtr(connectionString, dc);
                    PFLobby* lobbyPtr = null;
                    int err = Methods.PFMultiplayerJoinLobby(
                        handle.InteropHandle,
                        newMember.ToPointer(dc),
                        connectionStringPtr.Pointer,
                        configuration.ToPointer(dc),
                        asyncContextPtr.ToPointer(),
                        &lobbyPtr);
                    if (LobbyError.FAILED(err))
                    {
                        if (asyncContextPtr != IntPtr.Zero)
                        {
                            GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncContextPtr);
                            asyncGcHandle.Free();
                        }
                    }

                    lobby = new PFLobbyHandle(lobbyPtr);
                    return err;
                }
            }
        }

        public static int PFMultiplayerJoinArrangedLobby(
            PFMultiplayerHandle handle,
            PFEntityKey newMember,
            string arrangementString,
            PFLobbyArrangedJoinConfiguration configuration,
            object asyncContext,
            out PFLobbyHandle lobby)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                IntPtr asyncContextPtr = IntPtr.Zero;
                if (asyncContext != null)
                {
                    asyncContextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(asyncContext));
                }

                unsafe
                {
                    UTF8StringPtr arrangementStringPtr = new UTF8StringPtr(arrangementString, dc);
                    PFLobby* lobbyPtr = null;
                    int err = Methods.PFMultiplayerJoinArrangedLobby(
                        handle.InteropHandle,
                        newMember.ToPointer(dc),
                        arrangementStringPtr.Pointer,
                        configuration.ToPointer(dc),
                        asyncContextPtr.ToPointer(),
                        &lobbyPtr);
                    if (LobbyError.FAILED(err))
                    {
                        if (asyncContextPtr != IntPtr.Zero)
                        {
                            GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncContextPtr);
                            asyncGcHandle.Free();
                        }
                    }

                    lobby = new PFLobbyHandle(lobbyPtr);
                    return err;
                }
            }
        }

        public static int PFMultiplayerFindLobbies(
            PFMultiplayerHandle handle,
            PFEntityKey searchingEntity,
            PFLobbySearchConfiguration searchConfiguration,
            object asyncContext)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                IntPtr asyncContextPtr = IntPtr.Zero;
                if (asyncContext != null)
                {
                    asyncContextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(asyncContext));
                }

                unsafe
                {
                    int err = Methods.PFMultiplayerFindLobbies(
                        handle.InteropHandle,
                        searchingEntity.ToPointer(dc),
                        searchConfiguration.ToPointer(dc),
                        asyncContextPtr.ToPointer());
                    return err;
                }
            }
        }

        public static int PFMultiplayerStartListeningForLobbyInvites(
            PFMultiplayerHandle handle, 
            PFEntityKey listeningEntity)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    int err = Methods.PFMultiplayerStartListeningForLobbyInvites(
                        handle.InteropHandle,
                        listeningEntity.ToPointer(dc));
                    return err;
                }
            }
        }

        public static int PFMultiplayerStopListeningForLobbyInvites(
            PFMultiplayerHandle handle, 
            PFEntityKey listeningEntity)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    int err = Methods.PFMultiplayerStopListeningForLobbyInvites(
                        handle.InteropHandle,
                        listeningEntity.ToPointer(dc));
                    return err;
                }
            }
        }

        public static int PFMultiplayerGetLobbyInviteListenerStatus(
            PFMultiplayerHandle handle, 
            PFEntityKey listeningEntity,
            out PFLobbyInviteListenerStatus status)
        {
            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    Interop.PFLobbyInviteListenerStatus interopStatus;
                    int err = Methods.PFMultiplayerGetLobbyInviteListenerStatus(
                        handle.InteropHandle,
                        listeningEntity.ToPointer(dc),
                        &interopStatus);
                    status = (PFLobbyInviteListenerStatus)interopStatus;
                    return err;
                }
            }
        }
    }
}
