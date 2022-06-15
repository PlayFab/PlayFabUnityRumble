namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFMatchmakingTicketCompletedStateChange : PFMatchmakingStateChange")]
    public unsafe partial struct PFMatchmakingTicketCompletedStateChange
    {
        public PFMatchmakingStateChange __AnonymousBase_1;

        public int result;

        [NativeTypeName("PFMatchmakingTicketHandle")]
        public PFMatchmakingTicket* ticket;

        public void* asyncContext;
    }
}
