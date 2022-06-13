using System;

namespace PartyCSharpSDK
{
    public class PARTY_CHAT_CONTROL_HANDLE
    {
        internal PARTY_CHAT_CONTROL_HANDLE(Interop.PARTY_CHAT_CONTROL_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_CHAT_CONTROL_HANDLE interopHandle, out PARTY_CHAT_CONTROL_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_CHAT_CONTROL_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_CHAT_CONTROL_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_CHAT_CONTROL_HANDLE();
        }

        internal Interop.PARTY_CHAT_CONTROL_HANDLE InteropHandle { get; set; }
    }
}
