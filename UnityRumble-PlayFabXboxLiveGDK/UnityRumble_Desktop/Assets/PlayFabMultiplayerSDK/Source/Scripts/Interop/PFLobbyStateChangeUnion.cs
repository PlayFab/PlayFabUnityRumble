namespace PlayFab.Multiplayer.Interop
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Explicit)]
    internal struct PFLobbyStateChangeUnion
    {
        [FieldOffset(0)]
        internal readonly PFLobbyStateChange stateChange;
        [FieldOffset(0)]
        internal readonly PFLobbyCreateAndJoinLobbyCompletedStateChange createAndJoinCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyDisconnectingStateChange disconnecting;
        [FieldOffset(0)]
        internal readonly PFLobbyDisconnectedStateChange disconnected;
        [FieldOffset(0)]
        internal readonly PFLobbyMemberAddedStateChange memberAdded;
        [FieldOffset(0)]
        internal readonly PFLobbyMemberRemovedStateChange memberRemoved;
        [FieldOffset(0)]
        internal readonly PFLobbyAddMemberCompletedStateChange addMemberCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyForceRemoveMemberCompletedStateChange forceRemoveMember;
        [FieldOffset(0)]
        internal readonly PFLobbyJoinLobbyCompletedStateChange joinCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyUpdatedStateChange lobbyUpdated;
        [FieldOffset(0)]
        internal readonly PFLobbyLeaveLobbyCompletedStateChange leaveCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyJoinArrangedLobbyCompletedStateChange arrangedJoinCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyFindLobbiesCompletedStateChange findLobbiesCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbySendInviteCompletedStateChange sendInviteCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyPostUpdateCompletedStateChange postUpdateCompleted;
        [FieldOffset(0)]
        internal readonly PFLobbyInviteListenerStatusChangedStateChange inviteListenerStatusChanged;
        [FieldOffset(0)]
        internal readonly PFLobbyInviteReceivedStateChange inviteReceivedStateChange;
    }
}
