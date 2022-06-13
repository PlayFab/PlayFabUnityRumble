namespace PlayFab.Multiplayer
{
    using System;
    using System.Runtime.InteropServices;

    public class DisposableBuffer : IDisposable
    {
        public DisposableBuffer()
        {
            // Null buffer.
            this.IntPtr = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        public DisposableBuffer(int size)
        {
            this.IntPtr = Marshal.AllocHGlobal(size);
        }

        ~DisposableBuffer()
        {
            this.Dispose(isDisposing: false);
        }

        public IntPtr IntPtr { get; private set; }

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
    }
}
