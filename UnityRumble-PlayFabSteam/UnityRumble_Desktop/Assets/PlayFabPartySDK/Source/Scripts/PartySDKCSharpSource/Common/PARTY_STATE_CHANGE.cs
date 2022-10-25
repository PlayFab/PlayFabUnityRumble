using System;
using PartyCSharpSDK.Interop;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;

namespace PartyCSharpSDK
{
    public class PARTY_STATE_CHANGE
    {
        protected PARTY_STATE_CHANGE(PARTY_STATE_CHANGE_TYPE StateChangeType, IntPtr StateChangeId)
        {
            this.StateChangeType = StateChangeType;
            this.StateChangeId = StateChangeId;
            this.useObjectPool = false;
        }

        internal static PARTY_STATE_CHANGE CreateFromPtr(IntPtr stateChangePtr)
        {
            PARTY_STATE_CHANGE result = null;
            PARTY_STATE_CHANGE_UNION stateChangeUnion = (PARTY_STATE_CHANGE_UNION)Marshal.PtrToStructure(stateChangePtr, typeof(PARTY_STATE_CHANGE_UNION));
            switch (stateChangeUnion.stateChange.stateChangeType)
            {
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REGIONS_CHANGED:
                    result = new PARTY_REGIONS_CHANGED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DESTROY_LOCAL_USER_COMPLETED:
                    result = new PARTY_DESTROY_LOCAL_USER_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CREATE_NEW_NETWORK_COMPLETED:
                    result = new PARTY_CREATE_NEW_NETWORK_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CONNECT_TO_NETWORK_COMPLETED:
                    result = new PARTY_CONNECT_TO_NETWORK_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_AUTHENTICATE_LOCAL_USER_COMPLETED:
                    result = new PARTY_AUTHENTICATE_LOCAL_USER_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_NETWORK_CONFIGURATION_MADE_AVAILABLE:
                    result = new PARTY_NETWORK_CONFIGURATION_MADE_AVAILABLE_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_NETWORK_DESCRIPTOR_CHANGED:
                    result = new PARTY_NETWORK_DESCRIPTOR_CHANGED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_LOCAL_USER_REMOVED:
                    result = new PARTY_LOCAL_USER_REMOVED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REMOVE_LOCAL_USER_COMPLETED:
                    result = new PARTY_REMOVE_LOCAL_USER_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_LOCAL_USER_KICKED:
                    result = new PARTY_LOCAL_USER_KICKED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CREATE_ENDPOINT_COMPLETED:
                    result = new PARTY_CREATE_ENDPOINT_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DESTROY_ENDPOINT_COMPLETED:
                    result = new PARTY_DESTROY_ENDPOINT_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_ENDPOINT_CREATED:
                    result = new PARTY_ENDPOINT_CREATED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_ENDPOINT_DESTROYED:
                    result = new PARTY_ENDPOINT_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REMOTE_DEVICE_CREATED:
                    result = new PARTY_REMOTE_DEVICE_CREATED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REMOTE_DEVICE_DESTROYED:
                    result = new PARTY_REMOTE_DEVICE_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REMOTE_DEVICE_JOINED_NETWORK:
                    result = new PARTY_REMOTE_DEVICE_JOINED_NETWORK_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REMOTE_DEVICE_LEFT_NETWORK:
                    result = new PARTY_REMOTE_DEVICE_LEFT_NETWORK_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_LEAVE_NETWORK_COMPLETED:
                    result = new PARTY_LEAVE_NETWORK_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_NETWORK_DESTROYED:
                    result = new PARTY_NETWORK_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_ENDPOINT_MESSAGE_RECEIVED:
                    result = SDK.objectPool.Retrieve<PARTY_ENDPOINT_MESSAGE_RECEIVED_STATE_CHANGE>(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CREATE_INVITATION_COMPLETED:
                    result = new PARTY_CREATE_INVITATION_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_REVOKE_INVITATION_COMPLETED:
                    result = new PARTY_REVOKE_INVITATION_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_INVITATION_CREATED:
                    result = new PARTY_INVITATION_CREATED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_INVITATION_DESTROYED:
                    result = new PARTY_INVITATION_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_KICK_DEVICE_COMPLETED:
                    result = new PARTY_KICK_DEVICE_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_KICK_USER_COMPLETED:
                    result = new PARTY_KICK_USER_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CREATE_CHAT_CONTROL_COMPLETED:
                    result = new PARTY_CREATE_CHAT_CONTROL_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DESTROY_CHAT_CONTROL_COMPLETED:
                    result = new PARTY_DESTROY_CHAT_CONTROL_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_CONTROL_CREATED:
                    result = new PARTY_CHAT_CONTROL_CREATED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_CONTROL_DESTROYED:
                    result = new PARTY_CHAT_CONTROL_DESTROYED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_CONTROL_JOINED_NETWORK:
                    result = new PARTY_CHAT_CONTROL_JOINED_NETWORK_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_CONTROL_LEFT_NETWORK:
                    result = new PARTY_CHAT_CONTROL_LEFT_NETWORK_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CONNECT_CHAT_CONTROL_COMPLETED:
                    result = new PARTY_CONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DISCONNECT_CHAT_CONTROL_COMPLETED:
                    result = new PARTY_DISCONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_CHAT_AUDIO_INPUT_COMPLETED:
                    result = new PARTY_SET_CHAT_AUDIO_INPUT_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_CHAT_AUDIO_OUTPUT_COMPLETED:
                    result = new PARTY_SET_CHAT_AUDIO_OUTPUT_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_LOCAL_CHAT_AUDIO_INPUT_CHANGED:
                    result = new PARTY_LOCAL_CHAT_AUDIO_INPUT_CHANGED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_LOCAL_CHAT_AUDIO_OUTPUT_CHANGED:
                    result = new PARTY_LOCAL_CHAT_AUDIO_OUTPUT_CHANGED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_POPULATE_AVAILABLE_TEXT_TO_SPEECH_PROFILES_COMPLETED:
                    result = new PARTY_POPULATE_AVAILABLE_TEXT_TO_SPEECH_PROFILES_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_LANGUAGE_COMPLETED:
                    result = new PARTY_SET_LANGUAGE_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_TEXT_RECEIVED:
                    result = new PARTY_CHAT_TEXT_RECEIVED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_CHAT_AUDIO_ENCODER_BITRATE_COMPLETED:
                    result = new PARTY_SET_CHAT_AUDIO_ENCODER_BITRATE_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_TEXT_CHAT_OPTIONS_COMPLETED:
                    result = new PARTY_SET_TEXT_CHAT_OPTIONS_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_TEXT_TO_SPEECH_PROFILE_COMPLETED:
                    result = new PARTY_SET_TEXT_TO_SPEECH_PROFILE_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SET_TRANSCRIPTION_OPTIONS_COMPLETED:
                    result = new PARTY_SET_TRANSCRIPTION_OPTIONS_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_VOICE_CHAT_TRANSCRIPTION_RECEIVED:
                    result = SDK.objectPool.Retrieve<PARTY_VOICE_CHAT_TRANSCRIPTION_RECEIVED_STATE_CHANGE>(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SYNTHESIZE_TEXT_TO_SPEECH_COMPLETED:
                    result = new PARTY_SYNTHESIZE_TEXT_TO_SPEECH_COMPLETED_STATE_CHANGE(stateChangeUnion, stateChangePtr);
                    break;
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CONFIGURE_AUDIO_MANIPULATION_VOICE_STREAM_COMPLETED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CONFIGURE_AUDIO_MANIPULATION_CAPTURE_STREAM_COMPLETED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CONFIGURE_AUDIO_MANIPULATION_RENDER_STREAM_COMPLETED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_CHAT_CONTROL_PROPERTIES_CHANGED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_NETWORK_PROPERTIES_CHANGED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_ENDPOINT_PROPERTIES_CHANGED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_SYNCHRONIZE_MESSAGES_BETWEEN_ENDPOINTS_COMPLETED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DEVICE_PROPERTIES_CHANGED:
                case PARTY_STATE_CHANGE_TYPE.PARTY_STATE_CHANGE_TYPE_DATA_BUFFERS_RETURNED:
                default:
                    Debug.Write(String.Format("Unhandle type {0}\n", stateChangeUnion.stateChange.stateChangeType));
                    Debugger.Break();
                    result = new PARTY_STATE_CHANGE(stateChangeUnion.stateChange.stateChangeType, stateChangePtr);
                    break;
            }

            return result;
        }

        internal virtual void Cleanup()
        {
            if (useObjectPool)
            {
                SDK.objectPool.Return(this);
            }
        }

        public PARTY_STATE_CHANGE_TYPE StateChangeType { get; }
        internal IntPtr StateChangeId { get; }
        protected bool useObjectPool;
    }

    public class PARTY_CREATE_NEW_NETWORK_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CREATE_NEW_NETWORK_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CREATE_NEW_NETWORK_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.createNewNetworkCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.networkConfiguration = new PARTY_NETWORK_CONFIGURATION(stateChangeConverted.networkConfiguration);
            this.regionCount = stateChangeConverted.regionCount;
            this.regions = Converters.PtrToClassArray<PARTY_REGION, Interop.PARTY_REGION>(
                stateChangeConverted.regions,
                regionCount,
                (x) => new PARTY_REGION(x));

            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }

