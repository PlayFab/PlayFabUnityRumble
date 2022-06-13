namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyMemberRemovedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyMemberRemovedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;

        public PFEntityKey member;

        public PFLobbyMemberRemovedReason reason;
    }
}
