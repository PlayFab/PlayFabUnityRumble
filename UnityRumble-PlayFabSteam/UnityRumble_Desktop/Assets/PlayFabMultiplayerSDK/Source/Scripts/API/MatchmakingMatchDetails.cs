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

    public class MatchmakingMatchDetails
    {
        private InteropWrapper.PFMatchmakingMatchDetails details;

        private IList<MatchmakingTicketMatchMember> members;

        private IList<string> regionPreferences;

        internal MatchmakingMatchDetails(InteropWrapper.PFMatchmakingMatchDetails details)
        {
            this.details = details;
            this.members = this.details.Members.Select(x => new MatchmakingTicketMatchMember(x)).ToList();
            this.regionPreferences = details.RegionPreferences.ToList();
        }

        /// <summary>
        /// The ID of the match.
        /// </summary>
        public string MatchId
        {
            get
            {
                return this.details.MatchId;
            }
        }

        /// <summary>
        /// The members that have been matched together.
        /// </summary>
        public IList<MatchmakingTicketMatchMember> Members
        {
            get
            {
                return this.members;
            }
        }

        /// <summary>
        /// Preferred regions for the match, sorted from most to least preferred.
        /// </summary>
        public IList<string> RegionPreferences
        {
            get
            {
                return this.regionPreferences;
            }            
        }

        /// <summary>
        /// The lobby arrangement string associated with the match.
        /// </summary>
        /// <remarks>
        /// This connection string can optionally be used with <see cref="PlayFabMultiplayer.JoinArrangedLobby" /> to join a
        /// lobby associated with this match result. The lobby is not created until a user attempts to join it.
        /// </remarks>
        public string LobbyArrangementString
        {
            get
            {
                return this.details.LobbyArrangementString;
            }
        }
    }
}
