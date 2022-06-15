using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_AUDIO_FORMAT
    {
        internal PARTY_AUDIO_FORMAT(Interop.PARTY_AUDIO_FORMAT interopStruct)
        {
            this.SamplesPerSecond = interopStruct.samplesPerSecond;
            this.ChannelMask = interopStruct.channelMask;
            this.ChannelCount = interopStruct.channelCount;
            this.BitsPerSample = interopStruct.bitsPerSample;
            this.SampleType = interopStruct.sampleType;
            this.Interleaved = interopStruct.interleaved;
        }

        public UInt32 SamplesPerSecond { get; }
        public UInt32 ChannelMask { get; }
        public UInt16 ChannelCount { get; }
        public UInt16 BitsPerSample { get; }
        public PARTY_AUDIO_SAMPLE_TYPE SampleType { get; }
        public Byte Interleaved { get; }
    }
}