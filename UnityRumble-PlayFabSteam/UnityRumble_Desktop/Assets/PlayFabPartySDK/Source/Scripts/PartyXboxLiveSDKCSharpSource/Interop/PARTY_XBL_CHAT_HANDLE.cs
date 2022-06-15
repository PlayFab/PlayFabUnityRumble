using System;
using System.Runtime.InteropServices;

namespace PartyXBLCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_CHAT_HANDLE
    {
        private readonly IntPtr handle;
    }
}
