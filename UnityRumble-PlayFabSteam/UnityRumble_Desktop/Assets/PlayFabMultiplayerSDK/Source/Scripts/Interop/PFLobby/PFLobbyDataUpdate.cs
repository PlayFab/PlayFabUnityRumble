namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbyDataUpdate
    {
        [NativeTypeName("const PFEntityKey *")]
        public PFEntityKey* newOwner;

        [NativeTypeName("const uint32_t *")]
        public uint* maxMemberCount;

        [NativeTypeName("const PFLobbyAccessPolicy *")]
        public PFLobbyAccessPolicy* accessPolicy;

        [NativeTypeName("const PFLobbyMembershipLock *")]
        public PFLobbyMembershipLock* membershipLock;

        [NativeTypeName("uint32_t")]
        public uint searchPropertyCount;

        [NativeTypeName("const char *const *")]
        public sbyte** searchPropertyKeys;

        [NativeTypeName("const char *const *")]
        public sbyte** searchPropertyValues;

        [NativeTypeName("uint32_t")]
        public uint lobbyPropertyCount;

        [NativeTypeName("const char *const *")]
        public sbyte** lobbyPropertyKeys;

        [NativeTypeName("const char *const *")]
        public sbyte** lobbyPropertyValues;
    }
}
