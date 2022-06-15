namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyInviteListenerStatusChangedStateChange : PFLobbyStateChange")]
    public partial struct PFLobbyInviteListenerStatusChangedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public PFEntityKey listeningEntity;
    }
}
