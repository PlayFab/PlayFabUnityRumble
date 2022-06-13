using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION
    {
        internal PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION(Interop.PARTY_AUDIO_MANIPULATION_SINK_STREAM_CONFIGURATION interopStruct)
        {
            this.Format = new PARTY_AUDIO_FORMAT(interopStruct.format);
        }

        public PARTY_AUDIO_FORMAT Format { get; }
    }
}