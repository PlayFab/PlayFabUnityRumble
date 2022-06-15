namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyPostUpdateCompletedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyPostUpdateCompletedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public int result;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;

        public PFEntityKey localUser;

        public void* asyncContext;
    }
}
