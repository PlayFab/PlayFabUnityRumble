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

    public class PFMatchmakingMatchDetails
    {
        internal unsafe PFMatchmakingMatchDetails(Interop.PFMatchmakingMatchDetails* interopStruct)
        {
            this.MatchId = Converters.PtrToStringUTF8((IntPtr)interopStruct->matchId);
            this.Members = new PFMatchmakingMatchMember[interopStruct->memberCount];
            for (int i = 0; i < interopStruct->memberCount; i++)
            {
                this.Members[i] = new PFMatchmakingMatchMember();
                this.Members[i].EntityKey = new PFEntityKey(
                    Converters.PtrToStringUTF8((IntPtr)interopStruct->members[i].entityKey.id),
                    Converters.PtrToStringUTF8((IntPtr)interopStruct->members[i].entityKey.type));
                this.Members[i].TeamId = Converters.PtrToStringUTF8((IntPtr)interopStruct->members[i].teamId);
                this.Members[i].Attributes = Converters.PtrToStringUTF8((IntPtr)interopStruct->members[i].attributes);
            }

            this.RegionPreferences = Converters.StringPtrToArray(interopStruct->regionPreferences, interopStruct->regionPreferenceCount);
            this.LobbyArrangementString = Converters.PtrToStringUTF8((IntPtr)interopStruct->lobbyArrangementString);
            if (interopStruct->serverDetails != null)
            {
                this.ServerDetails = new PFMultiplayerServerDetails(interopStruct->serverDetails);
            }
        }

        public string MatchId { get; set; }

        public PFMatchmakingMatchMember[] Members { get; set; }

        public string[] RegionPreferences { get; set; }

        public string LobbyArrangementString { get; set; }

        public PFMultiplayerServerDetails ServerDetails { get; }
    }
}
