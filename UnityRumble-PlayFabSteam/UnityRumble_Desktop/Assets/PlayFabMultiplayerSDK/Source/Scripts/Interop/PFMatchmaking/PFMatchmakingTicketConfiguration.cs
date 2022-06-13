namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFMatchmakingTicketConfiguration
    {
        [NativeTypeName("uint32_t")]
        public uint timeoutInSeconds;

        [NativeTypeName("const char *")]
        public sbyte* queueName;

        [NativeTypeName("uint32_t")]
        public uint membersToMatchWithCount;

        [NativeTypeName("const PFEntityKey *")]
        public PFEntityKey* membersToMatchWith;
    }
}
