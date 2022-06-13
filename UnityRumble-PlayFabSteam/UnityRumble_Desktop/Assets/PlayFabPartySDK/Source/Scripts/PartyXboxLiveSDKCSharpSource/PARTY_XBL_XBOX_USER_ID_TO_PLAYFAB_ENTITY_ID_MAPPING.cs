using System;
using PartyXBLCSharpSDK.Interop;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING
    {
        internal PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING(Interop.PARTY_XBL_XBOX_USER_ID_TO_PLAYFAB_ENTITY_ID_MAPPING interopStruct)
        {
            this.xboxLiveUserId = interopStruct.xboxLiveUserId;
            this.playfabEntityId = Converters.PtrToStringUTF8(interopStruct.playfabEntityId);
        }

        public UInt64 xboxLiveUserId { get; }
        public string playfabEntityId { get; }
    }
}