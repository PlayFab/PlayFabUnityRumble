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
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// A collection of hints about an update which has been successfully applied to the lobby on behalf of a member.
    /// </summary>
    public class LobbyMemberUpdateSummary
    {
        private InteropWrapper.PFLobbyMemberUpdateSummary summary;

        private List<string> updatedMemberPropertyKeyList;

        private PFEntityKey member;

        internal unsafe LobbyMemberUpdateSummary(InteropWrapper.PFLobbyMemberUpdateSummary summary)
        {
            this.summary = summary;
            this.updatedMemberPropertyKeyList = new List<string>(summary.UpdatedMemberPropertyKeys);
            this.member = new PFEntityKey(summary.Member);
        }

        /// <summary>
        /// The member which performed the update
        /// </summary>
        public PFEntityKey Member 
        {
            get
            {
                return this.member;
            }
        }

        /// <summary>
        /// A flag indicating whether the member's connection status has updated.
        /// </summary>
        public bool ConnectionStatusUpdated
        {
            get
            {
                return this.summary.ConnectionStatusUpdated;
            }
        }

        /// <summary>
        /// The member properties which have been updated for <c>member</c>.
        /// </summary>
        public List<string> UpdatedMemberPropertyKeys
        {
            get
            {
                return this.updatedMemberPropertyKeyList;
            }
        }
    }
}
