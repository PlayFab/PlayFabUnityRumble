using System;
using PartyXBLCSharpSDK.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_STATE_CHANGE
    {
        protected PARTY_XBL_STATE_CHANGE(PARTY_XBL_STATE_CHANGE_TYPE StateChangeType, IntPtr StateChangeId)
        {
            this.StateChangeType = StateChangeType;
            this.StateChangeId = StateChangeId;
        }

        internal static PARTY_XBL_STATE_CHANGE CreateFromPtr(IntPtr stateChangePtr)
        {
            PARTY_XBL_STATE_CHANGE result = null;
            PARTY_XBL_STATE_CHANGE_UNION stateChangeUnion = (PARTY_XBL_STATE_CHANGE_UNION)Marshal.PtrToStructure(stateChangePtr, typeof(PARTY_XBL_STATE_CHANGE_UNION));
            switch (stateChangeUnion.stateChange.stateChangeType)
            {
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_TOKEN_AND_SIGNATURE_REQUESTED:
                    result = new PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_LOCAL_CHAT_USER_DESTROYED:
                    result = new PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_CREATE_LOCAL_CHAT_USER_COMPLETED:
                    result = new PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_LOGIN_TO_PLAYFAB_COMPLETED:
                    result = new PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED:
                    result = new PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_XBL_STATE_CHANGE_TYPE.PARTY_XBL_STATE_CHANGE_TYPE_REQUIRED_CHAT_PERMISSION_INFO_CHANGED:
                    result = new PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                default:
                    Debug.WriteLine(String.Format("Unhandle type {0}\n", stateChangeUnion.stateChange.stateChangeType));
                    Debugger.Break();
                    result = new PARTY_XBL_STATE_CHANGE(stateChangeUnion.stateChange.stateChangeType, stateChangePtr);
                    break;
            }

            return result;
        }

        public PARTY_XBL_STATE_CHANGE_TYPE StateChangeType { get; }
        internal IntPtr StateChangeId { get; }
    }

    public class PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_TOKEN_AND_SIGNATURE_REQUESTED_STATE_CHANGE stateChangeConverted = stateChange.tokenAndSignatureRequested;
            this.correlationId = stateChangeConverted.correlationId;
            this.method = Converters.PtrToStringUTF8(stateChangeConverted.method);
            this.url = Converters.PtrToStringUTF8(stateChangeConverted.url);
            this.headers = Converters.PtrToClassArray<PARTY_XBL_HTTP_HEADER, Interop.PARTY_XBL_HTTP_HEADER>(
                stateChangeConverted.headers,
                stateChangeConverted.headerCount,
                (x) => new PARTY_XBL_HTTP_HEADER(x));
            this.body = new Byte[stateChangeConverted.bodySize];
            if (stateChangeConverted.bodySize > 0)
            {
                Marshal.Copy(stateChangeConverted.body, this.body, 0, (int)stateChangeConverted.bodySize);
            }
            this.forceRefresh = Convert.ToBoolean(stateChangeConverted.forceRefresh);
            this.allUsers = Convert.ToBoolean(stateChangeConverted.allUsers);
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
        }

        public UInt32 correlationId { get; }
        public string method { get; }
        public string url { get; }
        public PARTY_XBL_HTTP_HEADER[] headers { get; }
        public Byte[] body { get; }
        public bool forceRefresh { get; }
        public bool allUsers { get; }
        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
    }

    public class PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_CREATE_LOCAL_CHAT_USER_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.createLocalChatUserCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
        }

        public PARTY_XBL_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public Object asyncIdentifier { get; }
        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
    }

    public class PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.localChatUserDestroyed;
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
        public PARTY_XBL_LOCAL_CHAT_USER_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_LOGIN_TO_PLAYFAB_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.loginToPlayfabCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.entityId = Converters.PtrToStringUTF8(stateChangeConverted.entityId);
            this.titlePlayerEntityToken = Converters.PtrToStringUTF8(stateChangeConverted.titlePlayerEntityToken);
            this.expirationTime = stateChangeConverted.expirationTime;
        }

        public PARTY_XBL_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
        public Object asyncIdentifier { get; }
        public string entityId { get; }
        public string titlePlayerEntityToken { get; }
        public Int64 expirationTime { get; }
    }

    public class PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_GET_ENTITY_IDS_FROM_XBOX_LIVE_USER_IDS_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.getEntityIdsFromXboxLiveUserIdsCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.xboxLiveSandbox = Converters.PtrToStringUTF8(stateChangeConverted.xboxLiveSandbox);
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.entityIdMappings = Converters.PtrToClassArray<PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING, Interop.PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING>(
                stateChangeConverted.entityIdMappings,
                stateChangeConverted.entityIdMappingCount,
                (x) => new PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING(x));
        }

        public PARTY_XBL_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public string xboxLiveSandbox { get; }
        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
        public Object asyncIdentifier { get; }
        public PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING[] entityIdMappings { get; }
    }

    public class PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE : PARTY_XBL_STATE_CHANGE
    {
        internal PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE(
            PARTY_XBL_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_XBL_REQUIRED_CHAT_PERMISSION_INFO_CHANGED_STATE_CHANGE stateChangeConverted = stateChange.requiredChatPermissionInfoChanged;
            this.localChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.localChatUser);
            this.targetChatUser = new PARTY_XBL_CHAT_USER_HANDLE(stateChangeConverted.targetChatUser);
        }

        public PARTY_XBL_CHAT_USER_HANDLE localChatUser { get; }
        public PARTY_XBL_CHAT_USER_HANDLE targetChatUser { get; }
    }
}