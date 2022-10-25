using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFMatchmakingTicketStatusChangedStateChange : PFMatchmakingStateChange")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct PFMatchmakingTicketStatusChangedStateChange
    {
        [FieldOffset(0)]
        public PFMatchmakingStateChange __AnonymousBase_1;

        [FieldOffset(8)]
        [NativeTypeName("PFMatchmakingTicketHandle")]
        public PFMatchmakingTicket* ticket;
    }
}
