namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFMatchmakingMatchDetails
    {
        [NativeTypeName("const char *")]
        public sbyte* matchId;

        [NativeTypeName("const PFMatchmakingMatchMember *")]
        public PFMatchmakingMatchMember* members;

        [NativeTypeName("uint32_t")]
        public uint memberCount;

        [NativeTypeName("const char *const *")]
        public sbyte** regionPreferences;

        [NativeTypeName("uint32_t")]
        public uint regionPreferenceCount;

        [NativeTypeName("const char *")]
        public sbyte* lobbyArrangementString;

        [NativeTypeName("const PFMultiplayerServerDetails *")]
        public PFMultiplayerServerDetails* serverDetails;
    }
}
