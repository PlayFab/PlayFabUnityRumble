using System;

namespace PartyCSharpSDK
{
    public class PARTY_NETWORK_HANDLE
    {
        internal PARTY_NETWORK_HANDLE(Interop.PARTY_NETWORK_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_NETWORK_HANDLE interopHandle, out PARTY_NETWORK_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_NETWORK_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_NETWORK_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_NETWORK_HANDLE();
        }

        internal Interop.PARTY_NETWORK_HANDLE InteropHandle { get; set; }
    }
}
