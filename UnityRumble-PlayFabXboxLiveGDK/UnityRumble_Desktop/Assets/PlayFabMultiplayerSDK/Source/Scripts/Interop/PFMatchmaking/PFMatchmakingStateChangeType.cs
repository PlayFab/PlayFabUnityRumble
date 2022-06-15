namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFMatchmakingStateChangeType : uint
    {
        TicketStatusChanged = 0,
        TicketCompleted = 1,
    }
}
