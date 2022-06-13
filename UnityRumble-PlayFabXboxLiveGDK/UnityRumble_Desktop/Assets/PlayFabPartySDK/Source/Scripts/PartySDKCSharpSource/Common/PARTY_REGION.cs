using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_REGION
    {
        internal PARTY_REGION(Interop.PARTY_REGION interopStruct)
        {
            this.RegionName = interopStruct.GetRegionName();
            this.RoundTripLatencyInMilliseconds = interopStruct.roundTripLatencyInMilliseconds;
        }

        public string RegionName { get; }
        public UInt32 RoundTripLatencyInMilliseconds { get; }
    }
}