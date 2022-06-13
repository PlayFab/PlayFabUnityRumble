namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyDisconnectingStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyDisconnectingStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        [NativeTypeName("PFLobbyHandle")]
        public PFLobby* lobby;

        public PFLobbyDisconnectingReason reason;
    }
}
