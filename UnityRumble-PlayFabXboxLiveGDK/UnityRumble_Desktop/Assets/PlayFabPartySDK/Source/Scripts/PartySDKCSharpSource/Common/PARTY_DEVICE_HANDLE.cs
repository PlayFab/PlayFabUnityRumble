using System;

namespace PartyCSharpSDK
{
    public class PARTY_DEVICE_HANDLE
    {
        internal PARTY_DEVICE_HANDLE(Interop.PARTY_DEVICE_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_DEVICE_HANDLE interopHandle, out PARTY_DEVICE_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_DEVICE_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_DEVICE_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_DEVICE_HANDLE();
        }

        internal Interop.PARTY_DEVICE_HANDLE InteropHandle { get; set; }
    }
}
