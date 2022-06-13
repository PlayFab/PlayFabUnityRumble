using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_HANDLE
    {
        internal readonly IntPtr handle;

        internal PARTY_HANDLE(Int64 handleValue)
        {
            handle = new IntPtr(handleValue);
        }
    }
}
