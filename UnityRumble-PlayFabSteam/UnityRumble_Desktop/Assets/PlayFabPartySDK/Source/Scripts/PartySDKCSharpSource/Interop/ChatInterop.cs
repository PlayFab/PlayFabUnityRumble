using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    partial class PFPInterop
    {
        //PartyChatControlGetAudioManipulationCaptureStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PARTY_AUDIO_MANIPULATION_SINK_STREAM_HANDLE* stream
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartyChatControlGetAudioManipulationCaptureStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    out PARTY_AUDIO_MANIPULATION_SINK_STREAM_HANDLE stream);

        //PartyChatControlSendText(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    uint32_t targetChatControlCount,
        //    _In_reads_(targetChatControlCount) const PARTY_CHAT_CONTROL_HANDLE* targetChatControls,
        //    PartyString chatText,
        //    uint32_t dataBufferCount,
        //    _In_reads_(dataBufferCount) const PARTY_DATA_BUFFER* dataBuffers
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSendText(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            UInt32 targetChatControlCount,
            IntPtr targetChatControls,
            Byte[] chatText,
            UInt32 dataBufferCount,
            IntPtr dataBuffers);

        //PartyChatControlGetAudioEncoderBitrate(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ uint32_t* bitrate
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetAudioEncoderBitrate(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UInt32 bitrate);

        //PartyChatControlSetAudioEncoderBitrate(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    uint32_t bitrate,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetAudioEncoderBitrate(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            UInt32 bitrate,
            IntPtr asyncIdentifier);

        //PartyChatControlConfigureAudioManipulationCaptureStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION* configuration,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlConfigureAudioManipulationCaptureStream(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION* configuration,
            IntPtr asyncIdentifier);

        //PartyChatControlGetPropertyKeys(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ uint32_t* propertyCount,
        //    _Outptr_result_buffer_(*propertyCount) const PartyString** keys
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlGetPropertyKeys(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UInt32 propertyCount,
            out UTF8StringPtr* keys);

        //PartyChatControlGetAudioManipulationVoiceStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_HANDLE* stream
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartyChatControlGetAudioManipulationVoiceStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    out PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_HANDLE stream);

        //PartyChatControlSetTextToSpeechProfile(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
        //    _In_opt_ PartyString profileIdentifier,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetTextToSpeechProfile(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            Byte[] profileIdentifier,
            IntPtr asyncIdentifier);

        //PartyChatControlGetTextToSpeechProfile(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
        //    _Outptr_result_maybenull_ PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE* profile
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetTextToSpeechProfile(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            out PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile);

        //PartyTextToSpeechProfileGetName(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _Outptr_ PartyString* name
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileGetName(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out UTF8StringPtr name);

        //PartyTextToSpeechProfileGetGender(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _Out_ PARTY_GENDER* gender
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileGetGender(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out PARTY_GENDER gender);

        //PartyChatControlGetLanguage(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PartyString* languageCode
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetLanguage(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UTF8StringPtr languageCode);

        //PartyChatControlSetAudioOutput(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
        //    _In_opt_ PartyString audioDeviceSelectionContext,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetAudioOutput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            Byte[] audioDeviceSelectionContext,
            IntPtr asyncIdentifier);

        //PartyChatControlGetAudioOutput(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PARTY_AUDIO_DEVICE_SELECTION_TYPE* audioDeviceSelectionType,
        //    _Outptr_ PartyString* audioDeviceSelectionContext,
        //    _Outptr_ PartyString* deviceId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetAudioOutput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            out UTF8StringPtr audioDeviceSelectionContext,
            out UTF8StringPtr deviceId);

        //PartyChatControlSetLanguage(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ PartyString languageCode,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetLanguage(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Byte[] languageCode,
            IntPtr asyncIdentifier);

        //PartyChatControlGetDevice(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PARTY_DEVICE_HANDLE* device
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetDevice(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_DEVICE_HANDLE device);

        //PartyChatControlSetProperties(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    uint32_t propertyCount,
        //    _In_reads_(propertyCount) const PartyString* keys,
        //    _In_reads_(propertyCount) const PARTY_DATA_BUFFER* values
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetProperties(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            UInt32 propertyCount,
            [In] UTF8StringPtr[] keys,
            [In] PARTY_DATA_BUFFER[] values);

        //PartyChatControlGetAvailableTextToSpeechProfiles(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ uint32_t* profileCount,
        //    _Outptr_result_buffer_(*profileCount) const PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE** profiles
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlGetAvailableTextToSpeechProfiles(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UInt32 profileCount,
            out IntPtr profiles);

        //PartyChatControlGetCustomContext(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetCustomContext(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out IntPtr customContext);

        //PartyTextToSpeechProfileGetLanguageCode(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _Outptr_ PartyString* languageCode
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileGetLanguageCode(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out UTF8StringPtr languageCode);

        //PartyChatControlSetCustomContext(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetCustomContext(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            IntPtr customContext);

        //PartyChatControlIsLocal(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PartyBool* isLocal
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlIsLocal(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out Byte isLocal);

        //PartyChatControlGetIncomingTextMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    _Out_ PartyBool* muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetIncomingTextMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out Byte muted);

        //PartyChatControlSetIncomingTextMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    PartyBool muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetIncomingTextMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            Byte muted);

        //PartyChatControlSetPermissions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    PARTY_CHAT_PERMISSION_OPTIONS chatPermissionOptions
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetPermissions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            PARTY_CHAT_PERMISSION_OPTIONS chatPermissionOptions);

        //PartyChatControlGetPermissions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    _Out_ PARTY_CHAT_PERMISSION_OPTIONS* chatPermissionOptions
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetPermissions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out PARTY_CHAT_PERMISSION_OPTIONS chatPermissionOptions);

        //PartyChatControlGetNetworks(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ uint32_t* networkCount,
        //    _Outptr_result_buffer_(*networkCount) const PARTY_NETWORK_HANDLE** networks
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlGetNetworks(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UInt32 networkCount,
            out IntPtr networks);

        //PartyChatControlConfigureAudioManipulationVoiceStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION* configuration,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlConfigureAudioManipulationVoiceStream(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION* configuration,
            IntPtr asyncIdentifier);

        //PartyChatControlGetLocalChatIndicator(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PARTY_LOCAL_CHAT_CONTROL_CHAT_INDICATOR* chatIndicator
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetLocalChatIndicator(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_LOCAL_CHAT_CONTROL_CHAT_INDICATOR chatIndicator);

        //PartyChatControlGetProperty(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PartyString key,
        //    _Outptr_result_maybenull_ const PARTY_DATA_BUFFER** value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlGetProperty(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Byte[] key,
            out PARTY_DATA_BUFFER* value);

        //PartyChatControlGetAudioInputMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PartyBool* muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetAudioInputMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out Byte muted);

        //PartyChatControlSetAudioInputMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PartyBool muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetAudioInputMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Byte muted);

        //PartyChatControlSetAudioRenderVolume(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    float volume
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetAudioRenderVolume(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            float volume);

        //PartyChatControlConfigureAudioManipulationRenderStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION* configuration,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyChatControlConfigureAudioManipulationRenderStream(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION* configuration,
            IntPtr asyncIdentifier);

        //PartyChatControlGetAudioRenderVolume(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    _Out_ float* volume
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetAudioRenderVolume(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out float volume);

        //PartyChatControlGetAudioManipulationRenderStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PARTY_AUDIO_MANIPULATION_SINK_STREAM_HANDLE* stream
        //    );
        //[DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        //internal static extern UInt32 PartyChatControlGetAudioManipulationRenderStream(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    out PARTY_AUDIO_MANIPULATION_SINK_STREAM_HANDLE stream);

        //PartyChatControlGetEntityId(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PartyString* entityId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetEntityId(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UTF8StringPtr entityId);

        //PartyChatControlGetTranscriptionOptions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS* options
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetTranscriptionOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options);

        //PartyTextToSpeechProfileGetIdentifier(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _Outptr_ PartyString* identifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileGetIdentifier(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out UTF8StringPtr identifier);

        //PartyChatControlSetTranscriptionOptions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetTranscriptionOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options,
            IntPtr asyncIdentifier);

        //PartyTextToSpeechProfileSetCustomContext(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileSetCustomContext(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            IntPtr customContext);

        //PartyTextToSpeechProfileGetCustomContext(
        //    PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyTextToSpeechProfileGetCustomContext(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out IntPtr customContext);

        //PartyChatControlPopulateAvailableTextToSpeechProfiles(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlPopulateAvailableTextToSpeechProfiles(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            IntPtr asyncIdentifier);

        //PartyChatControlGetChatIndicator(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    _Out_ PARTY_CHAT_CONTROL_CHAT_INDICATOR* chatIndicator
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetChatIndicator(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out PARTY_CHAT_CONTROL_CHAT_INDICATOR chatIndicator);
        //PartyChatControlSetTextChatOptions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_TEXT_CHAT_OPTIONS options,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetTextChatOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_TEXT_CHAT_OPTIONS options,
            IntPtr asyncIdentifier);

        //PartyChatControlGetTextChatOptions(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PARTY_TEXT_CHAT_OPTIONS* options
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetTextChatOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_TEXT_CHAT_OPTIONS options);

        //PartyChatControlGetIncomingAudioMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    _Out_ PartyBool* muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetIncomingAudioMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out Byte muted);
        //PartyChatControlSetIncomingAudioMuted(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_CHAT_CONTROL_HANDLE targetChatControl,
        //    PartyBool muted
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetIncomingAudioMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            Byte muted);

        //PartyChatControlGetLocalUser(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Outptr_ PARTY_LOCAL_USER_HANDLE* localUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetLocalUser(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_LOCAL_USER_HANDLE localUser);

        //PartyChatControlSetAudioInput(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
        //    _In_opt_ PartyString audioDeviceSelectionContext,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSetAudioInput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            Byte[] audioDeviceSelectionContext,
            IntPtr asyncIdentifier);

        //PartyChatControlGetAudioInput(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _Out_ PARTY_AUDIO_DEVICE_SELECTION_TYPE* audioDeviceSelectionType,
        //    _Outptr_ PartyString* audioDeviceSelectionContext,
        //    _Outptr_ PartyString* deviceId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlGetAudioInput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            out UTF8StringPtr audioDeviceSelectionContext,
            out UTF8StringPtr deviceId);

        //PartyChatControlSynthesizeTextToSpeech(
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
        //    PartyString textToSynthesize,
        //    _In_opt_ void * asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyChatControlSynthesizeTextToSpeech(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            Byte[] textToSynthesize,
            IntPtr asyncIdentifier);
    }
}
