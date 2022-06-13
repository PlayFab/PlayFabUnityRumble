using System;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_HANDLE
    {
        public Int64 GetHandleValue()
        {
            return InteropHandle.handle.ToInt64();
        }

        public PARTY_XBL_HANDLE(Int64 handleValue)
        {
            this.InteropHandle = new Interop.PARTY_XBL_HANDLE(handleValue);
        }

        internal PARTY_XBL_HANDLE(Interop.PARTY_XBL_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_XBL_HANDLE interopHandle, out PARTY_XBL_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_XBL_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_XBL_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_XBL_HANDLE();
        }

        internal Interop.PARTY_XBL_HANDLE InteropHandle { get; set; }
    }
}
