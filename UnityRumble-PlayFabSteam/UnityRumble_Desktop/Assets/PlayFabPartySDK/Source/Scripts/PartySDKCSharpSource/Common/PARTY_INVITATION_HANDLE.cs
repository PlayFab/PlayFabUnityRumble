using System;

namespace PartyCSharpSDK
{
    public class PARTY_INVITATION_HANDLE
    {
        internal PARTY_INVITATION_HANDLE(Interop.PARTY_INVITATION_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_INVITATION_HANDLE interopHandle, out PARTY_INVITATION_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_INVITATION_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_INVITATION_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_INVITATION_HANDLE();
        }

        internal Interop.PARTY_INVITATION_HANDLE InteropHandle { get; set; }
    }
}
