using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_NETWORK_DESCRIPTOR
    //{
    //    char networkIdentifier[PARTY_NETWORK_IDENTIFIER_STRING_LENGTH + 1];
    //    char regionName[PARTY_MAX_REGION_NAME_STRING_LENGTH + 1];
    //    uint8_t opaqueConnectionInformation[PARTY_OPAQUE_CONNECTION_INFORMATION_BYTE_COUNT];
    //} PARTY_NETWORK_DESCRIPTOR;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_NETWORK_DESCRIPTOR
    {
        private unsafe fixed Byte networkIdentifier[PartyConstants.c_networkIdentifierStringLength + 1]; // char networkIdentifier[PartyConstants.c_networkIdentifierStringLength + 1]
        private unsafe fixed Byte regionName[PartyConstants.c_maxRegionNameStringLength + 1]; // char regionName[PartyConstants.c_maxRegionNameStringLength + 1]
        private unsafe fixed Byte opaqueConnectionInformation[PartyConstants.c_opaqueConnectionInformationByteCount + 1]; // Byte opaqueConnectionInformation[PartyConstants.c_opaqueConnectionInformationByteCount + 1]

        internal string GetNetworkIdentifier() { unsafe { fixed (Byte* ptr = this.networkIdentifier) { return Converters.BytePointerToString(ptr, PartyConstants.c_networkIdentifierStringLength + 1); } } }
        internal string GetRegionName() { unsafe { fixed (Byte* ptr = this.regionName) { return Converters.BytePointerToString(ptr, PartyConstants.c_maxRegionNameStringLength + 1); } } }
        internal Byte[] GetOpaqueConnectionInformation()
        {
            unsafe
            {
                fixed (Byte* ptr = this.opaqueConnectionInformation)
                {
                    Byte[] bytes = new Byte[PartyConstants.c_opaqueConnectionInformationByteCount + 1];
                    Marshal.Copy(source: (IntPtr)ptr, destination: bytes, startIndex: 0, length: PartyConstants.c_opaqueConnectionInformationByteCount + 1);
                    return bytes;
                }
            }
        }

        internal PARTY_NETWORK_DESCRIPTOR(PartyCSharpSDK.PARTY_NETWORK_DESCRIPTOR publicObject)
        {
            unsafe
            {
                fixed (Byte* ptr = this.networkIdentifier)
                {
                    Converters.StringToNullTerminatedUTF8FixedPointer(publicObject.NetworkIdentifier, ptr, PartyConstants.c_networkIdentifierStringLength + 1);
                }
            }
            unsafe
            {
                fixed (Byte* ptr = this.regionName)
                {
                    Converters.StringToNullTerminatedUTF8FixedPointer(publicObject.RegionName, ptr, PartyConstants.c_maxRegionNameStringLength + 1);
                }
            }
            unsafe
            {
                fixed (Byte* ptr = this.opaqueConnectionInformation)
                {
                    Marshal.Copy(publicObject.OpaqueConnectionInformation, 0, (IntPtr)ptr, PartyConstants.c_opaqueConnectionInformationByteCount + 1);
                }
            }
        }
    }
}