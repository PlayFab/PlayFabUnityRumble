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

    public class PFLobbyDataUpdate
    {
        public PFLobbyDataUpdate()
        {
            this.LobbyProperties = new Dictionary<string, string>();
        }

        internal unsafe PFLobbyDataUpdate(Interop.PFLobbyDataUpdate interopStruct)
        {
            this.NewOwner = new PFEntityKey(interopStruct.newOwner);
            this.MaxMemberCount = Converters.PtrToStruct<uint>((IntPtr)interopStruct.maxMemberCount);
            this.AccessPolicy = Converters.PtrToStruct<LobbyAccessPolicy>((IntPtr)interopStruct.accessPolicy);
            this.MembershipLock = Converters.PtrToStruct<LobbyMembershipLock>((IntPtr)interopStruct.membershipLock);

            string[] lobbyPropertyKeys = Converters.StringPtrToArray(interopStruct.lobbyPropertyKeys, interopStruct.lobbyPropertyCount);
            string[] lobbyPropertyValues = Converters.StringPtrToArray(interopStruct.lobbyPropertyValues, interopStruct.lobbyPropertyCount);
            if (lobbyPropertyKeys.Length == lobbyPropertyValues.Length)
            {
                this.LobbyProperties = Enumerable.Range(0, lobbyPropertyKeys.Length).ToDictionary(
                    i => lobbyPropertyKeys[i],
                    i => lobbyPropertyValues[i]);
            }
            else
            {
                throw new IndexOutOfRangeException("lobbyPropertyKeys and lobbyPropertyValues don't have same length");
            }
        }

        public PFEntityKey NewOwner { get; set; }

        public uint? MaxMemberCount { get; set; }

        public LobbyAccessPolicy? AccessPolicy { get; set; }

        public LobbyMembershipLock? MembershipLock { get; set; }

        public IDictionary<string, string> SearchProperties { get; set; }

        public IDictionary<string, string> LobbyProperties { get; set; }

        internal unsafe Interop.PFLobbyDataUpdate* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFLobbyDataUpdate interopPtr = new Interop.PFLobbyDataUpdate();

            interopPtr.newOwner = this.NewOwner != null ? this.NewOwner.ToPointer(disposableCollection) : null;
            interopPtr.maxMemberCount = this.MaxMemberCount.HasValue ? (uint*)Converters.StructToPtr<uint>(this.MaxMemberCount.Value, disposableCollection) : null;
            interopPtr.accessPolicy = this.AccessPolicy.HasValue ? (Interop.PFLobbyAccessPolicy*)Converters.StructToPtr<Interop.PFLobbyAccessPolicy>((Interop.PFLobbyAccessPolicy)this.AccessPolicy.Value, disposableCollection) : null;
            interopPtr.membershipLock = this.MembershipLock.HasValue ? (Interop.PFLobbyMembershipLock*)Converters.StructToPtr<Interop.PFLobbyMembershipLock>((Interop.PFLobbyMembershipLock)this.MembershipLock.Value, disposableCollection) : null;

            SizeT count;
            interopPtr.searchPropertyCount = this.SearchProperties != null ? Convert.ToUInt32(this.SearchProperties.Count) : 0;
            interopPtr.searchPropertyKeys = interopPtr.searchPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.SearchProperties.Keys.ToArray(), disposableCollection, out count) : null;
            interopPtr.searchPropertyValues = interopPtr.searchPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.SearchProperties.Values.ToArray(), disposableCollection, out count) : null;

            interopPtr.lobbyPropertyCount = this.LobbyProperties != null ? Convert.ToUInt32(this.LobbyProperties.Count) : 0;
            interopPtr.lobbyPropertyKeys = interopPtr.lobbyPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.LobbyProperties.Keys.ToArray(), disposableCollection, out count) : null;
            interopPtr.lobbyPropertyValues = interopPtr.lobbyPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.LobbyProperties.Values.ToArray(), disposableCollection, out count) : null;

            return (Interop.PFLobbyDataUpdate*)Converters.StructToPtr<Interop.PFLobbyDataUpdate>(interopPtr, disposableCollection);
        }
    }
}
