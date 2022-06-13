namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbyJoinConfiguration
    {
        [NativeTypeName("uint32_t")]
        public uint memberPropertyCount;

        [NativeTypeName("const char *const *")]
        public sbyte** memberPropertyKeys;

        [NativeTypeName("const char *const *")]
        public sbyte** memberPropertyValues;
    }
}
