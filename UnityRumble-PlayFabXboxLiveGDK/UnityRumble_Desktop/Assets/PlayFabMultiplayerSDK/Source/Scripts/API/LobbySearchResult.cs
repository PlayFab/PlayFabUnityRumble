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
    using System.Linq;

    /// <summary>
    /// An entry in the collection of lobby search results received upon successful completion of a
    /// <see cref="PlayFabMultiplayer.FindLobbies" /> operation.
    /// </summary>
    public class LobbySearchResult
    {
        private InteropWrapper.PFLobbySearchResult searchResult;

        private PFEntityKey owner;

        private List<PFEntityKey> friendsList;

        internal LobbySearchResult(InteropWrapper.PFLobbySearchResult searchResult)
        {
            this.searchResult = searchResult;
            this.owner = new PFEntityKey(searchResult.OwnerEntity);

            this.friendsList = new List<PFEntityKey>();
            foreach (var friend in searchResult.Friends)
            {
                PFEntityKey apiFriend = new PFEntityKey(friend);
                this.Friends.Add(apiFriend);
            }
        }

        /// <summary>
        /// The ID of the found lobby.
        /// </summary>
        public string LobbyId
        {
            get
            {
                return this.searchResult.LobbyId;
            }
        }

        /// <summary>
        /// The connection string of the found lobby.
        /// </summary>
        /// <remarks>
        /// <c>LobbySearchResult.ConnectionString</c> can be null. In this case, an invite is required to join.
        /// </remarks>
        public string ConnectionString
        {
            get
            {
                return this.searchResult.ConnectionString;
            }
        }

        /// <summary>
        /// The current owner of the lobby.
        /// </summary>
        /// <remarks>
        /// <c>LobbySearchResult.OwnerEntity</c> may be null if the lobby doesn't currently have an owner.
        /// </remarks>
        public PFEntityKey OwnerEntity
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        /// The maximum number of members that can be present in this lobby.
        /// </summary>
        public uint MaxMemberCount
        {
            get
            {
                return this.searchResult.MaxMemberCount;
            }
        }

        /// <summary>
        /// The current number of members that are present in this lobby.
        /// </summary>
        public uint CurrentMemberCount
        {
            get
            {
                return this.searchResult.CurrentMemberCount;
            }
        }

        /// <summary>
        /// The search properties associated with this lobby.
        /// </summary>
        public IDictionary<string, string> SearchProperties
        {
            get
            {
                return this.searchResult.SearchProperties;
            }
        }

        /// <summary>
        /// The friends in the found lobby, if the lobby search was performed with a
        /// <c>LobbySearchFriendsFilter</c>.
        /// </summary>
        /// <remarks>
        /// If the lobby search which generated this search result was not performed with a
        /// <c>LobbySearchFriendsFilter</c>, this value will always be 0.
        /// <para>
        /// In some multiplayer social networks, friendship is a unidirectional relationship. One user may "follow" another
        /// or be their friend, but the same is not necessarily true in the reverse direction. Friends will only be returned
        /// in this search result when a bidirectional friendship exists. That is, the user querying for the lobby and the
        /// user in the lobby must both be friends with each other.
        /// </para>
        /// </remarks>
        public IList<PFEntityKey> Friends
        {
            get
            {
                return this.friendsList;
            }
        }
    }
}
