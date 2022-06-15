using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_NETWORK_CONFIGURATION
    //{
    //    uint32_t maxUserCount;
    //    _Field_range_(1, PARTY_MAX_NETWORK_CONFIGURATION_MAX_DEVICE_COUNT) uint32_t maxDeviceCount;
    //    _Field_range_(1, PARTY_MAX_LOCAL_USERS_PER_DEVICE_COUNT) uint32_t maxUsersPerDeviceCount;
    //    uint32_t maxDevicesPerUserCount;
    //    _Field_range_(0, PARTY_MAX_NETWORK_CONFIGURATION_MAX_ENDPOINTS_PER_DEVICE_COUNT) uint32_t maxEndpointsPerDeviceCount;
    //    PARTY_DIRECT_PEER_CONNECTIVITY_OPTIONS directPeerConnectivityOptions;
    //} PARTY_NETWORK_CONFIGURATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_NETWORK_CONFIGURATION
    {
        internal readonly UInt32 maxUserCount;
        internal readonly UInt32 maxDeviceCount;
        internal readonly UInt32 maxUsersPerDeviceCount;
        internal readonly UInt32 maxDevicesPerUserCount;
        internal readonly UInt32 maxEndpointsPerDeviceCount;
        internal readonly PARTY_DIRECT_PEER_CONNECTIVITY_OPTIONS directPeerConnectivityOptions;

        internal PARTY_NETWORK_CONFIGURATION(PartyCSharpSDK.PARTY_NETWORK_CONFIGURATION publicObject)
        {
            this.maxUserCount = publicObject.MaxUserCount;
            this.maxDeviceCount = publicObject.MaxDeviceCount;
            this.maxUsersPerDeviceCount = publicObject.MaxUsersPerDeviceCount;
            this.maxDevicesPerUserCount = publicObject.MaxDevicesPerUserCount;
            this.maxEndpointsPerDeviceCount = publicObject.MaxEndpointsPerDeviceCount;
            this.directPeerConnectivityOptions = publicObject.DirectPeerConnectivityOptions;
        }
    }
}