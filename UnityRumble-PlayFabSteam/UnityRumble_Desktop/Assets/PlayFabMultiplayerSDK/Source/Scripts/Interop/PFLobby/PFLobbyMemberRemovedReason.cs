namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFLobbyMemberRemovedReason : uint
    {
        LocalUserLeftLobby = 0,
        LocalUserForciblyRemoved = 1,
        RemoteUserLeftLobby = 2,
    }
}
