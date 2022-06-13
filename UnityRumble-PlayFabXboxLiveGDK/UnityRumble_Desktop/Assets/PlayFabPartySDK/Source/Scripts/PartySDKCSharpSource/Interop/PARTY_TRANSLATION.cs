using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_TRANSLATION
    //{
    //    PARTY_STATE_CHANGE_RESULT result;
    //    PartyError errorDetail;
    //    PartyString languageCode;
    //    PARTY_TRANSLATION_RECEIVED_OPTIONS options;
    //    PartyString translation;
    //}
    //PARTY_TRANSLATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_TRANSLATION
    {
        internal readonly PARTY_STATE_CHANGE_RESULT result;
        internal readonly UInt32 errorDetail;
        internal readonly IntPtr languageCode;
        internal readonly PARTY_TRANSLATION_RECEIVED_OPTIONS options;
        internal readonly IntPtr translation;
    }
}