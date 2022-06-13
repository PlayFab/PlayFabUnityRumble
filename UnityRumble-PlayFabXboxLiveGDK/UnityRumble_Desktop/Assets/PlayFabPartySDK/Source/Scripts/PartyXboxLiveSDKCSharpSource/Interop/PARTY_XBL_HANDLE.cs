using System;
using System.Runtime.InteropServices;

namespace PartyXBLCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_HANDLE
    {
        internal readonly IntPtr handle;

        internal PARTY_XBL_HANDLE(Int64 handleValue)
        {
            handle = new IntPtr(handleValue);
        }
    }
}
