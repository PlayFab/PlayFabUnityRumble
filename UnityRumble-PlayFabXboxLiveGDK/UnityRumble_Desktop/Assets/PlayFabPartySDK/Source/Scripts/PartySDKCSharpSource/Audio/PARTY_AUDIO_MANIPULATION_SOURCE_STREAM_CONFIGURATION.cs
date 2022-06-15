using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION
    {
        internal PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION(Interop.PARTY_AUDIO_MANIPULATION_SOURCE_STREAM_CONFIGURATION interopStruct)
        {
            this.Format = new PARTY_AUDIO_FORMAT(interopStruct.format);
            this.MaxTotalAudioBufferSizeInMilliseconds = interopStruct.maxTotalAudioBufferSizeInMilliseconds;
        }

        public PARTY_AUDIO_FORMAT Format { get; }
        public UInt32 MaxTotalAudioBufferSizeInMilliseconds { get; }
    }
}