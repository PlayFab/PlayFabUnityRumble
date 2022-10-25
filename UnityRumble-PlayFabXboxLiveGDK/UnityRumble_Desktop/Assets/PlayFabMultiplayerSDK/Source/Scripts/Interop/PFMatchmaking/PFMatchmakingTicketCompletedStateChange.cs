using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    [NativeTypeName("struct PFMatchmakingTicketCompletedStateChange : PFMatchmakingStateChange")]
    [StructLayout(LayoutKind.Explicit)]
    public unsafe partial struct PFMatchmakingTicketCompletedStateChange
    {
        [FieldOffset(0)]
        public PFMatchmakingStateChange __AnonymousBase_1;

        [FieldOffset(4)]
        public int result;

        [FieldOffset(8)]
        [NativeTypeName("PFMatchmakingTicketHandle")]
        public PFMatchmakingTicket* ticket;

        [FieldOffset(16)]
        public void* asyncContext;
    }
}
