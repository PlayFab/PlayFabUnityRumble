namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFLobbyDisconnectingReason : uint
    {
        NoLocalMembers = 0,
        LobbyDeleted = 1,
        ConnectionInterruption = 2,
    }
}
