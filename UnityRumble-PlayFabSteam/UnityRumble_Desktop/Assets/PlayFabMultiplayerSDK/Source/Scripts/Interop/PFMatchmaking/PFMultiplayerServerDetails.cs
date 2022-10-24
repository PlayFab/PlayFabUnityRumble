namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFMultiplayerServerDetails
    {
        [NativeTypeName("const char *")]
        public sbyte* fqdn;

        [NativeTypeName("const char *")]
        public sbyte* ipv4Address;

        [NativeTypeName("const PFMultiplayerPort *")]
        public PFMultiplayerPort* ports;

        [NativeTypeName("uint32_t")]
        public uint portCount;

        [NativeTypeName("const char *")]
        public sbyte* region;
    }
}
