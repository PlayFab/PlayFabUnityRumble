namespace PlayFab.Multiplayer
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SizeT
    {
        private readonly UIntPtr value;

        public SizeT(int length)
        {
            this.value = new UIntPtr(Convert.ToUInt64(length));
        }

        public SizeT(uint length)
        {
            this.value = new UIntPtr(Convert.ToUInt64(length));
        }

        public bool IsZero 
        { 
            get 
            { 
                return this.value == UIntPtr.Zero; 
            } 
        }

        public uint ToUInt32()
        {
            return Convert.ToUInt32(this.value.ToUInt64());
        }

        public int ToInt32()
        {
            return Convert.ToInt32(this.value.ToUInt64());
        }
    }
}
