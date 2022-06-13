namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFMatchmakingMatchMember
    {
        public PFEntityKey entityKey;

        [NativeTypeName("const char *")]
        public sbyte* teamId;

        [NativeTypeName("const char *")]
        public sbyte* attributes;
    }
}
