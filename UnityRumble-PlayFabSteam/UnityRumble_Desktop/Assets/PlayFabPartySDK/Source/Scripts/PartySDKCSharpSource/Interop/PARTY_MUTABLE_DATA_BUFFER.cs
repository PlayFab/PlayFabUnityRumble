using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_MUTABLE_DATA_BUFFER
    //{
    //    _Field_size_bytes_(bufferByteCount) void* buffer;
    //    uint32_t bufferByteCount;
    //} PARTY_MUTABLE_DATA_BUFFER;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_MUTABLE_DATA_BUFFER
    {
        internal readonly IntPtr buffer;
        internal readonly UInt32 bufferByteCount;

        internal PARTY_MUTABLE_DATA_BUFFER(PartyCSharpSDK.PARTY_MUTABLE_DATA_BUFFER publicObject)
        {
            this.buffer = publicObject.Buffer;
            this.bufferByteCount = publicObject.BufferByteCount;
        }
    }
}