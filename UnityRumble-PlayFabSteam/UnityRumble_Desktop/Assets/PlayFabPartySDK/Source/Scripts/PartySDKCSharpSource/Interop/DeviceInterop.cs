using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    partial class PFPInterop
    {
        //PartyDeviceGetProperty(
        //    PARTY_DEVICE_HANDLE device,
        //    PartyString key,
        //    _Outptr_result_maybenull_ const PARTY_DATA_BUFFER** value
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyDeviceGetProperty(
            PARTY_DEVICE_HANDLE device,
            Byte[] key,
            out PARTY_DATA_BUFFER* value);


        //PartyDeviceGetChatControls(
        //    PARTY_DEVICE_HANDLE device,
        //    _Out_ uint32_t* chatControlCount,
        //    _Outptr_result_buffer_(*chatControlCount) const PARTY_CHAT_CONTROL_HANDLE** chatControls
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        unsafe internal static extern UInt32 PartyDeviceGetChatControls(
            PARTY_DEVICE_HANDLE device,
            out UInt32 chatControlCount,
            out IntPtr chatControls);

        //PartyDeviceCreateChatControl(
        //    PARTY_DEVICE_HANDLE device,
        //    PARTY_LOCAL_USER_HANDLE localUser,
        //    _In_opt_ PartyString languageCode,
        //    _In_opt_ void* asyncIdentifier,
        //    _Outptr_opt_ PARTY_CHAT_CONTROL_HANDLE* chatControl
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceCreateChatControl(
            PARTY_DEVICE_HANDLE device,
            PARTY_LOCAL_USER_HANDLE localUser,
            Byte[] languageCode,
            IntPtr asyncIdentifier,
            out PARTY_CHAT_CONTROL_HANDLE chatControl);

        //PartyDeviceIsLocal(
        //    PARTY_DEVICE_HANDLE device,
        //    _Out_ PartyBool* isLocal
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceIsLocal(
            PARTY_DEVICE_HANDLE device,
            out Byte isLocal);

        //PartyDeviceSetCustomContext(
        //    PARTY_DEVICE_HANDLE device,
        //    _In_opt_ void* customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceSetCustomContext(
            PARTY_DEVICE_HANDLE device,
            IntPtr customContext);

        //PartyDeviceSetProperties(
        //    PARTY_DEVICE_HANDLE device,
        //    uint32_t propertyCount,
        //    _In_reads_(propertyCount) const PartyString* keys,
        //    _In_reads_(propertyCount) const PARTY_DATA_BUFFER* values
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceSetProperties(
            PARTY_DEVICE_HANDLE device,
            UInt32 propertyCount,
            [In] UTF8StringPtr[] keys,
            [In] PARTY_DATA_BUFFER[] values);

        //PartyDeviceGetPropertyKeys(
        //    PARTY_DEVICE_HANDLE device,
        //    _Out_ uint32_t* propertyCount,
        //    _Outptr_result_buffer_(*propertyCount) const PartyString** keys
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceGetPropertyKeys(
            PARTY_DEVICE_HANDLE device,
            out UInt32 propertyCount,
            out UTF8StringPtr[] keys);

        //PartyDeviceGetCustomContext(
        //     PARTY_DEVICE_HANDLE device,
        //     _Outptr_result_maybenull_ void** customContext
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceGetCustomContext(
            PARTY_DEVICE_HANDLE device,
            out IntPtr customContext);

        //PartyDeviceDestroyChatControl(
        //    PARTY_DEVICE_HANDLE device,
        //    PARTY_CHAT_CONTROL_HANDLE chatControl,
        //    _In_opt_ void* asyncIdentifier
        //    );
        [DllImport(ThunkDllName, CallingConvention = CallingConvention.StdCall)]
        internal static extern UInt32 PartyDeviceDestroyChatControl(
            PARTY_DEVICE_HANDLE device,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            IntPtr asyncIdentifier);
    }
}
