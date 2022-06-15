using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION
    {
        internal PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION(Interop.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION interopStruct)
        {
            this.options = interopStruct.options;
            this.port = interopStruct.port;
        }

        public PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION()
        {
        }

        public PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_OPTIONS options { get; set; }
        public UInt16 port { get; set; }
    }
}