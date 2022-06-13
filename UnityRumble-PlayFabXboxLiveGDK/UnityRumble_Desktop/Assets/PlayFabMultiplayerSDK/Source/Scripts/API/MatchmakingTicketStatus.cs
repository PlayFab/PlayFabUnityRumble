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
    /// The possible states for a matchmaking ticket.
    /// </summary>
    public enum MatchmakingTicketStatus : uint
    {
        /// <summary>
        /// The matchmaking ticket is being created.
        /// </summary>
        Creating = InteropWrapper.PFMatchmakingTicketStatus.Creating,

        /// <summary>
        /// The matchmaking ticket is being joined.
        /// </summary>
        Joining = InteropWrapper.PFMatchmakingTicketStatus.Joining,

        /// <summary>
        /// The matchmaking ticket is waiting for all remote users specified in the <c>membersToMatchWith</c> field of its
        /// configuration to join the ticket via <see cref="PlayFabMultiplayer.JoinMatchmakingTicket()" />.
        /// </summary>
        WaitingForPlayers = InteropWrapper.PFMatchmakingTicketStatus.WaitingForPlayers,

        /// <summary>
        /// The matchmaking ticket is waiting for a match to be found.
        /// </summary>
        WaitingForMatch = InteropWrapper.PFMatchmakingTicketStatus.WaitingForMatch,

        /// <summary>
        /// The matchmaking ticket has found a match.
        /// </summary>
        Matched = InteropWrapper.PFMatchmakingTicketStatus.Matched,

        /// <summary>
        /// The matchmaking ticket has been canceled.
        /// </summary>
        Canceled = InteropWrapper.PFMatchmakingTicketStatus.Canceled,

        /// <summary>
        /// The matchmaking ticket failed to find a match.
        /// </summary>
        Failed = InteropWrapper.PFMatchmakingTicketStatus.Failed
    }
}
