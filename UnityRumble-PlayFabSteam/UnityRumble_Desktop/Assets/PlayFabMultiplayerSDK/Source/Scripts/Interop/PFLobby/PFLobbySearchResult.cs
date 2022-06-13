namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbySearchResult
    {
        [NativeTypeName("const char *")]
        public sbyte* lobbyId;

        [NativeTypeName("const char *")]
        public sbyte* connectionString;

        [NativeTypeName("const PFEntityKey *")]
        public PFEntityKey* ownerEntity;

        [NativeTypeName("uint32_t")]
        public uint maxMemberCount;

        [NativeTypeName("uint32_t")]
        public uint currentMemberCount;

        [NativeTypeName("uint32_t")]
        public uint searchPropertyCount;

        [NativeTypeName("const char *const *")]
        public sbyte** searchPropertyKeys;

        [NativeTypeName("const char *const *")]
        public sbyte** searchPropertyValues;

        [NativeTypeName("uint32_t")]
        public uint friendCount;

        [NativeTypeName("const PFEntityKey *")]
        public PFEntityKey* friends;
    }
}
