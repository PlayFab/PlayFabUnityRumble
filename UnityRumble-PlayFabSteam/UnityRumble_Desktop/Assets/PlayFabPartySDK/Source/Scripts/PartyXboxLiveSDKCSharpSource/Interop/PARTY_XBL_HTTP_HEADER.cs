using System;
using System.Runtime.InteropServices;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK.Interop
{
    //typedef struct PARTY_XBL_HTTP_HEADER
    //{
    //    PartyString name;
    //    PartyString value;
    //}
    //PARTY_XBL_HTTP_HEADER;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_HTTP_HEADER
    {
        internal readonly IntPtr name;
        internal readonly IntPtr value;
    }
}