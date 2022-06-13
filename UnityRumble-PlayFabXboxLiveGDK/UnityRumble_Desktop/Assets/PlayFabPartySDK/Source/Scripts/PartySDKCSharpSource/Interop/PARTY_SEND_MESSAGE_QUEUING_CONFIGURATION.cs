using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION
    //{
    //    _Field_range_(PARTY_MIN_SEND_MESSAGE_QUEUING_PRIORITY, PARTY_MAX_SEND_MESSAGE_QUEUING_PRIORITY) int8_t priority;
    //    uint32_t identityForCancelFilters;
    //    uint32_t timeoutInMilliseconds;
    //} PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION
    {
        internal readonly SByte priority;
        internal readonly UInt32 identityForCancelFilters;
        internal readonly UInt32 timeoutInMilliseconds;

        internal PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION(PartyCSharpSDK.PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION publicObject)
        {
            this.priority = publicObject.Priority;
            this.identityForCancelFilters = publicObject.IdentityForCancelFilters;
            this.timeoutInMilliseconds = publicObject.TimeoutInMilliseconds;
        }
    }
}