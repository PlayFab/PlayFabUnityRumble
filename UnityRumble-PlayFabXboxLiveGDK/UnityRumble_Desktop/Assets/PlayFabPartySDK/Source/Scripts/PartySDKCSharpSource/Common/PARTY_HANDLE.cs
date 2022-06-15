using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyCSharpSDK
{
    public class PARTY_HANDLE
    {
        public Int64 GetHandleValue()
        {
            return InteropHandle.handle.ToInt64();
        }

        public PARTY_HANDLE(Int64 handleValue)
        {
            this.InteropHandle = new Interop.PARTY_HANDLE(handleValue);
        }

        internal PARTY_HANDLE(Interop.PARTY_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_HANDLE interopHandle, out PARTY_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_HANDLE();
        }

        internal Interop.PARTY_HANDLE InteropHandle { get; set; }
    }
}
