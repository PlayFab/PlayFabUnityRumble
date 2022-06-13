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
    /// The filter structure used to limit lobby search results to only those lobbies owned by the player's friends.
    /// </summary>
    /// <remarks>
    /// Regardless of which external friend lists are included when constructing this filter, friends from the PlayFab
    /// friends list will always be included.
    /// </remarks>
    public class LobbySearchFriendsFilter
    {
        private InteropWrapper.PFLobbySearchFriendsFilter filter;

        internal LobbySearchFriendsFilter(InteropWrapper.PFLobbySearchFriendsFilter filter)
        {
            this.filter = filter;
        }

        /// <summary>
        /// A flag which includes the player's Steam friends list if their PlayFab account is linked to their Steam account.
        /// </summary>
        public bool IncludeSteamFriends 
        {
            get
            {
                return this.filter.IncludeSteamFriends;
            }

            set
            {
                this.filter.IncludeSteamFriends = value;
            }
        }

        /// <summary>
        /// A flag which includes the player's Facebook friends list if their PlayFab account is linked to their Facebook
        /// account.
        /// </summary>
        public bool IncludeFacebookFriends
        {
            get
            {
                return this.filter.IncludeFacebookFriends;
            }

            set
            {
                this.filter.IncludeFacebookFriends = value;
            }
        }

        /// <summary>
        /// An Xbox Live token that, when provided, includes the player's Xbox Live friends list if their PlayFab account is
        /// linked to their Xbox Live account.
        /// </summary>
        /// <remarks>
        /// To retrieve this token, make a POST request to "https://playfabapi.com" with an empty request body using one of
        /// the GetTokenAndSignature APIs provided by Xbox Live.
        /// <para>
        /// GetTokenAndSignature APIs are provided natively as part of the Microsoft Game Core Development Kit (GDK). On all
        /// other platforms, these APIs are provided via the Xbox Authentication Library API (XAL).
        /// </para>
        /// </remarks>
        public string IncludeXboxFriendsToken
        {
            get
            {
                return this.filter.IncludeXboxFriendsToken;
            }

            set
            {
                this.filter.IncludeXboxFriendsToken = value;
            }
        }
    }
}
