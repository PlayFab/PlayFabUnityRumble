using System;

namespace PartyCSharpSDK
{
    public class PARTY_ENDPOINT_HANDLE
    {
        internal PARTY_ENDPOINT_HANDLE(Interop.PARTY_ENDPOINT_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_ENDPOINT_HANDLE interopHandle, out PARTY_ENDPOINT_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_ENDPOINT_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_ENDPOINT_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_ENDPOINT_HANDLE();
        }

        internal Interop.PARTY_ENDPOINT_HANDLE InteropHandle { get; set; }
    }
}
