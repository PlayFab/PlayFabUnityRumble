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

    public class PFLobbyCreateConfiguration
    {
        public PFLobbyCreateConfiguration()
        {
            this.SearchProperties = new Dictionary<string, string>();
            this.LobbyProperties = new Dictionary<string, string>();
        }

        internal unsafe PFLobbyCreateConfiguration(Interop.PFLobbyCreateConfiguration interopStruct)
        {
            this.MaxMemberCount = interopStruct.maxMemberCount;
            this.OwnerMigrationPolicy = (PFLobbyOwnerMigrationPolicy)interopStruct.ownerMigrationPolicy;
            this.AccessPolicy = (PFLobbyAccessPolicy)interopStruct.accessPolicy;

            string[] searchPropertyKeys = Converters.StringPtrToArray(interopStruct.searchPropertyKeys, interopStruct.searchPropertyCount);
            string[] searchPropertyValues = Converters.StringPtrToArray(interopStruct.searchPropertyValues, interopStruct.searchPropertyCount);
            if (searchPropertyKeys.Length == searchPropertyValues.Length)
            {
                this.SearchProperties = Enumerable.Range(0, searchPropertyKeys.Length).ToDictionary(
                    i => searchPropertyKeys[i],
                    i => searchPropertyValues[i]);
            }
            else
            {
                throw new IndexOutOfRangeException("searchPropertyKeys and searchPropertyValues don't have same length");
            }

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

        public uint MaxMemberCount { get; set; }

        public PFLobbyOwnerMigrationPolicy OwnerMigrationPolicy { get; set; }

        public PFLobbyAccessPolicy AccessPolicy { get; set; }

        public IDictionary<string, string> SearchProperties { get; set; }

        public IDictionary<string, string> LobbyProperties { get; set; }

        internal unsafe Interop.PFLobbyCreateConfiguration* ToPointer(DisposableCollection disposableCollection)
        {
            Interop.PFLobbyCreateConfiguration interopPtr = new Interop.PFLobbyCreateConfiguration();

            interopPtr.maxMemberCount = this.MaxMemberCount;
            interopPtr.ownerMigrationPolicy = (Interop.PFLobbyOwnerMigrationPolicy)this.OwnerMigrationPolicy;
            interopPtr.accessPolicy = (Interop.PFLobbyAccessPolicy)this.AccessPolicy;

            SizeT count;
            interopPtr.searchPropertyCount = Convert.ToUInt32(this.SearchProperties.Count);
            interopPtr.searchPropertyKeys = (sbyte**)Converters.StringArrayToUTF8StringArray(this.SearchProperties.Keys.ToArray(), disposableCollection, out count);
            interopPtr.searchPropertyValues = (sbyte**)Converters.StringArrayToUTF8StringArray(this.SearchProperties.Values.ToArray(), disposableCollection, out count);

            interopPtr.lobbyPropertyCount = Convert.ToUInt32(this.LobbyProperties.Count);
            interopPtr.lobbyPropertyKeys = (sbyte**)Converters.StringArrayToUTF8StringArray(this.LobbyProperties.Keys.ToArray(), disposableCollection, out count);
            interopPtr.lobbyPropertyValues = (sbyte**)Converters.StringArrayToUTF8StringArray(this.LobbyProperties.Values.ToArray(), disposableCollection, out count);

            return (Interop.PFLobbyCreateConfiguration*)Converters.StructToPtr<Interop.PFLobbyCreateConfiguration>(interopPtr, disposableCollection);
        }
    }
}
