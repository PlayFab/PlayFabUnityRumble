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

    public partial class PFMultiplayer
    {
        public static int PFLobbyGetLobbyId(
            PFLobbyHandle lobby,
            out string id)
        {
            id = null;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                sbyte* idPtr;
                int err = Methods.PFLobbyGetLobbyId(
                    lobby.InteropHandle,
                    &idPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    id = Converters.PtrToStringUTF8(idPtr);
                }

                return err;
            }
        }

        public static int PFLobbyGetMaxMemberCount(
            PFLobbyHandle lobby,
            out uint maxMemberCount)
        {
            maxMemberCount = 0;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                uint maxMemberCountPtr = 0;
                int err = Methods.PFLobbyGetMaxMemberCount(lobby.InteropHandle, &maxMemberCountPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    maxMemberCount = maxMemberCountPtr;
                }

                return err;
            }
        }

        public static int PFLobbyGetOwner(
            PFLobbyHandle lobby,
            out PFEntityKey entityKey)
        {
            entityKey = null;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                Interop.PFEntityKey* entityKeyPtr = null;
                int err = Methods.PFLobbyGetOwner(
                    lobby.InteropHandle,
                    &entityKeyPtr);
                if (LobbyError.SUCCEEDED(err) && entityKeyPtr != null)
                {
                    entityKey = new PFEntityKey(entityKeyPtr);
                }

                return err;
            }
        }

        public static int PFLobbyGetOwnerMigrationPolicy(
            PFLobbyHandle lobby,
            out PFLobbyOwnerMigrationPolicy ownerMigrationPolicy)
        {
            ownerMigrationPolicy = PFLobbyOwnerMigrationPolicy.Manual;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                Interop.PFLobbyOwnerMigrationPolicy policy;
                int err = Methods.PFLobbyGetOwnerMigrationPolicy(lobby.InteropHandle, &policy);
                ownerMigrationPolicy = (PFLobbyOwnerMigrationPolicy)policy;
                return err;
            }
        }

        public static int PFLobbyGetAccessPolicy(
            PFLobbyHandle lobby,
            out PFLobbyAccessPolicy accessPolicy)
        {
            accessPolicy = PFLobbyAccessPolicy.Private;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                Interop.PFLobbyAccessPolicy policy;
                int err = Methods.PFLobbyGetAccessPolicy(lobby.InteropHandle, &policy);
                accessPolicy = (PFLobbyAccessPolicy)policy;
                return err;
            }
        }

        public static int PFLobbyGetMembershipLock(
            PFLobbyHandle lobby,
            out PFLobbyMembershipLock lockState)
        {
            lockState = PFLobbyMembershipLock.Unlocked;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                Interop.PFLobbyMembershipLock lockStateInterop;
                int err = Methods.PFLobbyGetMembershipLock(
                    lobby.InteropHandle,
                    &lockStateInterop);
                if (LobbyError.SUCCEEDED(err))
                {
                    lockState = (PFLobbyMembershipLock)lockStateInterop;
                }
                return err;
            }
        }

        public static int PFLobbyGetConnectionString(
            PFLobbyHandle lobby,
            out string connectionString)
        {
            connectionString = null;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                sbyte* connectionStringPtr;
                int err = Methods.PFLobbyGetConnectionString(
                    lobby.InteropHandle,
                    &connectionStringPtr);
                if (LobbyError.SUCCEEDED(err))
                {
                    connectionString = Converters.PtrToStringUTF8(connectionStringPtr);
                }

                return err;
            }
        }

        public static int PFLobbyGetMembers(
            PFLobbyHandle lobby,
            out PFEntityKey[] users)
        {
            users = null;
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                uint handleCount;
                Interop.PFEntityKey* handles;
                int err = Methods.PFLobbyGetMembers(lobby.InteropHandle, &handleCount, &handles);
                if (LobbyError.SUCCEEDED(err))
                {
                    users = new PFEntityKey[handleCount];
                    for (int i = 0; i < handleCount; i++)
                    {
                        users[i] = new PFEntityKey(&handles[i]);
                    }
                }

                return err;
            }
        }

        public static int PFLobbyLeave(
            PFLobbyHandle lobby,
            PFEntityKey localUser,
            object asyncIdentifier)
        {
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            using (var disposableCollection = new DisposableCollection())
            {
                unsafe
                {
                    var ptrLocalUser = localUser == null ? null : localUser.ToPointer(disposableCollection);
                    int err = Methods.PFLobbyLeave(
                        lobby.InteropHandle,
                        ptrLocalUser,
                        asyncId.ToPointer());
                    if (LobbyError.FAILED(err))
                    {
                        if (asyncId != IntPtr.Zero)
                        {
                            GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                            asyncGcHandle.Free();
                        }
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetSearchPropertyKeys(
            PFLobbyHandle lobby,
            out string[] keys)
        {
            unsafe
            {
                sbyte** keysPtr;
                uint propertyCount = 0;
                int err = Methods.PFLobbyGetSearchPropertyKeys(
                    lobby.InteropHandle,
                    &propertyCount,
                    &keysPtr);

                if (LobbyError.SUCCEEDED(err))
                {
                    keys = Converters.StringPtrToArray(keysPtr, propertyCount);
                }
                else
                {
                    keys = new string[0];
                }

                return err;
            }
        }

        public static int PFLobbyGetLobbyPropertyKeys(
            PFLobbyHandle lobby,
            out string[] keys)
        {
            unsafe
            {
                sbyte** keysPtr;
                uint propertyCount = 0;
                int err = Methods.PFLobbyGetLobbyPropertyKeys(
                    lobby.InteropHandle,
                    &propertyCount,
                    &keysPtr);

                if (LobbyError.SUCCEEDED(err))
                {
                    keys = Converters.StringPtrToArray(keysPtr, propertyCount);
                }
                else
                {
                    keys = new string[0];
                }

                return err;
            }
        }

        public static int PFLobbyGetMemberPropertyKeys(
            PFLobbyHandle lobby,
            PFEntityKey member,
            out string[] keys)
        {
            using (var disposableCollection = new DisposableCollection())
            {
                unsafe
                {
                    sbyte** keysPtr;
                    uint propertyCount = 0;

                    int err = Methods.PFLobbyGetMemberPropertyKeys(
                        lobby.InteropHandle,
                        member.ToPointer(disposableCollection),
                        &propertyCount,
                        &keysPtr);

                    if (LobbyError.SUCCEEDED(err))
                    {
                        keys = Converters.StringPtrToArray(keysPtr, propertyCount);
                    }
                    else
                    {
                        keys = new string[0];
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetSearchProperty(
            PFLobbyHandle lobby,
            string key,
            out string value)
        {
            value = null;
            if (lobby == null || key == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                using (var disposableCollection = new DisposableCollection())
                {
                    UTF8StringPtr keyPtr = new UTF8StringPtr(key, disposableCollection);
                    sbyte* valuePtr;
                    int err = Methods.PFLobbyGetSearchProperty(
                        lobby.InteropHandle,
                        keyPtr.Pointer,
                        &valuePtr);
                    if (LobbyError.SUCCEEDED(err))
                    {
                        value = Converters.PtrToStringUTF8(valuePtr);
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetLobbyProperty(
            PFLobbyHandle lobby,
            string key,
            out string value)
        {
            value = null;
            if (lobby == null || key == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                using (var disposableCollection = new DisposableCollection())
                {
                    UTF8StringPtr keyPtr = new UTF8StringPtr(key, disposableCollection);
                    sbyte* valuePtr;
                    int err = Methods.PFLobbyGetLobbyProperty(
                        lobby.InteropHandle,
                        keyPtr.Pointer,
                        &valuePtr);
                    if (LobbyError.SUCCEEDED(err))
                    {
                        value = Converters.PtrToStringUTF8(valuePtr);
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetMemberProperty(
            PFLobbyHandle lobby,
            PFEntityKey member,
            string key,
            out string value)
        {
            value = null;
            if (lobby == null || key == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                using (var disposableCollection = new DisposableCollection())
                {
                    UTF8StringPtr keyPtr = new UTF8StringPtr(key, disposableCollection);
                    sbyte* valuePtr;
                    int err = Methods.PFLobbyGetMemberProperty(
                        lobby.InteropHandle,
                        member.ToPointer(disposableCollection),
                        keyPtr.Pointer,
                        &valuePtr);
                    if (LobbyError.SUCCEEDED(err))
                    {
                        value = Converters.PtrToStringUTF8(valuePtr);
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetMemberConnectionStatus(
            PFLobbyHandle lobby,
            PFEntityKey member,
            out PFLobbyMemberConnectionStatus memberConnectionStatus)
        {
            memberConnectionStatus = PFLobbyMemberConnectionStatus.NotConnected;
            if (lobby == null || member == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                using (var disposableCollection = new DisposableCollection())
                {
                    Interop.PFLobbyMemberConnectionStatus connectionStatus;
                    int err = Methods.PFLobbyGetMemberConnectionStatus(
                        lobby.InteropHandle,
                        member.ToPointer(disposableCollection),
                        &connectionStatus);
                    memberConnectionStatus = (PFLobbyMemberConnectionStatus)connectionStatus;
                    return err;
                }
            }
        }        

        public static int PFLobbyPostUpdate(
            PFLobbyHandle lobby,
            PFEntityKey member,
            PFLobbyDataUpdate lobbyUpdate,
            PFLobbyMemberDataUpdate memberUpdate,
            object asyncIdentifier)
        {
            if (member == null || lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                unsafe
                {
                    Interop.PFLobbyDataUpdate* lobbyUpdateStructPtr = null;
                    if (lobbyUpdate != null)
                    {
                        lobbyUpdateStructPtr = lobbyUpdate.ToPointer(dc);
                    }

                    Interop.PFLobbyMemberDataUpdate* memberUpdateStructPtr = null;
                    if (memberUpdate != null)
                    {
                        memberUpdateStructPtr = memberUpdate.ToPointer(dc);
                    }

                    int err = Methods.PFLobbyPostUpdate(
                        lobby.InteropHandle,
                        member.ToPointer(dc),
                        lobbyUpdateStructPtr,
                        memberUpdateStructPtr,
                        asyncId.ToPointer());
                    if (LobbyError.FAILED(err))
                    {
                        if (asyncId != IntPtr.Zero)
                        {
                            GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                            asyncGcHandle.Free();
                        }
                    }

                    return err;
                }
            }
        }

        public static int PFLobbyGetCustomContext(
            PFLobbyHandle lobby,
            out object customContext)
        {
            if (lobby == null)
            {
                customContext = null;
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                void* contextPtr;
                int err = Methods.PFLobbyGetCustomContext(lobby.InteropHandle, &contextPtr);

                customContext = null;
                if (contextPtr != null)
                {
                    var handle = GCHandle.FromIntPtr(new IntPtr(contextPtr));
                    customContext = handle.Target;
                    handle.Free();
                }

                return err;
            }
        }

        public static int PFLobbySetCustomContext(
            PFLobbyHandle lobby,
            object customContext)
        {
            if (lobby == null)
            {
                return LobbyError.InvalidArg;
            }

            unsafe
            {
                // Free old context pointer
                void* contextPtrOld;
                int err = Methods.PFLobbyGetCustomContext(lobby.InteropHandle, &contextPtrOld);
                if (LobbyError.SUCCEEDED(err))
                {
                    var contextPtr = IntPtr.Zero;
                    if (customContext != null)
                    {
                        contextPtr = GCHandle.ToIntPtr(GCHandle.Alloc(customContext));
                    }

                    err = Methods.PFLobbySetCustomContext(lobby.InteropHandle, contextPtr.ToPointer());
                    if (LobbyError.SUCCEEDED(err))
                    {
                        if (contextPtrOld != null)
                        {
                            GCHandle contextGcHandle = GCHandle.FromIntPtr((IntPtr)contextPtrOld);
                            contextGcHandle.Free();
                        }
                    }
                    else
                    {
                        if (contextPtr != IntPtr.Zero)
                        {
                            GCHandle contextGcHandle = GCHandle.FromIntPtr(contextPtr);
                            contextGcHandle.Free();
                        }
                    }
                }

                return err;
            }
        }

        public static int PFLobbySendInvite(
            PFLobbyHandle lobby,
            PFEntityKey sender,
            PFEntityKey invitee,
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
                    int err = Methods.PFLobbySendInvite(
                        lobby.InteropHandle,
                        sender.ToPointer(dc),
                        invitee.ToPointer(dc),
                        asyncContextPtr.ToPointer());
                    return err;
                }
            }
        }
    }
}
