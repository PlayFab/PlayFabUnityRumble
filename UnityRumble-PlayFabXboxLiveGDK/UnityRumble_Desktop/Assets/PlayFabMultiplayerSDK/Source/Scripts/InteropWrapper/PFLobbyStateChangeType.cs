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

    public enum PFLobbyStateChangeType : uint
    {
        CreateAndJoinLobbyCompleted = Interop.PFLobbyStateChangeType.CreateAndJoinLobbyCompleted,
        JoinLobbyCompleted = Interop.PFLobbyStateChangeType.JoinLobbyCompleted,
        MemberAdded = Interop.PFLobbyStateChangeType.MemberAdded,
        AddMemberCompleted = Interop.PFLobbyStateChangeType.AddMemberCompleted,
        MemberRemoved = Interop.PFLobbyStateChangeType.MemberRemoved,
        ForceRemoveMemberCompleted = Interop.PFLobbyStateChangeType.ForceRemoveMemberCompleted,
        LeaveLobbyCompleted = Interop.PFLobbyStateChangeType.LeaveLobbyCompleted,
        Updated = Interop.PFLobbyStateChangeType.Updated,
        PostUpdateCompleted = Interop.PFLobbyStateChangeType.PostUpdateCompleted,
        Disconnecting = Interop.PFLobbyStateChangeType.Disconnecting,
        Disconnected = Interop.PFLobbyStateChangeType.Disconnected,
        JoinArrangedLobbyCompleted = Interop.PFLobbyStateChangeType.JoinArrangedLobbyCompleted,
        FindLobbiesCompleted = Interop.PFLobbyStateChangeType.FindLobbiesCompleted,
        InviteReceived = Interop.PFLobbyStateChangeType.InviteReceived,
        InviteListenerStatusChanged = Interop.PFLobbyStateChangeType.InviteListenerStatusChanged,
        SendInviteCompleted = Interop.PFLobbyStateChangeType.SendInviteCompleted
    }
}