            this.networkDescriptor = new PARTY_NETWORK_DESCRIPTOR(stateChangeConverted.networkDescriptor);
            this.appliedInitialInvitationIdentifier = Converters.PtrToStringUTF8(stateChangeConverted.appliedInitialInvitationIdentifier);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public PARTY_NETWORK_CONFIGURATION networkConfiguration { get; }
        public UInt32 regionCount { get; }
        public PARTY_REGION[] regions { get; }
        public Object asyncIdentifier { get; }
        public PARTY_NETWORK_DESCRIPTOR networkDescriptor { get; }
        public string appliedInitialInvitationIdentifier { get; }
    }

    public class PARTY_DESTROY_LOCAL_USER_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_DESTROY_LOCAL_USER_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_DESTROY_LOCAL_USER_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.destroyLocalUserCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_LOCAL_USER_REMOVED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_LOCAL_USER_REMOVED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_LOCAL_USER_REMOVED_STATE_CHANGE stateChangeConverted = stateChange.localUserRemoved;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.removedReason = stateChangeConverted.removedReason;
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public PARTY_LOCAL_USER_REMOVED_REASON removedReason { get; }
    }

    public class PARTY_CONNECT_TO_NETWORK_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CONNECT_TO_NETWORK_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CONNECT_TO_NETWORK_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.connectToNetworkCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.networkDescriptor = new PARTY_NETWORK_DESCRIPTOR(stateChangeConverted.networkDescriptor);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_DESCRIPTOR networkDescriptor { get; }
        public Object asyncIdentifier { get; }
        public PARTY_NETWORK_HANDLE network { get; }
    }

    public class PARTY_NETWORK_DESTROYED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_NETWORK_DESTROYED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_NETWORK_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.networkDestroyed;
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
        }

        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
    }

    public class PARTY_AUTHENTICATE_LOCAL_USER_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_AUTHENTICATE_LOCAL_USER_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_AUTHENTICATE_LOCAL_USER_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.authenticateLocalUserCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.invitationIdentifier = Converters.PtrToStringUTF8(stateChangeConverted.invitationIdentifier);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public string invitationIdentifier { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_REGIONS_CHANGED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REGIONS_CHANGED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REGIONS_CHANGED_STATE_CHANGE stateChangeConverted = stateChange.regionsChanged;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_NETWORK_CONFIGURATION_MADE_AVAILABLE_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_NETWORK_CONFIGURATION_MADE_AVAILABLE_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_NETWORK_CONFIGURATION_MADE_AVAILABLE_STATE_CHANGE stateChangeConverted = stateChange.networkConfigurationMadeAvailable;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.networkConfiguration = Converters.PtrToClass<PARTY_NETWORK_CONFIGURATION, Interop.PARTY_NETWORK_CONFIGURATION>(
                stateChangeConverted.networkConfiguration,
                (x) => new PARTY_NETWORK_CONFIGURATION(x));
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_NETWORK_CONFIGURATION networkConfiguration { get; }
    }

    public class PARTY_INVITATION_CREATED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_INVITATION_CREATED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_INVITATION_CREATED_STATE_CHANGE stateChangeConverted = stateChange.invitationCreated;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.invitation = new PARTY_INVITATION_HANDLE(stateChangeConverted.invitation);
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_INVITATION_HANDLE invitation { get; }
    }

    public class PARTY_INVITATION_DESTROYED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_INVITATION_DESTROYED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_INVITATION_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.invitationDestroyed;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.invitation = new PARTY_INVITATION_HANDLE(stateChangeConverted.invitation);
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_INVITATION_HANDLE invitation { get; }
        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_CREATE_ENDPOINT_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CREATE_ENDPOINT_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CREATE_ENDPOINT_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.createEndpointCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.localEndpoint = new PARTY_ENDPOINT_HANDLE(stateChangeConverted.localEndpoint);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public Object asyncIdentifier { get; }
        public PARTY_ENDPOINT_HANDLE localEndpoint { get; }
    }

    public class PARTY_ENDPOINT_CREATED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_ENDPOINT_CREATED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_ENDPOINT_CREATED_STATE_CHANGE stateChangeConverted = stateChange.endpointCreated;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.endpoint = new PARTY_ENDPOINT_HANDLE(stateChangeConverted.endpoint);
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_ENDPOINT_HANDLE endpoint { get; }
    }

    public class PARTY_ENDPOINT_DESTROYED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_ENDPOINT_DESTROYED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_ENDPOINT_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.endpointDestroyed;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.endpoint = new PARTY_ENDPOINT_HANDLE(stateChangeConverted.endpoint);
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_ENDPOINT_HANDLE endpoint { get; }
        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_REMOTE_DEVICE_CREATED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REMOTE_DEVICE_CREATED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REMOTE_DEVICE_CREATED_STATE_CHANGE stateChangeConverted = stateChange.remoteDeviceCreated;
            this.device = new PARTY_DEVICE_HANDLE(stateChangeConverted.device);
        }

        public PARTY_DEVICE_HANDLE device { get; }
    }

    public class PARTY_REMOTE_DEVICE_DESTROYED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REMOTE_DEVICE_DESTROYED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REMOTE_DEVICE_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.remoteDeviceDestroyed;
            this.device = new PARTY_DEVICE_HANDLE(stateChangeConverted.device);
        }

        public PARTY_DEVICE_HANDLE device { get; }
    }

    public class PARTY_REMOTE_DEVICE_JOINED_NETWORK_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REMOTE_DEVICE_JOINED_NETWORK_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REMOTE_DEVICE_JOINED_NETWORK_STATE_CHANGE stateChangeConverted = stateChange.remoteDeviceJoinedNetwork;
            this.device = new PARTY_DEVICE_HANDLE(stateChangeConverted.device);
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
        }

        public PARTY_DEVICE_HANDLE device { get; }
        public PARTY_NETWORK_HANDLE network { get; }
    }

    public class PARTY_REMOTE_DEVICE_LEFT_NETWORK_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REMOTE_DEVICE_LEFT_NETWORK_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REMOTE_DEVICE_LEFT_NETWORK_STATE_CHANGE stateChangeConverted = stateChange.remoteDeviceLeftNetwork;
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.device = new PARTY_DEVICE_HANDLE(stateChangeConverted.device);
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
        }

        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
        public PARTY_DEVICE_HANDLE device { get; }
        public PARTY_NETWORK_HANDLE network { get; }
    }

    public class PARTY_ENDPOINT_MESSAGE_RECEIVED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_ENDPOINT_MESSAGE_RECEIVED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_ENDPOINT_MESSAGE_RECEIVED_STATE_CHANGE stateChangeConverted = stateChange.endpointMessageReceived;
            // This can be a high frequency state change, using object pool to improve performance.
            this.useObjectPool = true;
            this.network = SDK.objectPool.Retrieve<PARTY_NETWORK_HANDLE>(stateChangeConverted.network);
            this.senderEndpoint = SDK.objectPool.Retrieve<PARTY_ENDPOINT_HANDLE>(stateChangeConverted.senderEndpoint);
            this.receiverEndpoints = Converters.PtrToClassListFromPool<PARTY_ENDPOINT_HANDLE, Interop.PARTY_ENDPOINT_HANDLE>(
                stateChangeConverted.receiverEndpoints,
                stateChangeConverted.receiverEndpointCount,
                SDK.objectPool);
            this.options = stateChangeConverted.options;
            this.messageSize = stateChangeConverted.messageSize;
            this.messageBuffer = stateChangeConverted.messageBuffer;
        }

        internal override void Cleanup()
        {
            SDK.objectPool.Return(network);
            SDK.objectPool.Return(senderEndpoint);
            foreach (var endpoint in receiverEndpoints)
            {
                SDK.objectPool.Return(endpoint);
            }
            receiverEndpoints.Clear();
            SDK.objectPool.Return(receiverEndpoints);
            base.Cleanup();
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_ENDPOINT_HANDLE senderEndpoint { get; }
        public List<PARTY_ENDPOINT_HANDLE> receiverEndpoints { get; }
        public PARTY_MESSAGE_RECEIVED_OPTIONS options { get; }
        public UInt32 messageSize { get; }
        public IntPtr messageBuffer { get; }
    }

    public class PARTY_CREATE_INVITATION_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CREATE_INVITATION_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CREATE_INVITATION_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.createInvitationCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.invitation = new PARTY_INVITATION_HANDLE(stateChangeConverted.invitation);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public Object asyncIdentifier { get; }
        public PARTY_INVITATION_HANDLE invitation { get; }
    }

    public class PARTY_DESTROY_ENDPOINT_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_DESTROY_ENDPOINT_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_DESTROY_ENDPOINT_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.destroyEndpointCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localEndpoint = new PARTY_ENDPOINT_HANDLE(stateChangeConverted.localEndpoint);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_ENDPOINT_HANDLE localEndpoint { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_LEAVE_NETWORK_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_LEAVE_NETWORK_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_LEAVE_NETWORK_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.leaveNetworkCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_REMOVE_LOCAL_USER_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REMOVE_LOCAL_USER_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REMOVE_LOCAL_USER_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.removeLocalUserCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_REVOKE_INVITATION_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_REVOKE_INVITATION_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_REVOKE_INVITATION_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.revokeInvitationCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.invitation = new PARTY_INVITATION_HANDLE(stateChangeConverted.invitation);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public PARTY_INVITATION_HANDLE invitation { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_NETWORK_DESCRIPTOR_CHANGED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_NETWORK_DESCRIPTOR_CHANGED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_NETWORK_DESCRIPTOR_CHANGED_STATE_CHANGE stateChangeConverted = stateChange.networkDescriptorChanged;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
        }

        public PARTY_NETWORK_HANDLE network { get; }
    }

    public class PARTY_LOCAL_USER_KICKED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_LOCAL_USER_KICKED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_LOCAL_USER_KICKED_STATE_CHANGE stateChangeConverted = stateChange.localUserKicked;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
    }

    public class PARTY_KICK_DEVICE_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_KICK_DEVICE_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_KICK_DEVICE_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.kickDeviceCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.kickedDevice = new PARTY_DEVICE_HANDLE(stateChangeConverted.kickedDevice);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_DEVICE_HANDLE kickedDevice { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_KICK_USER_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_KICK_USER_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_KICK_USER_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.kickUserCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.kickedEntityId = Converters.PtrToStringUTF8(stateChangeConverted.kickedEntityId);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public string kickedEntityId { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_CREATE_CHAT_CONTROL_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CREATE_CHAT_CONTROL_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CREATE_CHAT_CONTROL_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.createChatControlCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localDevice = new PARTY_DEVICE_HANDLE(stateChangeConverted.localDevice);
            this.localUser = new PARTY_LOCAL_USER_HANDLE(stateChangeConverted.localUser);
            this.languageCode = Converters.PtrToStringUTF8(stateChangeConverted.languageCode);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_DEVICE_HANDLE localDevice { get; }
        public PARTY_LOCAL_USER_HANDLE localUser { get; }
        public string languageCode { get; }
        public Object asyncIdentifier { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
    }

    public class PARTY_DESTROY_CHAT_CONTROL_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_DESTROY_CHAT_CONTROL_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_DESTROY_CHAT_CONTROL_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.destroyChatControlCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localDevice = new PARTY_DEVICE_HANDLE(stateChangeConverted.localDevice);
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_DEVICE_HANDLE localDevice { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_CHAT_CONTROL_CREATED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CHAT_CONTROL_CREATED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CHAT_CONTROL_CREATED_STATE_CHANGE stateChangeConverted = stateChange.chatControlCreated;
            this.chatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.chatControl);
        }

        public PARTY_CHAT_CONTROL_HANDLE chatControl { get; }
    }

    public class PARTY_CHAT_CONTROL_DESTROYED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CHAT_CONTROL_DESTROYED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CHAT_CONTROL_DESTROYED_STATE_CHANGE stateChangeConverted = stateChange.chatControlDestroyed;
            this.chatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.chatControl);
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_CHAT_CONTROL_HANDLE chatControl { get; }
        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_CHAT_CONTROL_JOINED_NETWORK_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CHAT_CONTROL_JOINED_NETWORK_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CHAT_CONTROL_JOINED_NETWORK_STATE_CHANGE stateChangeConverted = stateChange.chatControlJoinedNetwork;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.chatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.chatControl);
        }

        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_CHAT_CONTROL_HANDLE chatControl { get; }
    }

    public class PARTY_CHAT_CONTROL_LEFT_NETWORK_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CHAT_CONTROL_LEFT_NETWORK_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CHAT_CONTROL_LEFT_NETWORK_STATE_CHANGE stateChangeConverted = stateChange.chatControlLeftNetwork;
            this.reason = stateChangeConverted.reason;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.chatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.chatControl);
        }

        public PARTY_DESTROYED_REASON reason { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_CHAT_CONTROL_HANDLE chatControl { get; }
    }

    public class PARTY_CONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.connectChatControlCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_DISCONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_DISCONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_DISCONNECT_CHAT_CONTROL_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.disconnectChatControlCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.network = new PARTY_NETWORK_HANDLE(stateChangeConverted.network);
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_NETWORK_HANDLE network { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_CHAT_AUDIO_INPUT_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_CHAT_AUDIO_INPUT_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_CHAT_AUDIO_INPUT_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setChatAudioInputCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.audioDeviceSelectionType = stateChangeConverted.audioDeviceSelectionType;
            this.audioDeviceSelectionContext = Converters.PtrToStringUTF8(stateChangeConverted.audioDeviceSelectionContext);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType { get; }
        public string audioDeviceSelectionContext { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_CHAT_AUDIO_OUTPUT_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_CHAT_AUDIO_OUTPUT_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_CHAT_AUDIO_OUTPUT_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setChatAudioOutputCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.audioDeviceSelectionType = stateChangeConverted.audioDeviceSelectionType;
            this.audioDeviceSelectionContext = Converters.PtrToStringUTF8(stateChangeConverted.audioDeviceSelectionContext);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType { get; }
        public string audioDeviceSelectionContext { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_LOCAL_CHAT_AUDIO_INPUT_CHANGED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_LOCAL_CHAT_AUDIO_INPUT_CHANGED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_LOCAL_CHAT_AUDIO_INPUT_CHANGED_STATE_CHANGE stateChangeConverted = stateChange.localChatAudioInputChanged;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.state = stateChangeConverted.state;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_AUDIO_INPUT_STATE state { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_LOCAL_CHAT_AUDIO_OUTPUT_CHANGED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_LOCAL_CHAT_AUDIO_OUTPUT_CHANGED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_LOCAL_CHAT_AUDIO_OUTPUT_CHANGED_STATE_CHANGE stateChangeConverted = stateChange.localChatAudioOutputChanged;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.state = stateChangeConverted.state;
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_AUDIO_OUTPUT_STATE state { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_POPULATE_AVAILABLE_TEXT_TO_SPEECH_PROFILES_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_POPULATE_AVAILABLE_TEXT_TO_SPEECH_PROFILES_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_POPULATE_AVAILABLE_TEXT_TO_SPEECH_PROFILES_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.populateAvailableTextToSpeechProfilesCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_LANGUAGE_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_LANGUAGE_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_LANGUAGE_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setLanguageCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.languageCode = Converters.PtrToStringUTF8(stateChangeConverted.languageCode);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public string languageCode { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_CHAT_TEXT_RECEIVED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_CHAT_TEXT_RECEIVED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_CHAT_TEXT_RECEIVED_STATE_CHANGE stateChangeConverted = stateChange.chatTextReceived;
            this.senderChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.senderChatControl);
            this.receiverChatControls = Converters.PtrToClassArray<PARTY_CHAT_CONTROL_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE>(
                stateChangeConverted.receiverChatControls,
                stateChangeConverted.receiverChatControlCount,
                (x) => new PARTY_CHAT_CONTROL_HANDLE(x));
            this.languageCode = Converters.PtrToStringUTF8(stateChangeConverted.languageCode);
            this.chatText = Converters.PtrToStringUTF8(stateChangeConverted.chatText);
            this.data = new Byte[stateChangeConverted.dataSize];
            if (stateChangeConverted.dataSize > 0)
            {
                Marshal.Copy(stateChangeConverted.data, this.data, 0, (int)stateChangeConverted.dataSize);
            }
            this.translations = Converters.PtrToClassArray<PARTY_TRANSLATION, Interop.PARTY_TRANSLATION>(
                stateChangeConverted.translations,
                stateChangeConverted.translationCount,
                (x) => new PARTY_TRANSLATION(x));
            this.options = stateChangeConverted.options;
            this.originalChatText = Converters.PtrToStringUTF8(stateChangeConverted.originalChatText);
            this.errorDetail = stateChangeConverted.errorDetail;
        }

        public PARTY_CHAT_CONTROL_HANDLE senderChatControl { get; }
        public PARTY_CHAT_CONTROL_HANDLE[] receiverChatControls { get; }
        public string languageCode { get; }
        public string chatText { get; }
        public Byte[] data { get; }
        public PARTY_TRANSLATION[] translations { get; }
        public PARTY_CHAT_TEXT_RECEIVED_OPTIONS options { get; }
        public string originalChatText { get; }
        public UInt32 errorDetail { get; }
    }

    public class PARTY_SET_CHAT_AUDIO_ENCODER_BITRATE_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_CHAT_AUDIO_ENCODER_BITRATE_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_CHAT_AUDIO_ENCODER_BITRATE_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setChatAudioEncoderBitrateCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.bitrate = stateChangeConverted.bitrate;
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public UInt32 bitrate { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_TEXT_CHAT_OPTIONS_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_TEXT_CHAT_OPTIONS_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_TEXT_CHAT_OPTIONS_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setTextChatOptionsCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.options = stateChangeConverted.options;
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_TEXT_CHAT_OPTIONS options { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_TEXT_TO_SPEECH_PROFILE_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_TEXT_TO_SPEECH_PROFILE_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_TEXT_TO_SPEECH_PROFILE_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setTextToSpeechProfileCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.type = stateChangeConverted.type;
            this.profileIdentifier = Converters.PtrToStringUTF8(stateChangeConverted.profileIdentifier);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type { get; }
        public string profileIdentifier { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_SET_TRANSCRIPTION_OPTIONS_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SET_TRANSCRIPTION_OPTIONS_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SET_TRANSCRIPTION_OPTIONS_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.setTranscriptionOptionsCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.options = stateChangeConverted.options;
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options { get; }
        public Object asyncIdentifier { get; }
    }

    public class PARTY_VOICE_CHAT_TRANSCRIPTION_RECEIVED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_VOICE_CHAT_TRANSCRIPTION_RECEIVED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_VOICE_CHAT_TRANSCRIPTION_RECEIVED_STATE_CHANGE stateChangeConverted = stateChange.voiceChatTranscriptionReceived;
            this.useObjectPool = true;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.senderChatControl = SDK.objectPool.Retrieve<PARTY_CHAT_CONTROL_HANDLE>(stateChangeConverted.senderChatControl);
            this.receiverChatControls = Converters.PtrToClassListFromPool<PARTY_CHAT_CONTROL_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE>(
                stateChangeConverted.receiverChatControls,
                stateChangeConverted.receiverChatControlCount,
                SDK.objectPool);
            this.sourceType = stateChangeConverted.sourceType;
            this.languageCode = Converters.PtrToStringUTF8(stateChangeConverted.languageCode);
            this.transcription = Converters.PtrToStringUTF8(stateChangeConverted.transcription);
            this.type = stateChangeConverted.type;
            this.translations = Converters.PtrToClassListFromPool<PARTY_TRANSLATION, Interop.PARTY_TRANSLATION>(
                stateChangeConverted.translations,
                stateChangeConverted.translationCount,
                SDK.objectPool);
        }

        internal override void Cleanup()
        {
            SDK.objectPool.Return(senderChatControl);
            foreach (var chatControl in receiverChatControls)
            {
                SDK.objectPool.Return(chatControl);
            }

            foreach (var translation in translations)
            {
                SDK.objectPool.Return(translation);
            }

            receiverChatControls.Clear();
            SDK.objectPool.Return(receiverChatControls);
            translations.Clear();
            SDK.objectPool.Return(translations);
            base.Cleanup();
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE senderChatControl { get; }
        public List<PARTY_CHAT_CONTROL_HANDLE> receiverChatControls { get; }
        public PARTY_AUDIO_SOURCE_TYPE sourceType { get; }
        public string languageCode { get; }
        public string transcription { get; }
        public PARTY_VOICE_CHAT_TRANSCRIPTION_PHRASE_TYPE type { get; }
        public List<PARTY_TRANSLATION> translations { get; }
    }

    public class PARTY_SYNTHESIZE_TEXT_TO_SPEECH_COMPLETED_STATE_CHANGE : PARTY_STATE_CHANGE
    {
        internal PARTY_SYNTHESIZE_TEXT_TO_SPEECH_COMPLETED_STATE_CHANGE(
            PARTY_STATE_CHANGE_UNION stateChange, IntPtr StateChangeId
            ) : base(stateChange.stateChange.stateChangeType, StateChangeId)
        {
            Interop.PARTY_SYNTHESIZE_TEXT_TO_SPEECH_COMPLETED_STATE_CHANGE stateChangeConverted = stateChange.synthesizeTextToSpeechCompleted;
            this.result = stateChangeConverted.result;
            this.errorDetail = stateChangeConverted.errorDetail;
            this.localChatControl = new PARTY_CHAT_CONTROL_HANDLE(stateChangeConverted.localChatControl);
            this.type = stateChangeConverted.type;
            this.textToSynthesize = Converters.PtrToStringUTF8(stateChangeConverted.textToSynthesize);
            this.asyncIdentifier = null;
            if (stateChangeConverted.asyncIdentifier != IntPtr.Zero)
            {
                GCHandle asyncGcHandle = GCHandle.FromIntPtr(stateChangeConverted.asyncIdentifier);
                this.asyncIdentifier = asyncGcHandle.Target;
                asyncGcHandle.Free();
            }
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public PARTY_CHAT_CONTROL_HANDLE localChatControl { get; }
        public PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type { get; }
        public string textToSynthesize { get; }
        public Object asyncIdentifier { get; }
    }
}