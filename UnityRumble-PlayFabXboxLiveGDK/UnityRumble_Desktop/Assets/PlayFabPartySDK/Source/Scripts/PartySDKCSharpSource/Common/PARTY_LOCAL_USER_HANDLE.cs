using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyCSharpSDK
{
    public class PARTY_LOCAL_USER_HANDLE
    {
        internal PARTY_LOCAL_USER_HANDLE(Interop.PARTY_LOCAL_USER_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_LOCAL_USER_HANDLE interopHandle, out PARTY_LOCAL_USER_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_LOCAL_USER_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_LOCAL_USER_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_LOCAL_USER_HANDLE();
        }

        internal Interop.PARTY_LOCAL_USER_HANDLE InteropHandle { get; set; }
    }
}
