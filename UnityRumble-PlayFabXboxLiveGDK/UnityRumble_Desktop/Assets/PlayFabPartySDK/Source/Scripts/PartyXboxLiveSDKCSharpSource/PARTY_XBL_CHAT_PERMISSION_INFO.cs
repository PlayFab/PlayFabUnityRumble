using System;
using PartyXBLCSharpSDK.Interop;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_CHAT_PERMISSION_INFO
    {
        internal PARTY_XBL_CHAT_PERMISSION_INFO(Interop.PARTY_XBL_CHAT_PERMISSION_INFO interopStruct)
        {
            this.ChatPermissionMask = interopStruct.chatPermissionMask;
            this.Reason = interopStruct.reason;
        }

        public PARTY_CHAT_PERMISSION_OPTIONS ChatPermissionMask { get; }

        public PARTY_XBL_CHAT_PERMISSION_MASK_REASON Reason { get; }
    }
}