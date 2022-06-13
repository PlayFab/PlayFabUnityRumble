using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION
    //{
    //    PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_OPTIONS options;
    //    uint16_t port;
    //}
    //PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION
    {
        internal readonly PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_OPTIONS options;
        internal readonly UInt16 port;

        internal PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION(PartyCSharpSDK.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION publicObject)
        {
            this.options = publicObject.options;
            this.port = publicObject.port;
        }
    }
}