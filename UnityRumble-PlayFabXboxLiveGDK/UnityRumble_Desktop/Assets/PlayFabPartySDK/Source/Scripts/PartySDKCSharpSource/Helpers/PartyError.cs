using System;

namespace PartyCSharpSDK
{
    public class PartyError
    {
        public const UInt32 Success = 0x00000000;
        // BumblelionError.h: c_bumblelionErrorInvalidArg
        public const UInt32 InvalidArg = 0x0004;
        // BumblelionError.h: c_bumblelionErrorUnsupportedPartyOption
        public const UInt32 UnsupportedPartyOption = 0x10D1;

        public static bool SUCCEEDED(UInt32 error)
        {
            return error == Success;
        }

        public static bool FAILED(UInt32 error)
        {
            return error != Success;
        }
    }
}
