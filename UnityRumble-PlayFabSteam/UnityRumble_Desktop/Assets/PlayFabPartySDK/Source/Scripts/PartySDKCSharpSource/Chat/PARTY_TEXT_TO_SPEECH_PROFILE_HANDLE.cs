using System;

namespace PartyCSharpSDK
{
    public class PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE
    {
        internal PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE(Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE interopHandle)
        {
            this.InteropHandle = interopHandle;
        }

        internal static UInt32 WrapAndReturnError(UInt32 error, Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE interopHandle, out PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE handle)
        {
            if (PartyError.SUCCEEDED(error))
            {
                handle = new PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE(interopHandle);
            }
            else
            {
                handle = default(PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE);
            }
            return error;
        }

        internal void ClearInteropHandle()
        {
            this.InteropHandle = new Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE();
        }

        internal Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE InteropHandle { get; set; }
    }
}
