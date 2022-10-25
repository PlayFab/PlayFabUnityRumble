namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFMultiplayerPort
    {
        [NativeTypeName("const char *")]
        public sbyte* name;

        [NativeTypeName("uint32_t")]
        public uint num;

        public PFMultiplayerProtocolType protocol;
    }
}
