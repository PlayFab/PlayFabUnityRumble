using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_REGION
    //{
    //    char regionName[PARTY_MAX_REGION_NAME_STRING_LENGTH + 1];
    //    uint32_t roundTripLatencyInMilliseconds;
    //} PARTY_REGION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_REGION
    {
        private unsafe fixed Byte regionName[PartyConstants.c_maxRegionNameStringLength + 1];
        internal readonly UInt32 roundTripLatencyInMilliseconds;

        internal string GetRegionName() { unsafe { fixed (Byte* ptr = this.regionName) { return Converters.BytePointerToString(ptr, PartyConstants.c_maxRegionNameStringLength + 1); } } }

        internal PARTY_REGION(PartyCSharpSDK.PARTY_REGION publicObject)
        {
            unsafe
            {
                fixed (Byte* ptr = this.regionName)
                {
                    Converters.StringToNullTerminatedUTF8FixedPointer(publicObject.RegionName, ptr, PartyConstants.c_maxRegionNameStringLength + 1);
                }
            }
            this.roundTripLatencyInMilliseconds = publicObject.RoundTripLatencyInMilliseconds;
        }
    }
}