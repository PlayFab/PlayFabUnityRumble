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
    /// A request to make an update to the shared portion of the lobby on behalf of a member.
    /// </summary>
    public class LobbyDataUpdate
    {
        public LobbyDataUpdate()
        {
            this.Update = new InteropWrapper.PFLobbyDataUpdate();
        }

        /// <summary>
        /// An optional, new owner of the lobby.
        /// </summary>
        /// <remarks>
        /// This value can only be updated under one of the following conditions:
        /// ` * The member updating this field is the lobby's current owner
        /// ` * The owner migration policy is <c>LobbyOwnerMigrationPolicy.Manual</c> and there is currently no owner
        /// ` * The owner migration policy is <c>LobbyOwnerMigrationPolicy.None</c>
        /// </remarks>
        public PFEntityKey NewOwner
        {
            get
            {
                return new PFEntityKey(this.Update.NewOwner);
            }

            set
            {
                this.Update.NewOwner = value.EntityKey;
            }
        }

        /// <summary>
        /// An optional, updated capacity for the number of members in this lobby.
        /// </summary>
        /// <remarks>
        /// This new value must be greater than than the number of members currently in the lobby and less than
        /// <c>LobbyMaxMemberCountUpperLimit</c>.
        /// <para>
        /// This value can only be updated by the current lobby owner.
        /// </para>
        /// </remarks>
        public uint? MaxMemberCount
        {
            get
            {
                return this.Update.MaxMemberCount;
            }

            set
            {
                this.Update.MaxMemberCount = value;
            }
        }

        /// <summary>
        /// An optional, updated access policy for this lobby.
        /// </summary>
        /// <remarks>
        /// This value can only be updated by the current lobby owner.
        /// </remarks>
        public LobbyAccessPolicy? AccessPolicy
        {
            get
            {
                return this.Update.AccessPolicy;
            }

            set
            {
                this.Update.AccessPolicy = value;
            }
        }

        /// <summary>
        /// An optional update to the membership lock on this lobby.
        /// </summary>
        /// <remarks>
        /// This value can only be updated by the current lobby owner.
        /// </remarks>
        public LobbyMembershipLock? MembershipLock
        {
            get
            {
                return (LobbyMembershipLock)this.Update.MembershipLock;
            }

            set
            {
                this.Update.MembershipLock = (LobbyMembershipLock)value;
            }
        }

        /// <summary>
        /// The search properties to update.
        /// </summary>
        /// <remarks>
        /// Only the current lobby owner can update the search properties.
        /// <para>
        /// There may only be <c>PlayFabMultiplayer.LobbyMaxSearchPropertyCount</c> concurrent search properties at any given time.
        /// Therefore, at most, twice that many unique properties can be specified in this update if half of those
        /// properties are being deleted.
        /// </para>
        /// <para>
        /// If the property limits are violated, the entire update operation will fail.
        /// </para>
        /// </remarks>
        public IDictionary<string, string> SearchProperties
        {
            get
            {
                return this.Update.SearchProperties;
            }

            set
            {
                this.Update.SearchProperties = value;
            }
        }

        /// <summary>
        /// The lobby properties to update.
        /// </summary>
        /// <remarks>
        /// Only the current lobby owner can update the lobby properties.
        /// <para>
        /// There may only be <c>PlayFabMultiplayer.LobbyMaxLobbyPropertyCount</c> concurrent lobby properties at any given time. Therefore,
        /// at most, twice that many unique properties can be specified in this update if half of those properties are being
        /// deleted.
        /// </para>
        /// <para>
        /// If the property limits are violated, the entire update operation will fail.
        /// </para>
        /// </remarks>
        public IDictionary<string, string> LobbyProperties
        {
            get
            {
                return this.Update.LobbyProperties;
            }

            set
            {
                this.Update.LobbyProperties = value;
            }
        }

        internal InteropWrapper.PFLobbyDataUpdate Update { get; set; }
    }
}
