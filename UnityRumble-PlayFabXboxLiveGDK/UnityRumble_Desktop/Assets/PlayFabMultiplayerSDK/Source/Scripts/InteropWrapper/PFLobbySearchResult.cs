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

    public class PFLobbySearchResult
    {
        internal unsafe PFLobbySearchResult(Interop.PFLobbySearchResult* interopStruct)
        {
            this.LobbyId = Converters.PtrToStringUTF8((IntPtr)interopStruct->lobbyId);
            this.ConnectionString = Converters.PtrToStringUTF8((IntPtr)interopStruct->connectionString);
            this.OwnerEntity = new PFEntityKey(interopStruct->ownerEntity);
            this.MaxMemberCount = interopStruct->maxMemberCount;
            this.CurrentMemberCount = interopStruct->currentMemberCount;

            if (interopStruct->searchPropertyCount > 0)
            {
                string[] searchPropertyKeys = Converters.StringPtrToArray(interopStruct->searchPropertyKeys, interopStruct->searchPropertyCount);
                string[] searchPropertyValues = Converters.StringPtrToArray(interopStruct->searchPropertyValues, interopStruct->searchPropertyCount);
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
            }
            else
            {
                this.SearchProperties = new Dictionary<string, string>();
            }

            if (interopStruct->friendCount > 0)
            {
                PFEntityKey[] friends = new PFEntityKey[interopStruct->friendCount];
                for (int i = 0; i < interopStruct->friendCount; i++)
                {
                    friends[i] = new PFEntityKey(&interopStruct->friends[i]);
                }

                this.Friends = friends.ToList();
            }
            else
            {
                this.Friends = new List<PFEntityKey>();
            }
        }

        public string LobbyId { get; set; }

        public string ConnectionString { get; set; }

        public PFEntityKey OwnerEntity { get; set; }

        public uint MaxMemberCount { get; set; }

        public uint CurrentMemberCount { get; set; }

        public Dictionary<string, string> SearchProperties { get; set; }

        public List<PFEntityKey> Friends { get; set; }
    }
}
