namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbySearchFriendsFilter
    {
        public bool includeSteamFriends;

        public bool includeFacebookFriends;

        [NativeTypeName("const char *")]
        public sbyte* includeXboxFriendsToken;
    }
}
