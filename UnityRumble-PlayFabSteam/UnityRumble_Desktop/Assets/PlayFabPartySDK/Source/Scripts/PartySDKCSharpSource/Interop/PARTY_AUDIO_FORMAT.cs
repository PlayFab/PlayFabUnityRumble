using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_AUDIO_FORMAT
    //{
    //    uint32_t samplesPerSecond;
    //    uint32_t channelMask;
    //    uint16_t channelCount;
    //    uint16_t bitsPerSample;
    //    PARTY_AUDIO_SAMPLE_TYPE sampleType;
    //    PartyBool interleaved;
    //} PARTY_AUDIO_FORMAT;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_AUDIO_FORMAT
    {
        internal readonly UInt32 samplesPerSecond;
        internal readonly UInt32 channelMask;
        internal readonly UInt16 channelCount;
        internal readonly UInt16 bitsPerSample;
        internal readonly PARTY_AUDIO_SAMPLE_TYPE sampleType;
        internal readonly Byte interleaved;

        internal PARTY_AUDIO_FORMAT(PartyCSharpSDK.PARTY_AUDIO_FORMAT publicObject)
        {
            this.samplesPerSecond = publicObject.SamplesPerSecond;
            this.channelMask = publicObject.ChannelMask;
            this.channelCount = publicObject.ChannelCount;
            this.bitsPerSample = publicObject.BitsPerSample;
            this.sampleType = publicObject.SampleType;
            this.interleaved = publicObject.Interleaved;
        }
    }
}