using System;

namespace PartyCSharpSDK
{
    public class PartyConstants
    {
        // Party.h constants
        public const Int32 c_maxNetworkConfigurationMaxDeviceCount = 32;
        public const Int32 c_maxNetworkConfigurationMaxEndpointsPerDeviceCount = 32;
        public const Int32 c_maxLocalUsersPerDeviceCount = 8;
        public const Int32 c_opaqueConnectionInformationByteCount = 300;
        public const Int32 c_maxInvitationIdentifierStringLength = 127;
        public const Int32 c_maxInvitationEntityIdCount = 1024;
        public const Int32 c_maxEntityIdStringLength = 20;
        public const Int32 c_networkIdentifierStringLength = 36;
        public const Int32 c_maxRegionNameStringLength = 19;
        public const Int32 c_maxSerializedNetworkDescriptorStringLength = 448;
        public const Int32 c_maxAudioDeviceIdentifierStringLength = 999;
        public const Int32 c_maxLanguageCodeStringLength = 84;
        public const Int32 c_maxChatTextMessageStringLength = 1023;
        public const Int32 c_maxChatTranscriptionMessageStringLength = 1023;
        public const Int32 c_maxTextToSpeechProfileIdentifierStringLength = 255;
        public const Int32 c_maxTextToSpeechProfileNameStringLength = 127;
        public const Int32 c_maxTextToSpeechInputStringLength = 1023;
        public const UInt64 c_anyProcessor = 0xffffffffffffffff;
        public const Int16 c_minSendMessageQueuingPriority = -5;
        public const Int16 c_chatSendMessageQueuingPriority = -3;
        public const Int16 c_defaultSendMessageQueuingPriority = 0;
        public const Int16 c_maxSendMessageQueuingPriority = 5;
#if PARTY_CHAT_SAVE_NETWORK_DESCRIPTOR_TO_CLOUD
        public const string c_partyChatRoom = "1234";
#endif
    }
}
