using System;
using System.Runtime.InteropServices;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK.Interop
{
    //typedef struct PARTY_XBL_CHAT_PERMISSION_INFO
    //{
    //    PARTY_CHAT_PERMISSION_OPTIONS chatPermissionMask;
    //    PARTY_XBL_CHAT_PERMISSION_MASK_REASON reason;
    //} PARTY_XBL_CHAT_PERMISSION_INFO;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_CHAT_PERMISSION_INFO
    {
        internal readonly PARTY_CHAT_PERMISSION_OPTIONS chatPermissionMask;
        internal readonly PARTY_XBL_CHAT_PERMISSION_MASK_REASON reason;

        internal PARTY_XBL_CHAT_PERMISSION_INFO(PartyXBLCSharpSDK.PARTY_XBL_CHAT_PERMISSION_INFO publicObject)
        {
            this.chatPermissionMask = publicObject.ChatPermissionMask;
            this.reason = publicObject.Reason;
        }
    }
}