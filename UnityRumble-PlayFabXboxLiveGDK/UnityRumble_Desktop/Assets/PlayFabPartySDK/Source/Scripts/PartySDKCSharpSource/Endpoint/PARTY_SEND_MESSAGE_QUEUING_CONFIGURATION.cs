using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION
    {
        internal PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION(Interop.PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION interopStruct)
        {
            this.Priority = interopStruct.priority;
            this.IdentityForCancelFilters = interopStruct.identityForCancelFilters;
            this.TimeoutInMilliseconds = interopStruct.timeoutInMilliseconds;
        }

        public PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION()
        {
        }

        public SByte Priority { get; set; }
        public UInt32 IdentityForCancelFilters { get; set; }
        public UInt32 TimeoutInMilliseconds { get; set; }
    }
}