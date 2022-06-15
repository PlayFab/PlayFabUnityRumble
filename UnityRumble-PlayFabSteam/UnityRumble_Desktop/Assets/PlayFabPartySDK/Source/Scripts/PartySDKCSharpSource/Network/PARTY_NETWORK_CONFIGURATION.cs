using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_NETWORK_CONFIGURATION
    {
        internal PARTY_NETWORK_CONFIGURATION(Interop.PARTY_NETWORK_CONFIGURATION interopStruct)
        {
            this.MaxUserCount = interopStruct.maxUserCount;
            this.MaxDeviceCount = interopStruct.maxDeviceCount;
            this.MaxUsersPerDeviceCount = interopStruct.maxUsersPerDeviceCount;
            this.MaxDevicesPerUserCount = interopStruct.maxDevicesPerUserCount;
            this.MaxEndpointsPerDeviceCount = interopStruct.maxEndpointsPerDeviceCount;
            this.DirectPeerConnectivityOptions = interopStruct.directPeerConnectivityOptions;
        }

        public PARTY_NETWORK_CONFIGURATION()
        {
        }

        public UInt32 MaxUserCount { get; set; }
        public UInt32 MaxDeviceCount { get; set; }
        public UInt32 MaxUsersPerDeviceCount { get; set; }
        public UInt32 MaxDevicesPerUserCount { get; set; }
        public UInt32 MaxEndpointsPerDeviceCount { get; set; }
        public PARTY_DIRECT_PEER_CONNECTIVITY_OPTIONS DirectPeerConnectivityOptions { get; set; }
    }
}