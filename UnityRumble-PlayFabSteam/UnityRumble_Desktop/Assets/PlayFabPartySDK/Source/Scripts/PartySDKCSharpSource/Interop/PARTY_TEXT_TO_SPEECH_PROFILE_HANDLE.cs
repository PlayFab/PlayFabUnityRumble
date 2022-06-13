using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE
    {
        private readonly IntPtr handle;
    }
}
