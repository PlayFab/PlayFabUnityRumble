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

    /// <summary>
    /// The initial configuration data used when joining an arranged lobby.
    /// </summary>
    public class LobbyArrangedJoinConfiguration
    {
        public LobbyArrangedJoinConfiguration()
        {
            this.Config = new InteropWrapper.PFLobbyArrangedJoinConfiguration();
        }

        /// <summary>
        /// The maximum number of members allowed in the lobby, if joiner is the first member in the lobby.
        /// </summary>
        /// <remarks>
        /// This value must be at least <c>PlayFabMultiplayer.LobbyMaxMemberCountLowerLimit</c> and no more than
        /// <c>PlayFabMultiplayer.LobbyMaxMemberCountUpperLimit</c>.
        /// <para>
        /// If a client would violate this limit by calling <see cref="JoinLobby()" /> or
        /// <see cref="Lobby.AddMember" />, the operation will fail asynchronously and
        /// <see cref="LobbyJoinCompletedStateChange.result" /> or
        /// <see cref="LobbyAddMemberCompletedStateChange.result" />, respectively, will be set to
        /// <see cref="LobbyStateChangeResult.LobbyNotJoinable" />.
        /// </para>
        /// </remarks>
        public uint MaxMemberCount 
        {
            get
            {
                return this.Config.MaxMemberCount;
            }

            set
            {
                this.Config.MaxMemberCount = value;
            }            
        }

        /// <summary>
        /// The owner migration policy for the lobby, if the joiner is the first member in the lobby.
        /// </summary>
        /// <remarks>
        /// This value cannot be set to <c>LobbyOwnerMigrationPolicy.Server</c>.
        /// </remarks>
        public LobbyOwnerMigrationPolicy OwnerMigrationPolicy
        {
            get
            {
                return (LobbyOwnerMigrationPolicy)this.Config.OwnerMigrationPolicy;
            }

            set
            {
                this.Config.OwnerMigrationPolicy = (InteropWrapper.PFLobbyOwnerMigrationPolicy)value;
            }
        }

        /// <summary>
        /// The access policy for the lobby, if the joiner is the first member in the lobby.
        /// </summary>
        public LobbyAccessPolicy AccessPolicy
        {
            get
            {
                return (LobbyAccessPolicy)this.Config.AccessPolicy;
            }

            set
            {
                this.Config.AccessPolicy = (InteropWrapper.PFLobbyAccessPolicy)value;
            }
        }

        /// <summary>
        /// The initial member properties for the joiner of the lobby.
        /// </summary>
        public IDictionary<string, string> MemberProperties
        {
            get
            {
                return this.Config.MemberProperties;
            }

            set
            {
                this.Config.MemberProperties = value;
            }
        }

        internal InteropWrapper.PFLobbyArrangedJoinConfiguration Config { get; set; }
    }
}
