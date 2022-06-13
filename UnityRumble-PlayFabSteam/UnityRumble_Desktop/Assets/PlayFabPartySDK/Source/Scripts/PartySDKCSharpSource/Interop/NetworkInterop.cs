using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    partial class PFPInterop
    {
        //PartyNetworkSetProperties(
        //    PARTY_NETWORK_HANDLE network,
        //    uint32_t propertyCount,
        //    _In_reads_(propertyCount) const PartyString* keys,
        //    _In_reads_(propertyCount) const PARTY_DATA_BUFFER* values
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkSetProperties(
            PARTY_NETWORK_HANDLE network,
            UInt32 propertyCount,
            [In] UTF8StringPtr[] keys,
            [In] PARTY_DATA_BUFFER[] values);

        //PartyNetworkLeaveNetwork(
        //    PARTY_NETWORK_HANDLE network,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkLeaveNetwork(
            PARTY_NETWORK_HANDLE network,
            IntPtr asyncIdentifier);

        //PartyNetworkCreateInvitation(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _In_opt_ const PARTY_INVITATION_CONFIGURATION* invitationConfiguration,
        //    void* asyncIdentifier,
        //    _Outptr_opt_ PARTY_INVITATION_HANDLE* invitation
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkCreateInvitation(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_INVITATION_CONFIGURATION* invitationConfiguration,
            IntPtr asyncIdentifier,
            out PARTY_INVITATION_HANDLE invitation);

        //PartyNetworkGetNetworkConfiguration(
        //    PARTY_NETWORK_HANDLE network,
        //    _Outptr_ const PARTY_NETWORK_CONFIGURATION** networkConfiguration
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetNetworkConfiguration(
            PARTY_NETWORK_HANDLE network,
            out IntPtr networkConfiguration);

        //PartySerializeNetworkDescriptor(
        //    const PARTY_NETWORK_DESCRIPTOR* networkDescriptor,
        //    _Out_writes_z_(PARTY_MAX_SERIALIZED_NETWORK_DESCRIPTOR_STRING_LENGTH + 1) char* serializedNetworkDescriptorString
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartySerializeNetworkDescriptor(
            PARTY_NETWORK_DESCRIPTOR* networkDescriptor,
            IntPtr serializedNetworkDescriptorString);

        //PartyDeserializeNetworkDescriptor(
        //    PartyString serializedNetworkDescriptorString,
        //    _Out_ PARTY_NETWORK_DESCRIPTOR* networkDescriptor
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeserializeNetworkDescriptor(
            Byte[] serializedNetworkDescriptorString,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor);

        //PartyNetworkDestroyEndpoint(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_ENDPOINT_HANDLE localEndpoint,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkDestroyEndpoint(
            PARTY_NETWORK_HANDLE network,
            PARTY_ENDPOINT_HANDLE localEndpoint,
            IntPtr asyncIdentifier);

        //PartyNetworkDisconnectChatControl(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkDisconnectChatControl(
            PARTY_NETWORK_HANDLE network,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            IntPtr asyncIdentifier);

        //PartyNetworkGetCustomContext(
        //    PARTY_NETWORK_HANDLE network,
        //    _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkGetCustomContext(
            PARTY_NETWORK_HANDLE network,
            out IntPtr customContext);

        //PartyNetworkGetDevices(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* deviceCount,
        //    _Outptr_result_buffer_(*deviceCount) const PARTY_DEVICE_HANDLE** devices
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetDevices(
            PARTY_NETWORK_HANDLE network,
            out UInt32 deviceCount,
            out IntPtr devices);

        //PartyNetworkSetCustomContext(
        //    PARTY_NETWORK_HANDLE network,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkSetCustomContext(
            PARTY_NETWORK_HANDLE network,
            IntPtr customContext);

        //PartyNetworkKickUser(
        //    PARTY_NETWORK_HANDLE network,
        //    PartyString targetEntityId,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkKickUser(
            PARTY_NETWORK_HANDLE network,
            Byte[] targetEntityId,
            IntPtr asyncIdentifier);

        //PartyNetworkGetPropertyKeys(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* propertyCount,
        //    _Outptr_result_buffer_(*propertyCount) const PartyString** keys
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetPropertyKeys(
            PARTY_NETWORK_HANDLE network,
            out UInt32 propertyCount,
            out UTF8StringPtr* keys);

        //PartyNetworkConnectChatControl(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkConnectChatControl(
            PARTY_NETWORK_HANDLE network,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            IntPtr asyncIdentifier);

        //PartyNetworkRevokeInvitation(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    PARTY_INVITATION_HANDLE invitation,
        //    void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkRevokeInvitation(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_INVITATION_HANDLE invitation,
            IntPtr asyncIdentifier);

        //PartyNetworkGetChatControls(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* chatControlCount,
        //    _Outptr_result_buffer_(*chatControlCount) const PARTY_CHAT_CONTROL_HANDLE** chatControls
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetChatControls(
            PARTY_NETWORK_HANDLE network,
            out UInt32 chatControlCount,
            out IntPtr chatControls);

        //PartyNetworkFindEndpointByUniqueIdentifier(
        //    PARTY_NETWORK_HANDLE network,
        //    uint16_t uniqueIdentifier,
        //    _Outptr_ PARTY_ENDPOINT_HANDLE* endpoint
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkFindEndpointByUniqueIdentifier(
            PARTY_NETWORK_HANDLE network,
            UInt16 uniqueIdentifier,
            out PARTY_ENDPOINT_HANDLE endpoint);

        //PartyNetworkGetLocalUsers(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* userCount,
        //    _Outptr_result_buffer_(*userCount) const PARTY_LOCAL_USER_HANDLE** users
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetLocalUsers(
            PARTY_NETWORK_HANDLE network,
            out UInt32 userCount,
            out IntPtr users);

        //PartyNetworkCreateEndpoint(
        //    PARTY_NETWORK_HANDLE network,
        //    _In_opt_ PARTY_LOCAL_USER_HANDLE localUser,
        //    uint32_t propertyCount,
        //    _In_reads_opt_(propertyCount) const PartyString* keys,
        //    _In_reads_opt_(propertyCount) const PARTY_DATA_BUFFER* values,
        //    _In_opt_ void* asyncIdentifier,
        //    _Outptr_opt_ PARTY_ENDPOINT_HANDLE* endpoint
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkCreateEndpoint(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            UInt32 propertyCount,
            IntPtr keys,
            IntPtr values,
            IntPtr asyncIdentifier,
            out PARTY_ENDPOINT_HANDLE endpoint);

        //PartyNetworkGetInvitations(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* invitationCount,
        //    _Outptr_result_buffer_(*invitationCount) const PARTY_INVITATION_HANDLE** invitations
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetInvitations(
            PARTY_NETWORK_HANDLE network,
            out UInt32 invitationCount,
            out IntPtr invitations);

        //PartyNetworkGetEndpoints(
        //    PARTY_NETWORK_HANDLE network,
        //    _Out_ uint32_t* endpointCount,
        //    _Outptr_result_buffer_(*endpointCount) const PARTY_ENDPOINT_HANDLE** endpoints
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetEndpoints(
            PARTY_NETWORK_HANDLE network,
            out UInt32 endpointCount,
            out IntPtr endpoints);

        //PartyNetworkGetProperty(
        //    PARTY_NETWORK_HANDLE network,
        //    PartyString key,
        //    _Outptr_result_maybenull_ const PARTY_DATA_BUFFER** value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyNetworkGetProperty(
            PARTY_NETWORK_HANDLE network,
            Byte[] key,
            out PARTY_DATA_BUFFER* value);

        //PartyNetworkKickDevice(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_DEVICE_HANDLE targetDevice,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkKickDevice(
            PARTY_NETWORK_HANDLE network,
            PARTY_DEVICE_HANDLE targetDevice,
            IntPtr asyncIdentifier);

        //PartyNetworkGetNetworkStatistics(
        //    PARTY_NETWORK_HANDLE network,
        //    uint32_t statisticCount,
        //    _In_reads_(statisticCount) const PARTY_NETWORK_STATISTIC* statisticTypes,
        //    _Out_writes_all_(statisticCount) uint64_t* statisticValues
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkGetNetworkStatistics(
            PARTY_NETWORK_HANDLE network,
            UInt32 statisticCount,
            PARTY_NETWORK_STATISTIC[] statisticTypes,
            UInt64[] statisticValues);

        //PartyNetworkGetNetworkDescriptor(
        //     PARTY_NETWORK_HANDLE network,
        //     _Out_ PARTY_NETWORK_DESCRIPTOR* networkDescriptor
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkGetNetworkDescriptor(
            PARTY_NETWORK_HANDLE network,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor);

        //PartyNetworkRemoveLocalUser(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkRemoveLocalUser(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            IntPtr asyncIdentifier);

        //PartyNetworkAuthenticateLocalUser(
        //    PARTY_NETWORK_HANDLE network,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    PartyString invitationIdentifier,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyNetworkAuthenticateLocalUser(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            Byte[] invitationIdentifier,
            IntPtr asyncIdentifier);
    }
}
