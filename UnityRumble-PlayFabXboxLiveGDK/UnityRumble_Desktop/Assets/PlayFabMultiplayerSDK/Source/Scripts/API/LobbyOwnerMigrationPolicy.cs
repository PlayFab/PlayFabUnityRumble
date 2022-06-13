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
    /// The available policies the lobby service can use to migrate lobby ownership between members.
    /// </summary>
    public enum LobbyOwnerMigrationPolicy : uint
    {
        /// <summary>
        /// Once the lobby owner is disconnected, a new owner is chosen at random from the set of connected members.
        /// </summary>
        Automatic = InteropWrapper.PFLobbyOwnerMigrationPolicy.Automatic,

        /// <summary>
        /// Once the lobby owner is disconnected, any member may elect themselves the new owner.
        /// </summary>
        /// <remarks>
        /// Until a new owner is chosen, <see cref="Lobby.GetOwner" /> will return a null owner.
        /// </remarks>
        Manual = InteropWrapper.PFLobbyOwnerMigrationPolicy.Manual,

        /// <summary>
        /// At any point, any member may elect themselves the owner of the lobby, regardless of the state of the current
        /// owner.
        /// </summary>
        /// <remarks>
        /// If the current owner leaves, <see cref="Lobby.GetOwner" /> will return a null owner until a new owner elects
        /// themselves.
        /// </remarks>
        None = InteropWrapper.PFLobbyOwnerMigrationPolicy.None,

        /// <summary>
        /// The server is the owner and owner migration is not possible.
        /// </summary>
        Server = InteropWrapper.PFLobbyOwnerMigrationPolicy.Server,
    }
}
