using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_MUTABLE_DATA_BUFFER
    {
        internal PARTY_MUTABLE_DATA_BUFFER(Interop.PARTY_MUTABLE_DATA_BUFFER interopStruct)
        {
            this.Buffer = interopStruct.buffer;
            this.BufferByteCount = interopStruct.bufferByteCount;
        }

        public IntPtr Buffer { get; }
        public UInt32 BufferByteCount { get; }
    }
}