namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyFindLobbiesCompletedStateChange : PFLobbyStateChange")]
    public unsafe partial struct PFLobbyFindLobbiesCompletedStateChange
    {
        public PFLobbyStateChange __AnonymousBase_1;

        public int result;

        public PFEntityKey searchingEntity;

        public void* asyncContext;

        [NativeTypeName("uint32_t")]
        public uint searchResultCount;

        [NativeTypeName("const PFLobbySearchResult *")]
        public PFLobbySearchResult* searchResults;
    }
}
