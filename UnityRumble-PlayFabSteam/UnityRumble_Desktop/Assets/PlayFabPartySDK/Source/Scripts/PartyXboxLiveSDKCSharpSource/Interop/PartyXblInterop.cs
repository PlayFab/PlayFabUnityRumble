using System;
using System.Runtime.InteropServices;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK.Interop
{
    partial class PartyXblInterop
    {
#if (UNITY_GAMECORE || MICROSOFT_GAME_CORE) && !UNITY_EDITOR
        const string ThunkDllName = "PartyXboxLive";
#else
        const string ThunkDllName = "PartyXboxLiveWin32";
#endif

        //PartyXblStartProcessingStateChanges(
        //    PARTY_XBL_HANDLE handle,
        //    _Out_ uint32_t* stateChangeCount,
        //    _Outptr_result_buffer_(*stateChangeCount) const PARTY_XBL_STATE_CHANGE*const** stateChanges
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyXblStartProcessingStateChanges(
            PARTY_XBL_HANDLE handle,
            out UInt32 stateChangeCount,
            out IntPtr stateChanges);

        //PartyXblDestroyChatUser(
        //    PARTY_XBL_HANDLE handle,
        //    PARTY_XBL_CHAT_USER_HANDLE chatUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblDestroyChatUser(
            PARTY_XBL_HANDLE handle,
            PARTY_XBL_CHAT_USER_HANDLE chatUser);

        //PartyXblInitialize(
        //    PARTY_HANDLE partyHandle,
        //    PartyString titleId,
        //    _Outptr_ PARTY_XBL_HANDLE* handle
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblInitialize(
            IntPtr partyHandle,
            Byte[] titleId,
            out PARTY_XBL_HANDLE handle);

        //PartyXblCompleteGetTokenAndSignatureRequest(
        //    PARTY_XBL_HANDLE handle,
        //    uint32_t correlationId,
        //    PartyBool succeeded,
        //   _In_opt_ PartyString token,
        //   _In_opt_ PartyString signature
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblCompleteGetTokenAndSignatureRequest(
            PARTY_XBL_HANDLE handle,
            UInt32 correlationId,
            Byte succeeded,
            Byte[] token,
            Byte[] signature);

        //PartyXblLocalChatUserGetCrossNetworkCommunicationPrivacySetting(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    PARTY_XBL_CROSS_NETWORK_COMMUNICATION_PRIVACY_SETTING* setting
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyXblLocalChatUserGetCrossNetworkCommunicationPrivacySetting(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out PARTY_XBL_CROSS_NETWORK_COMMUNICATION_PRIVACY_SETTING setting);

        //PartyXblLocalChatUserGetAccessibilitySettings(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    _Out_ PARTY_XBL_ACCESSIBILITY_SETTINGS* settings
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblLocalChatUserGetAccessibilitySettings(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out PARTY_XBL_ACCESSIBILITY_SETTINGS settings);

        //PartyXblCleanup(
        //    _Post_invalid_ PARTY_XBL_HANDLE handle
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblCleanup(
            PARTY_XBL_HANDLE handle);

        //PartyXblGetChatUsers(
        //    PARTY_XBL_HANDLE handle,
        //    _Out_ uint32_t* chatUserCount,
        //    _Outptr_result_buffer_(*chatUserCount) const PARTY_XBL_CHAT_USER_HANDLE** chatUsers
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyXblGetChatUsers(
            PARTY_XBL_HANDLE handle,
            out UInt32 chatUserCount,
            out IntPtr chatUsers);

        //PartyXblChatUserGetXboxUserId(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    _Out_ uint64_t* xboxUserId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblChatUserGetXboxUserId(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out UInt64 xboxUserId);

        //PartyXblLoginToPlayFab(
        //    PARTY_XBL_CHAT_USER_HANDLE localChatUser,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblLoginToPlayFab(
            PARTY_XBL_CHAT_USER_HANDLE localChatUser,
            IntPtr asyncIdentifier);

        //PartyXblCreateRemoteChatUser(
        //    PARTY_XBL_HANDLE handle,
        //    uint64_t xboxUserId,
        //    _Outptr_opt_ PARTY_XBL_CHAT_USER_HANDLE* chatUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblCreateRemoteChatUser(
            PARTY_XBL_HANDLE handle,
            UInt64 xboxUserId,
            out PARTY_XBL_CHAT_USER_HANDLE chatUser);

        //PartyXblSetThreadAffinityMask(
        //    PARTY_XBL_THREAD_ID threadId,
        //    uint64_t threadAffinityMask
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblSetThreadAffinityMask(
            PARTY_XBL_THREAD_ID threadId,
            UInt64 threadAffinityMask);

        //PartyXblGetThreadAffinityMask(
        //    PARTY_XBL_THREAD_ID threadId,
        //    _Out_ uint64_t* threadAffinityMask
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblGetThreadAffinityMask(
            PARTY_XBL_THREAD_ID threadId,
            out UInt64 threadAffinityMask);

        //PartyXblChatUserIsLocal(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    _Out_ PartyBool* isLocal
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblChatUserIsLocal(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out Byte isLocal);

        //PartyXblSetMemoryCallbacks(
        //    _In_ PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //    _In_ PARTY_MEM_FREE_FUNC freeMemoryCallback
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartyXblSetMemoryCallbacks(
        //    PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //    PARTY_MEM_FREE_FUNC freeMemoryCallback);

        //PartyXblFinishProcessingStateChanges(
        //    PARTY_XBL_HANDLE handle,
        //    uint32_t stateChangeCount,
        //    _In_reads_(stateChangeCount) const PARTY_XBL_STATE_CHANGE*const* stateChanges
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyXblFinishProcessingStateChanges(
            PARTY_XBL_HANDLE handle,
            UInt32 stateChangeCount,
            IntPtr stateChanges);

        //PartyXblGetMemoryCallbacks(
        //    _Outptr_ PARTY_MEM_ALLOC_FUNC* allocateMemoryCallback,
        //    _Outptr_ PARTY_MEM_FREE_FUNC* freeMemoryCallback
        //    );
        // [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        // internal static extern UInt32 PartyXblGetMemoryCallbacks(
        //     out PARTY_MEM_ALLOC_FUNC allocateMemoryCallback,
        //     out PARTY_MEM_FREE_FUNC freeMemoryCallback);

        //PartyXblChatUserSetCustomContext(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblChatUserSetCustomContext(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            IntPtr customContext);

        //PartyXblChatUserGetCustomContext(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblChatUserGetCustomContext(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out IntPtr customContext);

        //PartyXblGetErrorMessage(
        //    PartyError error,
        //    _Outptr_ PartyString* errorMessage
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblGetErrorMessage(
            UInt32 error,
            out UTF8StringPtr errorMessage);

        //PartyXblCreateLocalChatUser(
        //    PARTY_XBL_HANDLE handle,
        //    uint64_t xboxUserId,
        //    _In_opt_ void* asyncIdentifier,
        //    _Outptr_opt_ PARTY_XBL_CHAT_USER_HANDLE* localXboxLiveUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblCreateLocalChatUser(
            PARTY_XBL_HANDLE handle,
            UInt64 xboxUserId,
            IntPtr asyncIdentifier,
            out PARTY_XBL_CHAT_USER_HANDLE localXboxLiveUser);

        //PartyXblLocalChatUserGetRequiredChatPermissionInfo(
        //    PARTY_XBL_CHAT_USER_HANDLE handle,
        //    const PARTY_XBL_CHAT_USER_HANDLE targetChaUser,
        //    _Out_ PARTY_XBL_CHAT_PERMISSION_INFO* chatPermissionInfo
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblLocalChatUserGetRequiredChatPermissionInfo(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            PARTY_XBL_CHAT_USER_HANDLE targetChaUser,
            out PARTY_XBL_CHAT_PERMISSION_INFO chatPermissionInfo);

        //PartyXblGetEntityIdsFromXboxLiveUserIds(
        //    PARTY_XBL_HANDLE handle,
        //    uint32_t xboxLiveUserIdCount,
        //    _In_reads_(xboxLiveUserIdCount) const uint64_t* xboxLiveUserIds,
        //    PARTY_XBL_CHAT_USER_HANDLE localChatUser,
        //    _In_opt_ void * asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyXblGetEntityIdsFromXboxLiveUserIds(
            PARTY_XBL_HANDLE handle,
            UInt32 xboxLiveUserIdCount,
            UInt64[] xboxLiveUserIds,
            PARTY_XBL_CHAT_USER_HANDLE localChatUser,
            IntPtr asyncIdentifier);
    }
}
