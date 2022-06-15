namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFMatchmakingTicketStatus : uint
    {
        Creating = 0,
        Joining = 1,
        WaitingForPlayers = 2,
        WaitingForMatch = 3,
        Matched = 4,
        Canceled = 5,
        Failed = 6,
    }
}
