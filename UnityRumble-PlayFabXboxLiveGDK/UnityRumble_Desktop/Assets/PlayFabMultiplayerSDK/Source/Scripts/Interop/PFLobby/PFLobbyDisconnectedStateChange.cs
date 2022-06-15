namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyDisconnectedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyDisconnectedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;
    }
}
