namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyInviteReceivedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyInviteReceivedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public PFEntityKey listeningEntity;

        public PFEntityKey invitingEntity;

        [NativeTypeName("const char *")]
        public sbyte* connectionString;
    }
}
