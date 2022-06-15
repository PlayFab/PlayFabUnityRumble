namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyForceRemoveMemberCompletedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyForceRemoveMemberCompletedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public int result;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;

        public PFEntityKey targetMember;

        public void* asyncContext;
    }
}
