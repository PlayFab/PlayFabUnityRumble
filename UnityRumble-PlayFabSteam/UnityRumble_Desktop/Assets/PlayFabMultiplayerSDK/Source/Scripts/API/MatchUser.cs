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

    public struct MatchUser
    {
#if UNITY_2017_1_OR_NEWER
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchUser" /> struct.
        /// </summary>
        /// <param name="localUser">
        /// The local user to join to the ticket.
        /// </param>
        /// <param name="localUserJsonAttributesJSON">
        /// The array of local user attribute strings. There should be one attribute string for each local user. Each attribute
        /// string should either be an empty string or a serialized JSON object. For example,
        /// <c>{"player_color":"blue","player_role":"tank"}</c>.
        /// </param>
        public MatchUser(PlayFab.PlayFabAuthenticationContext localUser, string localUserJsonAttributesJSON)
        {
            PlayFabMultiplayer.SetEntityToken(localUser);
            this.LocalUser = new PFEntityKey(localUser);
            this.LocalUserJsonAttributesJSON = localUserJsonAttributesJSON;
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchUser" /> struct.
        /// </summary>
        /// <param name="localUser">
        /// The local user to join to the ticket.
        /// </param>
        /// <param name="localUserJsonAttributesJSON">
        /// The array of local user attribute strings. There should be one attribute string for each local user. Each attribute
        /// string should either be an empty string or a serialized JSON object. For example,
        /// <c>{"player_color":"blue","player_role":"tank"}</c>.
        /// </param>
        public MatchUser(PFEntityKey localUser, string localUserJsonAttributesJSON)
        {
            this.LocalUser = localUser;
            this.LocalUserJsonAttributesJSON = localUserJsonAttributesJSON;
        }

        /// <summary>
        /// The local user to join to the ticket.
        /// </summary>
        public PFEntityKey LocalUser { get; set; }

        /// <summary>
        /// The local user attributes as JSON string. There should be one attribute string for each local user. Each attribute
        /// string should either be an empty string or a serialized JSON object. For example,
        /// <c>{"player_color":"blue","player_role":"tank"}</c>.
        /// </summary>
        public string LocalUserJsonAttributesJSON { get; set; }
    }
}
