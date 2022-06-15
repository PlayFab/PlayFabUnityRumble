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

    public class PFLobbyArrangedJoinConfiguration
    {
        public uint MaxMemberCount { get; set; }

        public PFLobbyOwnerMigrationPolicy OwnerMigrationPolicy { get; set; }

        public PFLobbyAccessPolicy AccessPolicy { get; set; }

        public IDictionary<string, string> MemberProperties { get; set; }

        internal unsafe Interop.PFLobbyArrangedJoinConfiguration* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFLobbyArrangedJoinConfiguration interopPtr = new Interop.PFLobbyArrangedJoinConfiguration();

            interopPtr.maxMemberCount = (uint)this.MaxMemberCount;
            interopPtr.ownerMigrationPolicy = (Interop.PFLobbyOwnerMigrationPolicy)this.OwnerMigrationPolicy;
            interopPtr.accessPolicy = (Interop.PFLobbyAccessPolicy)this.AccessPolicy;

            SizeT count;
            interopPtr.memberPropertyCount = this.MemberProperties != null ? Convert.ToUInt32(this.MemberProperties.Count) : 0;
            interopPtr.memberPropertyKeys = interopPtr.memberPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.MemberProperties.Keys.ToArray(), disposableCollection, out count) : null;
            interopPtr.memberPropertyValues = interopPtr.memberPropertyCount > 0 ? (sbyte**)Converters.StringArrayToUTF8StringArray(this.MemberProperties.Values.ToArray(), disposableCollection, out count) : null;

            return (Interop.PFLobbyArrangedJoinConfiguration*)Converters.StructToPtr<Interop.PFLobbyArrangedJoinConfiguration>(interopPtr, disposableCollection);
        }
    }
}
