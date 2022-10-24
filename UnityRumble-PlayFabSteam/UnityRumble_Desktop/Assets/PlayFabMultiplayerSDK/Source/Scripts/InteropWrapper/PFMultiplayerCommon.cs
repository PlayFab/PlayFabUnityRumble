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
        public const uint PFLobbyMaxMemberCountLowerLimit = Interop.Methods.PFLobbyMaxMemberCountLowerLimit;
        public const uint PFLobbyMaxMemberCountUpperLimit = Interop.Methods.PFLobbyMaxMemberCountUpperLimit;
        public const uint PFLobbyMaxSearchPropertyCount = Interop.Methods.PFLobbyMaxSearchPropertyCount;
        public const uint PFLobbyMaxLobbyPropertyCount = Interop.Methods.PFLobbyMaxLobbyPropertyCount;
        public const uint PFLobbyMaxMemberPropertyCount = Interop.Methods.PFLobbyMaxMemberPropertyCount;
        public const uint PFLobbyClientRequestedSearchResultCountUpperLimit = Interop.Methods.PFLobbyClientRequestedSearchResultCountUpperLimit;

        public static string PFMultiplayerGetErrorMessage(
            int hresult)
        {
            unsafe
            {
                sbyte* errorMessagePtr = Methods.PFMultiplayerGetErrorMessage(hresult);
                if (errorMessagePtr != null)
                {
                    return Converters.PtrToStringUTF8((IntPtr)errorMessagePtr);
                }

                return null;
            }
        }

        public static int PFMultiplayerInitialize(
            string titleId,
            out PFMultiplayerHandle handle)
        {
            using (var disposableCollection = new DisposableCollection())
            {
                unsafe
                {
                    Interop.PFMultiplayer* interopUserHandle;
                    var titlePtr = new UTF8StringPtr(titleId, disposableCollection);

                    int err = Methods.PFMultiplayerInitialize(
                        titlePtr.Pointer,
                        &interopUserHandle);

                    const int XBOX_E_MULTIPLAYER_API_ALREADY_INITIALIZED = unchecked((int)0x89236401);
                    if (err == XBOX_E_MULTIPLAYER_API_ALREADY_INITIALIZED)
                    {
                        Methods.PFMultiplayerUninitialize(null);
                        err = Methods.PFMultiplayerInitialize(
                            titlePtr.Pointer,
                            &interopUserHandle);
                    }

                    return PFMultiplayerHandle.WrapAndReturnError(err, interopUserHandle, out handle);
                }
            }
        }

        public static int PFMultiplayerUninitialize(
            PFMultiplayerHandle handle)
        {
            unsafe
            {
                return Methods.PFMultiplayerUninitialize(handle.InteropHandle);
            }
        }

        public static int PFMultiplayerSetThreadAffinityMask(
            PFMultiplayerThreadId threadId,
            ulong threadAffinityMask)
        {
            return Methods.PFMultiplayerSetThreadAffinityMask(
                (Interop.PFMultiplayerThreadId)threadId,
                threadAffinityMask);
        }

        public static int PFMultiplayerSetEntityToken(
            PFMultiplayerHandle handle,
            PFEntityKey localMember,
            string entityToken)
        {
            unsafe
            {
                using (var disposableCollection = new DisposableCollection())
                {
                    UTF8StringPtr entityTokenPtr = new UTF8StringPtr(entityToken, disposableCollection);
                    return Methods.PFMultiplayerSetEntityToken(
                        handle.InteropHandle,
                        localMember.ToPointer(disposableCollection),
                        entityTokenPtr.Pointer);
                }
            }
        }
    }
}
