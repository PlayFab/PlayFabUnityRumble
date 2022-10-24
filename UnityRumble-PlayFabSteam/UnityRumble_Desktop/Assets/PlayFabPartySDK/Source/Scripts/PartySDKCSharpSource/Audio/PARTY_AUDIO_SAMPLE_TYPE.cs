using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_AUDIO_SAMPLE_TYPE
    //{
        // PARTY_AUDIO_SAMPLE_TYPE_INTEGER = 0,
        // PARTY_AUDIO_SAMPLE_TYPE_FLOAT = 1,
    //} PARTY_AUDIO_SAMPLE_TYPE;
    public enum PARTY_AUDIO_SAMPLE_TYPE : UInt32
    {
        PARTY_AUDIO_SAMPLE_TYPE_INTEGER = 0,
        PARTY_AUDIO_SAMPLE_TYPE_FLOAT = 1,
    }
}