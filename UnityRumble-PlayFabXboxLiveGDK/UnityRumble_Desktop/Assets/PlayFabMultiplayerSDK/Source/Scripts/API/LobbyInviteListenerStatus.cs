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
    /// Values representing the current status of an invite listener.
    /// </summary>
    public enum LobbyInviteListenerStatus : uint
    {
        /// <summary>
        /// The invite listener has not been started or has been stopped and is not listening for invites.
        /// </summary>
        NotListening = InteropWrapper.PFLobbyInviteListenerStatus.NotListening,

        /// <summary>
        /// The invite listener has been established and is listening for invites.
        /// </summary>
        Listening = InteropWrapper.PFLobbyInviteListenerStatus.Listening,

        /// <summary>
        /// The listening entity was not authorized to establish an invite listener.
        /// </summary>
        /// <remarks>
        /// This status is fatal. The title should clear the invite listener with
        /// <see cref="PlayFabMultiplayer.StopListeningForLobbyInvites" />. No invites will be received on the corresponding
        /// listener after this status update.
        /// <para>
        /// Receiving this status likely represents a programming error where an invalid entity has been passed to
        /// <c>PlayFabMultiplayer.StopListeningForLobbyInvites</c>.
        /// </para>
        /// </remarks>
        NotAuthorized = InteropWrapper.PFLobbyInviteListenerStatus.NotAuthorized,
    }
}
