using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK
{
    public class DisposableBuffer : IDisposable
    {
        public DisposableBuffer()
        {
            // Null buffer.
            this.IntPtr = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        public DisposableBuffer(Int32 size)
        {
            this.IntPtr = Marshal.AllocHGlobal(size);
        }

        public void Dispose()
        {
            this.Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (this.IntPtr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.IntPtr);
                this.IntPtr = IntPtr.Zero;
            }
        }

        ~DisposableBuffer()
        {
            this.Dispose(isDisposing: false);
        }

        public IntPtr IntPtr { get; private set; }
    }
}
