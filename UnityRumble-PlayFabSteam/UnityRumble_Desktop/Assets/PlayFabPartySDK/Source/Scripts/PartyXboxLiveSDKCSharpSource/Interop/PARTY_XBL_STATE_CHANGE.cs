using System;
using System.Runtime.InteropServices;

namespace PartyXBLCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE_TYPE stateChangeType;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly PARTY_XBL_STATE_CHANGE_RESULT result;
        internal readonly UInt32 errorDetail;
        internal readonly IntPtr asyncIdentifier;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly PARTY_XBL_STATE_CHANGE_RESULT result;
        internal readonly UInt32 errorDetail;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
        internal readonly IntPtr asyncIdentifier;
        internal readonly IntPtr entityId;
        internal readonly IntPtr titlePlayerEntityToken;
        internal readonly Int64 expirationTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly PARTY_XBL_STATE_CHANGE_RESULT result;
        internal readonly UInt32 errorDetail;
        internal readonly IntPtr xboxLiveSandbox;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
        internal readonly IntPtr asyncIdentifier;
        internal readonly UInt32 entityIdMappingCount;
        internal readonly IntPtr entityIdMappings;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
        internal readonly PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_REASON reason;
        internal readonly UInt32 errorDetail;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE targetChatUser;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE
    {
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        internal readonly UInt32 correlationId;
        internal readonly IntPtr method;
        internal readonly IntPtr url;
        internal readonly UInt32 headerCount;
        internal readonly IntPtr headers;
        internal readonly UInt32 bodySize;
        internal readonly IntPtr body;
        internal readonly Byte forceRefresh;
        internal readonly Byte allUsers;
        internal readonly PARTY_XBL_CHAT_USER_HANDLE localChatUser;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct PARTY_XBL_STATE_CHANGE_UNION
    {
        [FieldOffset(0)]
        internal readonly PARTY_XBL_STATE_CHANGE stateChange;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE createLocalChatUserCompleted;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE loginToPlayfabCompleted;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE getEntityIdsFromXboxLiveUserIdsCompleted;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE localChatUserDestroyed;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE requiredChatPermissionInfoChanged;
        [FieldOffset(0)]
        internal readonly PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE tokenAndSignatureRequested;
    }
}