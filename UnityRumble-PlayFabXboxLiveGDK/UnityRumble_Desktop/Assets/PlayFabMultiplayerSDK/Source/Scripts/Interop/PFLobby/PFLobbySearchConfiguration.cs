namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbySearchConfiguration
    {
        [NativeTypeName("const PFLobbySearchFriendsFilter *")]
        public PFLobbySearchFriendsFilter* friendsFilter;

        [NativeTypeName("const char *")]
        public sbyte* filterString;

        [NativeTypeName("const char *")]
        public sbyte* sortString;

        [NativeTypeName("const uint32_t *")]
        public uint* clientSearchResultCount;
    }
}
