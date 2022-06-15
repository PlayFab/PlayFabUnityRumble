using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_ENDPOINT_HANDLE
    {
        internal readonly IntPtr handle;
    }
}
