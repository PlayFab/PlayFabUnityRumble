using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_AUDIO_SOURCE_TYPE
    //{
        // PARTY_AUDIO_SOURCE_TYPE_MICROPHONE = 0,
        // PARTY_AUDIO_SOURCE_TYPE_TEXT_TO_SPEECH = 1,
    //}
    //PARTY_AUDIO_SOURCE_TYPE;
    public enum PARTY_AUDIO_SOURCE_TYPE : UInt32
    {
        PARTY_AUDIO_SOURCE_TYPE_MICROPHONE = 0,
        PARTY_AUDIO_SOURCE_TYPE_TEXT_TO_SPEECH = 1,
    }
}