using System;
using System.Runtime.InteropServices;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK.Interop
{
    //typedef struct PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING
    //{
    //    uint64_t xboxLiveUserId;
    //    PartyString playfabEntityId;
    //}
    //PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING
    {
        internal readonly UInt64 xboxLiveUserId;
        internal readonly IntPtr playfabEntityId;
    }
}