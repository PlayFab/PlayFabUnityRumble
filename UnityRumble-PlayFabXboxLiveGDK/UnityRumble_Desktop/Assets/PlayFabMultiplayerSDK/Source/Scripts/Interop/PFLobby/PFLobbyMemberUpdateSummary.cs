namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFLobbyMemberUpdateSummary
    {
        public PFEntityKey member;

        public bool connectionStatusUpdated;

        [NativeTypeName("uint32_t")]
        public uint updatedMemberPropertyCount;

        [NativeTypeName("const char *const *")]
        public sbyte** updatedMemberPropertyKeys;
    }
}
