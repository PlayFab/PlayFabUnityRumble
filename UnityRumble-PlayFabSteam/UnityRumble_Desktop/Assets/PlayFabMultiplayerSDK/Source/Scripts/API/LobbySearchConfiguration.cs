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
    /// <summary>
    /// The configuration structure used to specify how a <see cref="FindLobbies" /> operation should be
    /// performed.
    /// </summary>
    public class LobbySearchConfiguration
    {
        public LobbySearchConfiguration()
        {
            this.SearchConfig = new InteropWrapper.PFLobbySearchConfiguration();
        }

        /// <summary>
        /// A filter that, when provided, will constrain the lobby search operation to only those owned by the members of
        /// that player's various friend lists.
        /// </summary>
        /// <remarks>
        /// If omitted, the search operation will search all available lobbies.
        /// </remarks>
        public LobbySearchFriendsFilter FriendsFilter
        {
            get
            {
                return new LobbySearchFriendsFilter(this.SearchConfig.FriendsFilter);
            }
        }

        /// <summary>
        /// The query string used to filter which lobbies are returned in the search results.
        /// </summary>
        /// <remarks>
        /// This string is formatted in an OData-like filtering syntax.
        /// <para>
        /// Only the following operators are supported: "and" (logical and), "eq" (equal), "ne" (not equals), "ge" (greater
        /// than or equal), "gt" (greater than), "le" (less than or equal), and "lt" (less than).
        /// </para>
        /// <para>
        /// The left-hand side of each OData logical expression should be either a search property key (e.g. string_key1,
        /// number_key3, etc) or one of the pre-defined search keys (<see cref="LobbyMemberCountSearchKey" /> or
        /// <see cref="LobbyMemberSearchKey" />).
        /// </para>
        /// <para>
        /// The left-hand side of each OData logical expression should be a search property key.
        /// </para>
        /// <para>
        /// This string cannot exceed 500 characters.
        /// </para>
        /// <para>
        /// Example: "string_key1 eq 'CaptureTheFlag' and number_key10 gt 50 and memberCount lt 5"
        /// </para>
        /// </remarks>
        public string FilterString
        {
            get
            {
                return this.SearchConfig.FilterString;
            }

            set
            {
                this.SearchConfig.FilterString = value;
            }
        }

        /// <summary>
        /// The query string used to sort the lobbies returned in the search results.
        /// </summary>
        /// <remarks>
        /// This string is formatted in an OData-like order-by syntax: a comma-separated list of search property keys with
        /// an optional specifier to sort in either ascending or descending order.
        /// <para>
        /// To specify ascending order, use the "asc" operator after the associated search property key. To specify
        /// descending order, use the "desc" operator after the associated search property key.
        /// </para>
        /// <para>
        /// Additionally, a special sorting moniker, distance, is supported to enable sorting by closest distance from some
        /// numeric value. For example, "distance{number_key10=5} asc" will sort the results so that lobbies who have their
        /// "number_key10" search property closer to the value "5" will return earlier in the search results.
        /// </para>
        /// <para>
        /// This string cannot exceed 100 characters.
        /// </para>
        /// <para>
        /// Example: "string_key1 asc,memberCount desc"
        /// </para>
        /// </remarks>
        public string SortString 
        {
            get
            {
                return this.SearchConfig.SortString;
            }

            set
            {
                this.SearchConfig.SortString = value;
            }
        }

        /// <summary>
        /// An optional value which, when specified, will limit the number of results provided in the completion response.
        /// </summary>
        /// <remarks>
        /// This value may only be specified when <see cref="FindLobbies" /> is called with a client-entity.
        /// <para>
        /// This value can be no higher than <c>PlayFabMultiplayer.LobbyClientRequestedSearchResultCountUpperLimit</c>.
        /// </para>
        /// <para>
        /// When not specified, the limit on the number of search results is service-defined but will be no greater than
        /// <c>PlayFabMultiplayer.LobbyClientRequestedSearchResultCountUpperLimit</c>.
        /// </para>
        /// </remarks>
        public uint? ClientSearchResultCount
        {
            get
            {
                return this.SearchConfig.ClientSearchResultCount;
            }

            set
            {
                this.SearchConfig.ClientSearchResultCount = value;
            }
        }

        internal InteropWrapper.PFLobbySearchConfiguration SearchConfig { get; set; }
    }
}
