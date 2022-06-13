using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFLobbyUpdatedStateChange : PFLobbyStateChange")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct PFLobbyUpdatedStateChange
    {
        [FieldOffset(0)]
        public PFLobbyStateChange __AnonymousBase_1;

        [NativeTypeName("PFLobbyHandle")]
        [FieldOffset(8)]
        public PFLobby* lobby;

        [FieldOffset(16)]
        public bool ownerUpdated;

        [FieldOffset(17)]
        public bool maxMembersUpdated;

        [FieldOffset(18)]
        public bool accessPolicyUpdated;

        [FieldOffset(19)]
        public bool membershipLockUpdated;

        [NativeTypeName("uint32_t")]
        [FieldOffset(20)]
        public uint updatedSearchPropertyCount;

        [NativeTypeName("const char *const *")]
        [FieldOffset(24)]
        public sbyte** updatedSearchPropertyKeys;

        [NativeTypeName("uint32_t")]
        [FieldOffset(32)]
        public uint updatedLobbyPropertyCount;

        [NativeTypeName("const char *const *")]
        [FieldOffset(40)]
        public sbyte** updatedLobbyPropertyKeys;

        [NativeTypeName("uint32_t")]
        [FieldOffset(48)]
        public uint memberUpdateCount;

        [NativeTypeName("const PFLobbyMemberUpdateSummary *")]
        [FieldOffset(56)]
        public PFLobbyMemberUpdateSummary* memberUpdates;
    }
}
