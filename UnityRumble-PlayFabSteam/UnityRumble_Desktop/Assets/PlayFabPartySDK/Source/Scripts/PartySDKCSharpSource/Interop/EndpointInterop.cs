using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    partial class PFPInterop
    {
        //PartyEndpointGetUniqueIdentifier(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Out_ uint16_t* uniqueIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetUniqueIdentifier(
            PARTY_ENDPOINT_HANDLE endpoint,
            out UInt16 uniqueIdentifier);

        //PartyEndpointSetCustomContext(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointSetCustomContext(
            PARTY_ENDPOINT_HANDLE endpoint,
            IntPtr customContext);

        //PartyEndpointCancelMessages(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    uint32_t targetEndpointCount,
        //    _In_reads_(targetEndpointCount) const PARTY_ENDPOINT_HANDLE* targetEndpoints,
        //    PARTY_CANCEL_MESSAGES_FILTER_EXPRESSION filterExpression,
        //    uint32_t messageIdentityFilterMask,
        //    uint32_t filteredMessageIdentitiesToMatch,
        //    _Out_opt_ uint32_t* canceledMessagesCount
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointCancelMessages(
            PARTY_ENDPOINT_HANDLE endpoint,
            UInt32 targetEndpointCount,
            IntPtr targetEndpoints,
            PARTY_CANCEL_MESSAGES_FILTER_EXPRESSION filterExpression,
            UInt32 messageIdentityFilterMask,
            UInt32 filteredMessageIdentitiesToMatch,
            out UInt32 canceledMessagesCount);

        //PartyEndpointGetDevice(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Outptr_ PARTY_DEVICE_HANDLE* device
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetDevice(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_DEVICE_HANDLE device);

        //PartyEndpointIsLocal(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Out_ PartyBool* isLocal
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointIsLocal(
            PARTY_ENDPOINT_HANDLE endpoint,
            out Byte isLocal);

        //PartyEndpointSendMessage(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    uint32_t targetEndpointCount,
        //    _In_reads_(targetEndpointCount) const PARTY_ENDPOINT_HANDLE* targetEndpoints,
        //    PARTY_SEND_MESSAGE_OPTIONS options,
        //    _In_opt_ const PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION* queuingConfiguration,
        //    uint32_t dataBufferCount,
        //    _In_reads_(dataBufferCount) const PARTY_DATA_BUFFER* dataBuffers,
        //    _In_opt_ void* messageIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyEndpointSendMessage(
            PARTY_ENDPOINT_HANDLE endpoint,
            UInt32 targetEndpointCount,
            IntPtr targetEndpoints,
            PARTY_SEND_MESSAGE_OPTIONS options,
            PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION* queuingConfiguration,
            UInt32 dataBufferCount,
            PARTY_DATA_BUFFER* dataBuffers,
            IntPtr messageIdentifier);

        //PartyEndpointGetCustomContext(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetCustomContext(
            PARTY_ENDPOINT_HANDLE endpoint,
            out IntPtr customContext);

        //PartyEndpointGetPropertyKeys(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Out_ uint32_t* propertyCount,
        //    _Outptr_result_buffer_(*propertyCount) const PartyString** keys
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyEndpointGetPropertyKeys(
            PARTY_ENDPOINT_HANDLE endpoint,
            out UInt32 propertyCount,
            out UTF8StringPtr* keys);

        //PartyEndpointGetEntityId(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Outptr_result_maybenull_ PartyString* entityId
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetEntityId(
            PARTY_ENDPOINT_HANDLE endpoint,
            out UTF8StringPtr entityId);

        //PartyEndpointGetEndpointStatistics(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    uint32_t targetEndpointCount,
        //    _In_reads_(targetEndpointCount) const PARTY_ENDPOINT_HANDLE* targetEndpoints,
        //    uint32_t statisticCount,
        //    _In_reads_(statisticCount) const PARTY_ENDPOINT_STATISTIC* statisticTypes,
        //    _Out_writes_all_(statisticCount) uint64_t* statisticValues
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetEndpointStatistics(
            PARTY_ENDPOINT_HANDLE endpoint,
            UInt32 targetEndpointCount,
            IntPtr targetEndpoints,
            UInt32 statisticCount,
            PARTY_ENDPOINT_STATISTIC[] statisticTypes,
            UInt64[] statisticValues);

        //PartyEndpointGetProperty(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    PartyString key,
        //    _Outptr_result_maybenull_ const PARTY_DATA_BUFFER** value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyEndpointGetProperty(
            PARTY_ENDPOINT_HANDLE endpoint,
            Byte[] key,
            out PARTY_DATA_BUFFER* value);

        //PartyEndpointGetNetwork(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Outptr_ PARTY_NETWORK_HANDLE* network
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetNetwork(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_NETWORK_HANDLE network);

        //PartyEndpointFlushMessages(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    uint32_t targetEndpointCount,
        //    _In_reads_(targetEndpointCount) const PARTY_ENDPOINT_HANDLE* targetEndpoints
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointFlushMessages(
            PARTY_ENDPOINT_HANDLE endpoint,
            UInt32 targetEndpointCount,
            IntPtr targetEndpoints);

        //PartyEndpointGetLocalUser(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    _Outptr_result_maybenull_ PARTY_LOCAL_USER_HANDLE* localUser
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointGetLocalUser(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_LOCAL_USER_HANDLE localUser);

        //PartyEndpointSetProperties(
        //    PARTY_ENDPOINT_HANDLE endpoint,
        //    uint32_t propertyCount,
        //    _In_reads_(propertyCount) const PartyString* keys,
        //    _In_reads_(propertyCount) const PARTY_DATA_BUFFER* values
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyEndpointSetProperties(
            PARTY_ENDPOINT_HANDLE endpoint,
            UInt32 propertyCount,
            [In] UTF8StringPtr[] keys,
            [In] PARTY_DATA_BUFFER[] values);
    }
}
