using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    partial class PFPInterop
    {
#if (UNITY_GAMECORE || MICROSOFT_GAME_CORE) && !UNITY_EDITOR
        const string ThunkDllName = "PartyWin";
#elif UNITY_SWITCH && !UNITY_EDITOR
        const string ThunkDllName = "__Internal";
#elif UNITY_IOS && !UNITY_EDITOR
        const string ThunkDllName = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
        const string ThunkDllName = "party";
#elif UNITY_PS4 && !UNITY_EDITOR
        const string ThunkDllName = "Party.prx";
#elif UNITY_PS5 && !UNITY_EDITOR
        const string ThunkDllName = "Party.prx";
#else
        const string ThunkDllName = "PartyWin32";
#endif

        //PartyInitialize(
        //    PartyString titleId,
        //    _Outptr_ PARTY_HANDLE* handle
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyInitialize(
            Byte[] titleId,
            out PARTY_HANDLE handle);

        //PartyCleanup(
        //    _Post_invalid_ PARTY_HANDLE handle
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyCleanup(
            PARTY_HANDLE handle);

        //PartyCreateLocalUser(
        //    PARTY_HANDLE handle,
        //    PartyString entityId,
        //    PartyString titlePlayerEntityToken,
        //    _Outptr_ PARTY_LOCAL_USER_HANDLE* localUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyCreateLocalUser(
            PARTY_HANDLE handle,
            Byte[] entityId,
            Byte[] titlePlayerEntityToken,
            out PARTY_LOCAL_USER_HANDLE localUser);

        //PartyCreateNewNetwork(
        //    PARTY_HANDLE handle,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    const PARTY_NETWORK_CONFIGURATION* networkConfiguration,
        //    uint32_t regionCount,
        //    _In_reads_(regionCount) const PARTY_REGION* regions,
        //    _In_opt_ const PARTY_INVITATION_CONFIGURATION* initialInvitationConfiguration,
        //    _In_opt_ void * asyncIdentifier,
        //    _Out_opt_ PARTY_NETWORK_DESCRIPTOR* networkDescriptor,
        //    _Out_writes_opt_z_(PARTY_MAX_INVITATION_IDENTIFIER_STRING_LENGTH + 1) char * appliedInitialInvitationIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal unsafe static extern UInt32 PartyCreateNewNetwork(
            PARTY_HANDLE handle,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_NETWORK_CONFIGURATION* networkConfiguration,
            UInt32 regionCount,
            IntPtr regions,
            PARTY_INVITATION_CONFIGURATION* initialInvitationConfiguration,
            IntPtr asyncIdentifier,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor,
            IntPtr appliedInitialInvitationIdentifier);

        //PartyGetNetworks(
        //    PARTY_HANDLE handle,
        //    _Out_ uint32_t* networkCount,
        //    _Outptr_result_buffer_(*networkCount) const PARTY_NETWORK_HANDLE** networks
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyGetNetworks(
            PARTY_HANDLE handle,
            out UInt32 networkCount,
            out IntPtr networks);

        //PartyInvitationGetCreatorEntityId(
        //    PARTY_INVITATION_HANDLE invitation,
        //    _Outptr_result_maybenull_ PartyString * entityId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyInvitationGetCreatorEntityId(
            PARTY_INVITATION_HANDLE invitation,
            out UTF8StringPtr entityId);

        //PartySynchronizeMessagesBetweenEndpoints(
        //    PARTY_HANDLE handle,
        //    uint32_t endpointCount,
        //    _In_reads_(endpointCount) const PARTY_ENDPOINT_HANDLE* endpoints,
        //    PARTY_SYNCHRONIZE_MESSAGES_BETWEEN_ENDPOINTS_OPTIONS options,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartySynchronizeMessagesBetweenEndpoints(
            PARTY_HANDLE handle,
            UInt32 endpointCount,
            [In] PARTY_ENDPOINT_HANDLE[] endpoints,
            PARTY_SYNCHRONIZE_MESSAGES_BETWEEN_ENDPOINTS_OPTIONS options,
            IntPtr asyncIdentifier);

        //PartyInvitationSetCustomContext(
        //    PARTY_INVITATION_HANDLE invitation,
        //    _In_opt_ void * customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyInvitationSetCustomContext(
            PARTY_INVITATION_HANDLE invitation,
            IntPtr customContext);

        //PartyGetChatControls(
        //    PARTY_HANDLE handle,
        //    _Out_ uint32_t* chatControlCount,
        //    _Outptr_result_buffer_(*chatControlCount) const PARTY_CHAT_CONTROL_HANDLE** chatControls
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyGetChatControls(
            PARTY_HANDLE handle,
            out UInt32 chatControlCount,
            out IntPtr chatControls);

        //PartyInvitationGetInvitationConfiguration(
        //    PARTY_INVITATION_HANDLE invitation,
        //    _Outptr_ const PARTY_INVITATION_CONFIGURATION ** configuration
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyInvitationGetInvitationConfiguration(
            PARTY_INVITATION_HANDLE invitation,
            out IntPtr configuration);

        //PartyInvitationGetCustomContext(
        //    PARTY_INVITATION_HANDLE invitation,
        //    _Outptr_result_maybenull_ void ** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyInvitationGetCustomContext(
            PARTY_INVITATION_HANDLE invitation,
            out IntPtr customContext);

        //PartyFinishProcessingStateChanges(
        //    PARTY_HANDLE handle,
        //    uint32_t stateChangeCount,
        //    _In_reads_(stateChangeCount) const PARTY_STATE_CHANGE*const* stateChanges
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyFinishProcessingStateChanges(
            PARTY_HANDLE handle,
            UInt32 stateChangeCount,
            IntPtr stateChanges);

        //PartySetOption(
        //    _In_opt_ void* object,
        //    PARTY_OPTION option,
        //    _In_opt_ const void* value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartySetOption(
            IntPtr Object,
            PARTY_OPTION option,
            IntPtr value);

        //PartyLocalUserGetEntityId(
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _Outptr_ PartyString* entityId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyLocalUserGetEntityId(
            PARTY_LOCAL_USER_HANDLE localUser,
            out UTF8StringPtr entityId);

        //PartySetMemoryCallbacks(
        //    _In_ PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //    _In_ PARTY_MEM_FREE_FUNC freeMemoryCallback
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartySetMemoryCallbacks(
        //    PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //    PARTY_MEM_FREE_FUNC freeMemoryCallback);

        //PartyGetMemoryCallbacks(
        //    _Out_ PARTY_MEM_ALLOC_FUNC* allocateMemoryCallback,
        //    _Out_ PARTY_MEM_FREE_FUNC* freeMemoryCallback
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartyGetMemoryCallbacks(
        //    out PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //    out PARTY_MEM_FREE_FUNC freeMemoryCallback);

        //PartyConnectToNetwork(
        //    PARTY_HANDLE handle,
        //    const PARTY_NETWORK_DESCRIPTOR* networkDescriptor,
        //    _In_opt_ void* asyncIdentifier,
        //    _Outptr_opt_ PARTY_NETWORK_HANDLE* network
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyConnectToNetwork(
            PARTY_HANDLE handle,
            PARTY_NETWORK_DESCRIPTOR* networkDescriptor,
            IntPtr asyncIdentifier,
            out PARTY_NETWORK_HANDLE network);

        //PartyGetRegions(
        //    PARTY_HANDLE handle,
        //    _Out_ uint32_t* regionListCount,
        //    _Outptr_result_buffer_(*regionListCount) const PARTY_REGION** regionList
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyGetRegions(
            PARTY_HANDLE handle,
            out UInt32 regionListCount,
            out IntPtr regionList);

        //PartyLocalUserUpdateEntityToken(
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    PartyString titlePlayerEntityToken
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyLocalUserUpdateEntityToken(
            PARTY_LOCAL_USER_HANDLE localUser,
            Byte[] titlePlayerEntityToken);

        //PartyDestroyLocalUser(
        //    PARTY_HANDLE handle,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDestroyLocalUser(
            PARTY_HANDLE handle,
            PARTY_LOCAL_USER_HANDLE localUser,
            IntPtr asyncIdentifier);

        //PartySetThreadAffinityMask(
        //    PARTY_THREAD_ID threadId,
        //    uint64_t threadAffinityMask
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartySetThreadAffinityMask(
            PARTY_THREAD_ID threadId,
            UInt64 threadAffinityMask);

        //PartySetWorkMode(
        //    PARTY_THREAD_ID threadId,
        //    PARTY_WORK_MODE workMode
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartySetWorkMode(
            PARTY_THREAD_ID threadId,
            PARTY_WORK_MODE workMode);

        //PartyDoWork(
        //    PARTY_HANDLE handle,
        //    PARTY_THREAD_ID threadId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDoWork(
            PARTY_HANDLE handle,
            PARTY_THREAD_ID threadId);

        //PartyGetOption(
        //    _In_opt_ const void* object,
        //    PARTY_OPTION option,
        //    _Out_ void* value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyGetOption(
            IntPtr Object,
            PARTY_OPTION option,
            IntPtr value);

        //PartyLocalUserGetCustomContext(
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyLocalUserGetCustomContext(
            PARTY_LOCAL_USER_HANDLE localUser,
            out IntPtr customContext);

        //PartyGetErrorMessage(
        //    PartyError error,
        //    _Outptr_ PartyString* errorMessage
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyGetErrorMessage(
            UInt32 error,
            out UTF8StringPtr errorMessage);

        //PartyGetThreadAffinityMask(
        //    PARTY_THREAD_ID threadId,
        //    _Out_ uint64_t* threadAffinityMask
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyGetThreadAffinityMask(
            PARTY_THREAD_ID threadId,
            out UInt64 threadAffinityMask);

        //PartyGetWorkMode(
        //    PARTY_THREAD_ID threadId,
        //    _Out_ PARTY_WORK_MODE * workMode
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyGetWorkMode(
            PARTY_THREAD_ID threadId,
            out PARTY_WORK_MODE workMode);

        //PartyStartProcessingStateChanges(
        //    PARTY_HANDLE handle,
        //    _Out_ uint32_t* stateChangeCount,
        //    _Outptr_result_buffer_(*stateChangeCount) const PARTY_STATE_CHANGE*const** stateChanges
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyStartProcessingStateChanges(
            PARTY_HANDLE handle,
            out UInt32 stateChangeCount,
            out IntPtr stateChanges);

        //PartyGetLocalDevice(
        //    PARTY_HANDLE handle,
        //    _Outptr_ PARTY_DEVICE_HANDLE* localDevice
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyGetLocalDevice(
            PARTY_HANDLE handle,
            out PARTY_DEVICE_HANDLE localDevice);

        //PartyLocalUserSetCustomContext(
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyLocalUserSetCustomContext(
            PARTY_LOCAL_USER_HANDLE localUser,
            IntPtr customContext);

        //PartyGetLocalUsers(
        //    PARTY_HANDLE handle,
        //    _Out_ uint32_t* userCount,
        //    _Outptr_result_buffer_(*userCount) const PARTY_LOCAL_USER_HANDLE** users
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyGetLocalUsers(
            PARTY_HANDLE handle,
            out UInt32 userCount,
            out IntPtr users);
    }
}
