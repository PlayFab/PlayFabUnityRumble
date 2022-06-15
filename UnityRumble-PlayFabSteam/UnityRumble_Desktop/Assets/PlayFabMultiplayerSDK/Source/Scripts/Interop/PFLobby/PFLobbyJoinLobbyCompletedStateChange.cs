namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyJoinLobbyCompletedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyJoinLobbyCompletedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public int result;

        public PFEntityKey newMember;

        public void* asyncContext;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;
    }
}
