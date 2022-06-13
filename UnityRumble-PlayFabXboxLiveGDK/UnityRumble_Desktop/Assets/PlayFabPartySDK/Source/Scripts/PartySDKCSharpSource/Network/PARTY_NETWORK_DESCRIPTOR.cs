using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_NETWORK_DESCRIPTOR
    {
        internal PARTY_NETWORK_DESCRIPTOR(Interop.PARTY_NETWORK_DESCRIPTOR interopStruct)
        {
            this.NetworkIdentifier = interopStruct.GetNetworkIdentifier();
            this.RegionName = interopStruct.GetRegionName();
            this.OpaqueConnectionInformation = interopStruct.GetOpaqueConnectionInformation();
        }

        public string NetworkIdentifier { get; }
        public string RegionName { get; }
        public Byte[] OpaqueConnectionInformation { get; }
    }
}