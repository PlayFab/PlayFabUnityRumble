using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_DATA_BUFFER
    //{
    //    _Field_size_bytes_(bufferByteCount) const void* buffer;
    //    uint32_t bufferByteCount;
    //} PARTY_DATA_BUFFER;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_DATA_BUFFER
    {
        internal readonly IntPtr buffer;
        internal readonly UInt32 bufferByteCount;

        internal PARTY_DATA_BUFFER(Byte[] publicObject, DisposableCollection disposableCollection)
        {
            this.bufferByteCount = checked((UInt32)publicObject.Length);
            if (bufferByteCount > 0)
            {
                this.buffer = disposableCollection.Add(new DisposableBuffer(publicObject.Length)).IntPtr;
                Marshal.Copy(publicObject, 0, this.buffer, publicObject.Length);
            }
            else
            {
                this.buffer = IntPtr.Zero;
            }
        }

        internal PARTY_DATA_BUFFER(IntPtr bufferPtr, UInt32 bufferSize)
        {
            this.buffer = bufferPtr;
            this.bufferByteCount = bufferSize;
        }
    }
}