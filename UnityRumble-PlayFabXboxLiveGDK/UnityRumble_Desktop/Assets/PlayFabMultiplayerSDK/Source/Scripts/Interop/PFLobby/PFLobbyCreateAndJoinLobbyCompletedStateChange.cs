namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyCreateAndJoinLobbyCompletedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyCreateAndJoinLobbyCompletedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public int result;

        public void* asyncContext;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;
    }
}
