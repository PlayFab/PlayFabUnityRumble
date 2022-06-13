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
    using PlayFab.Multiplayer.Interop;

    public struct MatchmakingStateChangeCollection
    {
        public List<PFMatchmakingStateChange> StateChanges;
        public uint StateChangeCount;
        internal unsafe PlayFab.Multiplayer.Interop.PFMatchmakingStateChange** RawStateChanges;
    }

    public partial class PFMultiplayer
    {
        public static int PFMultiplayerCreateMatchmakingTicket(
            PFMultiplayerHandle multiplayer,
            PFEntityKey[] localUsers,
            string[] localUserAttributes,
            PFMatchmakingTicketConfiguration configuration,
            object asyncIdentifier,
            out PFMatchmakingTicketHandle handle)
        {
            using (var disposableCollection = new DisposableCollection())
            {
                unsafe
                {
                    Interop.PFMatchmakingTicket* matchTicketHandle;

                    sbyte*[] localUserAttributesPtrs = Converters.StringArrayToPtr(localUserAttributes, disposableCollection);
                    Interop.PFEntityKey[] localUsersPtrs = new Interop.PFEntityKey[localUsers.Length];
                    for (int i = 0; i < localUsers.Length; i++)
                    {
                        UTF8StringPtr idPtr = new UTF8StringPtr(localUsers[i].Id, disposableCollection);
                        UTF8StringPtr typePtr = new UTF8StringPtr(localUsers[i].Type, disposableCollection);
                        localUsersPtrs[i].id = idPtr.Pointer;
                        localUsersPtrs[i].type = typePtr.Pointer;
                    }

                    var asyncId = IntPtr.Zero;
                    if (asyncIdentifier != null)
                    {
                        asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                    }

                    fixed (Interop.PFEntityKey* localUsersPtrsArray = &localUsersPtrs[0])
                    {
                        fixed (sbyte** localUserAttributesPtrsArray = &localUserAttributesPtrs[0])
                        {
                            int err = Methods.PFMultiplayerCreateMatchmakingTicket(
                                multiplayer.InteropHandle,
                                (uint)localUsers.Length,
                                localUsersPtrsArray,
                                localUserAttributesPtrsArray,
                                configuration.ToPointer(disposableCollection),
                                asyncId.ToPointer(),
                                &matchTicketHandle);

                            if (LobbyError.FAILED(err))
                            {
                                if (asyncId != IntPtr.Zero)
                                {
                                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                                    asyncGcHandle.Free();
                                }
                            }

                            return PFMatchmakingTicketHandle.WrapAndReturnError(err, matchTicketHandle, out handle);
                        }
                    }
                }
            }
        }

        public static int PFMultiplayerJoinMatchmakingTicketFromId(
            PFMultiplayerHandle multiplayer,
            PFEntityKey[] localUsers,
            string[] localUserAttributes,
            string ticketId,
            string queueName,
            object asyncIdentifier,
            out PFMatchmakingTicketHandle handle)
        {
            using (var disposableCollection = new DisposableCollection())
            {
                unsafe
                {
                    Interop.PFMatchmakingTicket* matchTicketHandle;

                    sbyte*[] localUserAttributesPtrs = Converters.StringArrayToPtr(localUserAttributes, disposableCollection);
                    Interop.PFEntityKey[] localUsersPtrs = new Interop.PFEntityKey[localUsers.Length];
                    for (int i = 0; i < localUsers.Length; i++)
                    {
                        UTF8StringPtr idPtr = new UTF8StringPtr(localUsers[i].Id, disposableCollection);
                        UTF8StringPtr typePtr = new UTF8StringPtr(localUsers[i].Type, disposableCollection);
                        localUsersPtrs[i].id = idPtr.Pointer;
                        localUsersPtrs[i].type = typePtr.Pointer;
                    }

                    var asyncId = IntPtr.Zero;
                    if (asyncIdentifier != null)
                    {
                        asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                    }

                    fixed (Interop.PFEntityKey* localUsersPtrsArray = &localUsersPtrs[0])
                    {
                        fixed (sbyte** localUserAttributesPtrsArray = &localUserAttributesPtrs[0])
                        {
                            UTF8StringPtr ticketIdPtr = new UTF8StringPtr(ticketId, disposableCollection);
                            UTF8StringPtr queueNamePtr = new UTF8StringPtr(queueName, disposableCollection);
                            int err = Methods.PFMultiplayerJoinMatchmakingTicketFromId(
                                multiplayer.InteropHandle,
                                (uint)localUsers.Length,
                                localUsersPtrsArray,
                                localUserAttributesPtrsArray,
                                ticketIdPtr.Pointer,
                                queueNamePtr.Pointer,
                                asyncId.ToPointer(),
                                &matchTicketHandle);

                            if (LobbyError.FAILED(err))
                            {
                                if (asyncId != IntPtr.Zero)
                                {
                                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                                    asyncGcHandle.Free();
                                }
                            }

                            return PFMatchmakingTicketHandle.WrapAndReturnError(err, matchTicketHandle, out handle);
                        }
                    }
                }
            }
        }

        public static int PFMultiplayerDestroyMatchmakingTicket(
            PFMultiplayerHandle multiplayer,
            PFMatchmakingTicketHandle matchTicketHandle)
        {
            unsafe
            {
                int err = Methods.PFMultiplayerDestroyMatchmakingTicket(
                    multiplayer.InteropHandle,
                    matchTicketHandle.InteropHandle);
                return err;
            }
        }

        public static int PFMatchmakingTicketGetStatus(
            PFMatchmakingTicketHandle matchTicketHandle,
            out PFMatchmakingTicketStatus status)
        {
            unsafe
            {
                status = PFMatchmakingTicketStatus.Failed;
                Interop.PFMatchmakingTicketStatus statusPtr;
                int err = Methods.PFMatchmakingTicketGetStatus(matchTicketHandle.InteropHandle, &statusPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    status = (PFMatchmakingTicketStatus)statusPtr;
                }

                return err;
            }
        }

        public static int PFMatchmakingTicketCancel(
            PFMatchmakingTicketHandle matchTicketHandle)
        {
            unsafe
            {
                return Methods.PFMatchmakingTicketCancel(
                    matchTicketHandle.InteropHandle);
            }
        }

        public static int PFMatchmakingTicketGetTicketId(
            PFMatchmakingTicketHandle matchTicketHandle,
            out string ticketId)
        {
            unsafe
            {
                ticketId = null;

                sbyte* ticketIdPtr;
                int err = Methods.PFMatchmakingTicketGetTicketId(matchTicketHandle.InteropHandle, &ticketIdPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    ticketId = Converters.PtrToStringUTF8(ticketIdPtr);
                }

                return err;
            }
        }

        public static int PFMatchmakingTicketGetMatch(
            PFMatchmakingTicketHandle matchTicketHandle,
            out PFMatchmakingMatchDetails matchDetails)
        {
            unsafe
            {
                matchDetails = null;
                Interop.PFMatchmakingMatchDetails* detailsPtr;
                int err = Methods.PFMatchmakingTicketGetMatch(matchTicketHandle.InteropHandle, &detailsPtr);
                if (LobbyError.SUCCEEDED(err) && detailsPtr != null)
                {
                    matchDetails = new PFMatchmakingMatchDetails(detailsPtr);
                }

                return err;
            }
        }

        public static int PFMultiplayerStartProcessingMatchmakingStateChanges(
            PFMultiplayerHandle handle,
            out MatchmakingStateChangeCollection collection)
        {
            uint stateChangeCount = 0;

            collection.StateChanges = ObjPool.Retrieve<List<PFMatchmakingStateChange>>();
            unsafe
            {
                PlayFab.Multiplayer.Interop.PFMatchmakingStateChange** rawStateChanges = null;

                rawStateChanges = null;
                stateChangeCount = 0;
                int err = Methods.PFMultiplayerStartProcessingMatchmakingStateChanges(
                    handle.InteropHandle,
                    &stateChangeCount,
                    &rawStateChanges);

                collection.RawStateChanges = rawStateChanges;
                collection.StateChangeCount = stateChangeCount;

                if (LobbyError.SUCCEEDED(err) && stateChangeCount > 0)
                {
                    for (int i = 0; i < stateChangeCount; i++)
                    {
                        PFMatchmakingStateChange stateChangeObj = PFMatchmakingStateChange.CreateFromPtr(rawStateChanges[i]);
                        if (stateChangeObj.GetType() != typeof(PFMatchmakingStateChange))
                        {
                            collection.StateChanges.Add(stateChangeObj);
                        }
                    }
                }

                return err;
            }
        }

        public static unsafe int PFMultiplayerFinishProcessingMatchmakingStateChanges(
            PFMultiplayerHandle handle,
            MatchmakingStateChangeCollection collection)
        {
            if (handle == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                collection.StateChanges.Clear();
                ObjPool.Return(collection.StateChanges); 

                int err = Methods.PFMultiplayerFinishProcessingMatchmakingStateChanges(
                    handle.InteropHandle,
                    collection.StateChangeCount,
                    collection.RawStateChanges);
                return err;
            }
        }
    }
}
