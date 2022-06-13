using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    public static unsafe partial class Methods
    {
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerStartProcessingMatchmakingStateChanges([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t *")] uint* stateChangeCount, [NativeTypeName("const PFMatchmakingStateChange *const **")] PFMatchmakingStateChange*** stateChanges);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerFinishProcessingMatchmakingStateChanges([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t")] uint stateChangeCount, [NativeTypeName("const PFMatchmakingStateChange *const *")] PFMatchmakingStateChange** stateChanges);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerCreateMatchmakingTicket([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t")] uint localUserCount, [NativeTypeName("const PFEntityKey *")] PFEntityKey* localUsers, [NativeTypeName("const char *const *")] sbyte** localUserAttributes, [NativeTypeName("const PFMatchmakingTicketConfiguration *")] PFMatchmakingTicketConfiguration* configuration, void* asyncContext, [NativeTypeName("PFMatchmakingTicketHandle *")] PFMatchmakingTicket** ticket);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerJoinMatchmakingTicketFromId([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t")] uint localUserCount, [NativeTypeName("const PFEntityKey *")] PFEntityKey* localUsers, [NativeTypeName("const char *const *")] sbyte** localUserAttributes, [NativeTypeName("const char *")] sbyte* ticketId, [NativeTypeName("const char *")] sbyte* queueName, void* asyncContext, [NativeTypeName("PFMatchmakingTicketHandle *")] PFMatchmakingTicket** ticket);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerDestroyMatchmakingTicket([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("PFMatchmakingTicketHandle")] PFMatchmakingTicket* ticket);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMatchmakingTicketGetStatus([NativeTypeName("PFMatchmakingTicketHandle")] PFMatchmakingTicket* ticket, PFMatchmakingTicketStatus* status);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMatchmakingTicketCancel([NativeTypeName("PFMatchmakingTicketHandle")] PFMatchmakingTicket* ticket);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMatchmakingTicketGetTicketId([NativeTypeName("PFMatchmakingTicketHandle")] PFMatchmakingTicket* ticket, [NativeTypeName("const char **")] sbyte** id);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMatchmakingTicketGetMatch([NativeTypeName("PFMatchmakingTicketHandle")] PFMatchmakingTicket* ticket, [NativeTypeName("const PFMatchmakingMatchDetails **")] PFMatchmakingMatchDetails** match);
    }
}
