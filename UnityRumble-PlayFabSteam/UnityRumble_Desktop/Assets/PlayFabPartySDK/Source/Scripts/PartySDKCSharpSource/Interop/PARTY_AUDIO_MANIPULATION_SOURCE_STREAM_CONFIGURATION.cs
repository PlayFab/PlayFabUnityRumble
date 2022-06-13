using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION
    //{
    //    PARTY_AUDIO_FORMAT format;
    //    uint32_t maxTotalAudioBufferSizeInMilliseconds;
    //} PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION
    {
        internal readonly PARTY_AUDIO_FORMAT format;
        internal readonly UInt32 maxTotalAudioBufferSizeInMilliseconds;

        internal PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION(PartyCSharpSDK.PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION publicObject)
        {
            this.format = new PARTY_AUDIO_FORMAT(publicObject.Format);
            this.maxTotalAudioBufferSizeInMilliseconds = publicObject.MaxTotalAudioBufferSizeInMilliseconds;
        }
    }
}