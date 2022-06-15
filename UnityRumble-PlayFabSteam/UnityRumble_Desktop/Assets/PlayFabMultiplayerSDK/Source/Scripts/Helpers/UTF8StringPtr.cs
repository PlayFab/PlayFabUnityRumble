namespace PlayFab.Multiplayer.InteropWrapper
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Represents a null-terminated UTF-8 char* whose _pointer value_ is marshalled between managed and unmanaged code.
    /// </summary>>
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct UTF8StringPtr
    {
        private IntPtr pointer;

        /// <summary>
        /// Constructor for marshaling a UTF8 char* _to_ managed code.  Requires an existing DisposableCollection to add a buffer to.
        /// </summary>
        public UTF8StringPtr(string str, DisposableCollection disposableCollection)
        {
            if (str == null)
            {
                this.pointer = IntPtr.Zero;
            }
            else
            {
                byte[] utf8Bytes = StringToNullTerminatedUTF8ByteArray(str);
                DisposableBuffer buffer = new DisposableBuffer(utf8Bytes.Length);
                Marshal.Copy(source: utf8Bytes, startIndex: 0, destination: buffer.IntPtr, length: utf8Bytes.Length);
                disposableCollection.Add(buffer);
                this.pointer = buffer.IntPtr;
            }
        }

        public sbyte* Pointer
        {
            get
            {
                return (sbyte*)this.pointer.ToPointer();
            }
        }

        public string GetString()
        {
            if (this.pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return Converters.PtrToStringUTF8(this.pointer);
            }
        }

        internal static byte[] StringToNullTerminatedUTF8ByteArray(string str) 
        { 
            return StringToNullTerminatedUTF8ByteArrayInternal(str, requiredByteArrayLength: -1); 
        }

        private static byte[] StringToNullTerminatedUTF8ByteArrayInternal(string str, int requiredByteArrayLength)
        {
            if (str == null)
            {
                return null;
            }
            else if (requiredByteArrayLength == -1)
            {
                return System.Text.Encoding.UTF8.GetBytes(str + '\0');
            }
            else
            {
                byte[] result = new byte[requiredByteArrayLength];
                System.Text.Encoding.UTF8.GetBytes(str + '\0', 0, str.Length + 1, result, 0);
                return result;
            }
        }
    }
}
