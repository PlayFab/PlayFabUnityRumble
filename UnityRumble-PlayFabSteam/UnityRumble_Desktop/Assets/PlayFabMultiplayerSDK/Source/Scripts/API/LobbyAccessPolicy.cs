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

    /// <summary>
    /// The access policy for the lobby 
    /// </summary>
    public enum LobbyAccessPolicy : uint
    {
        /// <summary>
        /// The lobby is both visible in queries and any player may join, including invited players.
        /// </summary>
        Public = InteropWrapper.PFLobbyAccessPolicy.Public,

        /// <summary>
        /// The lobby and its connection string are queryable by friends of members in this lobby.
        /// </summary>
        /// <remarks>
        /// In some multiplayer social networks, friendship is a unidirectional relationship. One user may "follow" another
        /// or be their friend, but the same is not necessarily true in the reverse direction. This access policy only
        /// grants access when a bidirectional friendship exists. That is, the user querying for the lobby and the user in
        /// the lobby must both be friends with each other.
        /// <para>
        /// When querying for lobbies, users can opt into searching external multiplayer social networks for friendship
        /// relationships as well as the native PlayFab friends list. For example, a user could opt to check their Xbox Live
        /// friends list in addition to the PlayFab friends list. An external multiplayer social relationship can only be
        /// considered if both PlayFab users for this title have linked that multiplayer social network to their PlayFab
        /// accounts.
        /// </para>
        /// </remarks>
        Friends = InteropWrapper.PFLobbyAccessPolicy.Friends,

        /// <summary>
        /// The lobby is not visible in queries, and a player must receive an invite to join.
        /// </summary>
        Private = InteropWrapper.PFLobbyAccessPolicy.Private,
    }
}
