using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PartyCSharpSDK.Interop;
using System.Linq;
using System.Reflection;

namespace PartyCSharpSDK
{
    public partial class SDK
    {
        // For storing high frequency objects
        internal static ObjectPool objectPool;

        static SDK()
        {
            objectPool = new ObjectPool();
            // Limit is arbitrary
            objectPool.AddEntry<List<PARTY_STATE_CHANGE>>(4, new Type[] { });
            // PARTY_STATE_CHANGE_TYPE_ENDPOINT_MESSAGE_RECEIVED
            objectPool.AddEntry<List<PARTY_ENDPOINT_HANDLE>>(32, new Type[] { });
            objectPool.AddEntry<PARTY_ENDPOINT_MESSAGE_RECEIVED_STATE_CHANGE>(32, new Type[] { typeof(PARTY_STATE_CHANGE_UNION), typeof(IntPtr) });
            objectPool.AddEntry<PARTY_NETWORK_HANDLE>(32, new Type[] { typeof(Interop.PARTY_NETWORK_HANDLE) });
            objectPool.AddEntry<PARTY_ENDPOINT_HANDLE>(64, new Type[] { typeof(Interop.PARTY_ENDPOINT_HANDLE) });
            // PARTY_STATE_CHANGE_TYPE_VOICE_CHAT_TRANSCRIPTION_RECEIVED
            objectPool.AddEntry<PARTY_VOICE_CHAT_TRANSCRIPTION_RECEIVED_STATE_CHANGE>(32, new Type[] { typeof(PARTY_STATE_CHANGE_UNION), typeof(IntPtr) });
            objectPool.AddEntry<List<PARTY_CHAT_CONTROL_HANDLE>>(32, new Type[] { });
            objectPool.AddEntry<List<PARTY_TRANSLATION>>(32, new Type[] { });
            objectPool.AddEntry<PARTY_CHAT_CONTROL_HANDLE>(64, new Type[] { typeof(Interop.PARTY_CHAT_CONTROL_HANDLE) });
            objectPool.AddEntry<PARTY_TRANSLATION>(64, new Type[] { typeof(Interop.PARTY_TRANSLATION) });
        }

        // PartyManager
        public static UInt32 PartyInitialize(
            string titleId,
            out PARTY_HANDLE handle)
        {
            Interop.PARTY_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyInitialize(Converters.StringToNullTerminatedUTF8ByteArray(titleId), out interopHandle);
            return PARTY_HANDLE.WrapAndReturnError(err, interopHandle, out handle);
        }

        public static UInt32 PartyCleanup(
            PARTY_HANDLE handle)
        {
            return PFPInterop.PartyCleanup(handle.InteropHandle);
        }

        public static UInt32 PartyCreateLocalUser(
            PARTY_HANDLE handle,
            string entityId,
            string titlePlayerEntityToken,
            out PARTY_LOCAL_USER_HANDLE localUser)
        {
            localUser = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_LOCAL_USER_HANDLE interopUserHandle;
            UInt32 err = PFPInterop.PartyCreateLocalUser(
                handle.InteropHandle,
                Converters.StringToNullTerminatedUTF8ByteArray(entityId),
                Converters.StringToNullTerminatedUTF8ByteArray(titlePlayerEntityToken),
                out interopUserHandle);

            return PARTY_LOCAL_USER_HANDLE.WrapAndReturnError(err, interopUserHandle, out localUser);
        }

        public static UInt32 PartyCreateNewNetwork(
            PARTY_HANDLE handle,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_NETWORK_CONFIGURATION networkConfiguration,
            PARTY_REGION[] regions,
            PARTY_INVITATION_CONFIGURATION initialInvitationConfiguration,
            Object asyncIdentifier,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor,
            out string appliedInitialInvitationIdentifier)
        {
            UInt32 err;
            networkDescriptor = null;
            appliedInitialInvitationIdentifier = null;
            if (handle == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                var networkConfig = new Interop.PARTY_NETWORK_CONFIGURATION(networkConfiguration);
                SizeT arrayCount;
                var regionArray = Converters.ClassArrayToPtr<PARTY_REGION, Interop.PARTY_REGION>(regions, (x, xc) => new Interop.PARTY_REGION(x), dc, out arrayCount);
                var inviteConfig = new Interop.PARTY_INVITATION_CONFIGURATION(initialInvitationConfiguration, dc);
                var asyncId = IntPtr.Zero;
                if (asyncIdentifier != null)
                {
                    asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                }

                var invitationIdentifier = dc.Add(new DisposableBuffer(checked(PartyConstants.c_maxInvitationIdentifierStringLength + 1))).IntPtr;

                Interop.PARTY_NETWORK_DESCRIPTOR interopNetworkDescriptor;
                unsafe
                {
                    err = PFPInterop.PartyCreateNewNetwork(
                        handle.InteropHandle,
                        localUser.InteropHandle,
                        &networkConfig,
                        arrayCount.ToUInt32(),
                        regionArray,
                        &inviteConfig,
                        asyncId,
                        out interopNetworkDescriptor,
                        invitationIdentifier);
                }

                if (PartyError.SUCCEEDED(err))
                {
                    networkDescriptor = new PARTY_NETWORK_DESCRIPTOR(interopNetworkDescriptor);
                    appliedInitialInvitationIdentifier = Converters.PtrToStringUTF8(invitationIdentifier);
                }
                else
                {
                    if (asyncId != IntPtr.Zero)
                    {
                        GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                        asyncGcHandle.Free();
                    }
                }
            }

            return err;
        }

        public static UInt32 PartyConnectToNetwork(
             PARTY_HANDLE handle,
             PARTY_NETWORK_DESCRIPTOR networkDescriptor,
             Object asyncIdentifier,
             out PARTY_NETWORK_HANDLE network)
        {
            network = null;
            if (handle == null || networkDescriptor == null)
            {
                return PartyError.InvalidArg;
            }

            var descriptorInterop = new Interop.PARTY_NETWORK_DESCRIPTOR(networkDescriptor);
            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            Interop.PARTY_NETWORK_HANDLE interopHandle;
            UInt32 err;
            unsafe
            {
                err = PFPInterop.PartyConnectToNetwork(
                    handle.InteropHandle,
                    &descriptorInterop,
                    asyncId,
                    out interopHandle);
            }
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return PARTY_NETWORK_HANDLE.WrapAndReturnError(err, interopHandle, out network);
        }

        public static unsafe UInt32 PartyStartProcessingStateChanges(
            PARTY_HANDLE handle,
            out List<PARTY_STATE_CHANGE> stateChanges)
        {
            UInt32 err;
            stateChanges = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            UInt32 stateChangeCount;
            IntPtr rawStateChanges;
            stateChanges = objectPool.Retrieve<List<PARTY_STATE_CHANGE>>();
            err = PFPInterop.PartyStartProcessingStateChanges(handle.InteropHandle, out stateChangeCount, out rawStateChanges);
            if (PartyError.SUCCEEDED(err) && stateChangeCount > 0)
            {
                List<PARTY_STATE_CHANGE> unsupportStateChanges = null;
                IntPtr* arrayPtr = (IntPtr*)rawStateChanges.ToPointer();
                for (Int32 i = 0; i < stateChangeCount; i++)
                {
                    PARTY_STATE_CHANGE stateChangeObj = PARTY_STATE_CHANGE.CreateFromPtr(arrayPtr[i]);
                    if (stateChangeObj.GetType() != typeof(PARTY_STATE_CHANGE))
                    {
                        stateChanges.Add(stateChangeObj);
                    }
                    else
                    {
                        // Remove and immediately finish processing state changes that aren't supported by the
                        // CSharp wrapper, which will all be of the basic PARTY_STATE_CHANGE type
                        if (unsupportStateChanges == null)
                        {
                            unsupportStateChanges = objectPool.Retrieve<List<PARTY_STATE_CHANGE>>();
                        }

                        unsupportStateChanges.Add(stateChangeObj);
                    }
                }

                if (unsupportStateChanges != null)
                {
                    err = PartyFinishProcessingStateChanges(handle, unsupportStateChanges);
                }
            }

            return err;
        }

        public static unsafe UInt32 PartyFinishProcessingStateChanges(
            PARTY_HANDLE handle,
            List<PARTY_STATE_CHANGE> stateChanges)
        {
            UInt32 err;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            UInt32 stateChangeCount = (UInt32)stateChanges.Count;
            IntPtr* stateChangeIds = stackalloc IntPtr[stateChanges.Count];
            for (Int32 i = 0; i < stateChanges.Count; i++)
            {
                stateChangeIds[i] = stateChanges[i].StateChangeId;
                stateChanges[i].Cleanup();
            }

            stateChanges.Clear();
            objectPool.Return(stateChanges);

            err = PFPInterop.PartyFinishProcessingStateChanges(
                handle.InteropHandle,
                stateChangeCount,
                new IntPtr(stateChangeIds));

            return err;
        }

        public static UInt32 PartyDestroyLocalUser(
            PARTY_HANDLE handle,
            PARTY_LOCAL_USER_HANDLE localUser,
            Object asyncIdentifier)
        {
            if (handle == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyDestroyLocalUser(
                handle.InteropHandle,
                localUser.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyGetErrorMessage(
            UInt32 error,
            out string errorMessage)
        {
            UTF8StringPtr errorMessagePtr;
            UInt32 err = PFPInterop.PartyGetErrorMessage(
                error,
                out errorMessagePtr);
            if (PartyError.SUCCEEDED(err))
            {
                errorMessage = errorMessagePtr.GetString();
            }
            else
            {
                errorMessage = null;
            }

            return err;
        }

        public static UInt32 PartyGetRegions(
            PARTY_HANDLE handle,
            out PARTY_REGION[] regionList)
        {
            regionList = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_HANDLE, Interop.PARTY_REGION, PARTY_REGION>(
                PFPInterop.PartyGetRegions,
                s => new PARTY_REGION(s),
                handle.InteropHandle,
                out regionList);
        }

        public static UInt32 PartyGetLocalDevice(
            PARTY_HANDLE handle,
            out PARTY_DEVICE_HANDLE localDevice)
        {
            localDevice = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_DEVICE_HANDLE localDeviceInterop;
            UInt32 err = PFPInterop.PartyGetLocalDevice(handle.InteropHandle, out localDeviceInterop);
            return PARTY_DEVICE_HANDLE.WrapAndReturnError(err, localDeviceInterop, out localDevice);
        }

        public static UInt32 PartyGetLocalUsers(
            PARTY_HANDLE handle,
            out PARTY_LOCAL_USER_HANDLE[] users)
        {
            users = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_HANDLE, Interop.PARTY_LOCAL_USER_HANDLE, PARTY_LOCAL_USER_HANDLE>(
                PFPInterop.PartyGetLocalUsers,
                s => new PARTY_LOCAL_USER_HANDLE(s),
                handle.InteropHandle,
                out users);
        }

        public static UInt32 PartyLocalUserGetEntityId(
            PARTY_LOCAL_USER_HANDLE localUser,
            out string entityId)
        {
            entityId = null;
            if (localUser == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr entityIdPtr;
            UInt32 err = PFPInterop.PartyLocalUserGetEntityId(
                localUser.InteropHandle,
                out entityIdPtr);
            if (PartyError.SUCCEEDED(err))
            {
                entityId = entityIdPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyLocalUserUpdateEntityToken(
            PARTY_LOCAL_USER_HANDLE localUser,
            string titlePlayerEntityToken)
        {
            if (localUser == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyLocalUserUpdateEntityToken(
                localUser.InteropHandle,
                Converters.StringToNullTerminatedUTF8ByteArray(titlePlayerEntityToken));
        }

        public static UInt32 PartyLocalUserGetCustomContext(
            PARTY_LOCAL_USER_HANDLE localUser,
            out Object customContext)
        {
            if (localUser == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_LOCAL_USER_HANDLE>(
                PFPInterop.PartyLocalUserGetCustomContext,
                localUser.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyLocalUserSetCustomContext(
            PARTY_LOCAL_USER_HANDLE localUser,
            Object customContext)
        {
            if (localUser == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_LOCAL_USER_HANDLE>(
                PFPInterop.PartyLocalUserGetCustomContext,
                PFPInterop.PartyLocalUserSetCustomContext,
                localUser.InteropHandle,
                customContext);
        }

        public static UInt32 PartyGetNetworks(
            PARTY_HANDLE handle,
            out PARTY_NETWORK_HANDLE[] networks)
        {
            networks = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_HANDLE, Interop.PARTY_NETWORK_HANDLE, PARTY_NETWORK_HANDLE>(
                PFPInterop.PartyGetNetworks,
                s => new PARTY_NETWORK_HANDLE(s),
                handle.InteropHandle,
                out networks);
        }

        public static UInt32 PartySetOption(
            Object contextObject,
            PARTY_OPTION option,
            Object value)
        {
            UInt32 err = PartyError.UnsupportedPartyOption;
            if (option == PARTY_OPTION.PARTY_OPTION_LOCAL_UDP_SOCKET_BIND_ADDRESS)
            {
                using (DisposableCollection dc = new DisposableCollection())
                {
                    if (value != null && value.GetType() == typeof(PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION))
                    {
                        SizeT valueCount;
                        var valuePtr = Converters.ClassArrayToPtr<PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION, Interop.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION>(
                            new PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION[] { (PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION)value },
                            (x, d) => new Interop.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION(x),
                            dc,
                            out valueCount);
                        err = PFPInterop.PartySetOption(IntPtr.Zero, option, valuePtr);
                    }
                    else
                    {
                        err = PartyError.InvalidArg;
                    }
                }
            }

            return err;
        }

        public static UInt32 PartyGetOption(
            Object contextObject,
            PARTY_OPTION option,
            out Object value)
        {
            UInt32 err = PartyError.UnsupportedPartyOption;
            value = null;
            if (option == PARTY_OPTION.PARTY_OPTION_LOCAL_UDP_SOCKET_BIND_ADDRESS)
            {
                Interop.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION config = default(Interop.PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION);
                unsafe
                {
                    err = PFPInterop.PartyGetOption(
                        IntPtr.Zero,
                        option,
                        (IntPtr)(&config));
                    if (PartyError.SUCCEEDED(err))
                    {
                        value = new PARTY_LOCAL_UDP_SOCKET_BIND_ADDRESS_CONFIGURATION(config);
                    }
                }
            }

            return err;
        }

        public static UInt32 PartySetThreadAffinityMask(
            PARTY_THREAD_ID threadId,
            UInt64 threadAffinityMask)
        {
            return PFPInterop.PartySetThreadAffinityMask(threadId, threadAffinityMask);
        }

        public static UInt32 PartyGetThreadAffinityMask(
            PARTY_THREAD_ID threadId,
            out UInt64 threadAffinityMask)
        {
            return PFPInterop.PartyGetThreadAffinityMask(threadId, out threadAffinityMask);
        }

        public static UInt32 PartySetWorkMode(
            PARTY_THREAD_ID threadId,
            PARTY_WORK_MODE workMode)
        {
            return PFPInterop.PartySetWorkMode(threadId, workMode);
        }

        public static UInt32 PartyGetWorkMode(
            PARTY_THREAD_ID threadId,
            out PARTY_WORK_MODE workMode)
        {
            return PFPInterop.PartyGetWorkMode(threadId, out workMode);
        }

        public static UInt32 PartyDoWork(
            PARTY_HANDLE handle,
            PARTY_THREAD_ID threadId)
        {
            return PFPInterop.PartyDoWork(handle.InteropHandle, threadId);
        }

        public static UInt32 PartyGetChatControls(
            PARTY_HANDLE handle,
            out PARTY_CHAT_CONTROL_HANDLE[] chatControls)
        {
            chatControls = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE, PARTY_CHAT_CONTROL_HANDLE>(
                PFPInterop.PartyGetChatControls,
                s => new PARTY_CHAT_CONTROL_HANDLE(s),
                handle.InteropHandle,
                out chatControls);
        }

        // PartyNetwork
        public static UInt32 PartyNetworkAuthenticateLocalUser(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            string invitationIdentifier,
            Object asyncIdentifier)
        {
            if (network == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkAuthenticateLocalUser(
                network.InteropHandle,
                localUser.InteropHandle,
                Converters.StringToNullTerminatedUTF8ByteArray(invitationIdentifier),
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkGetNetworkDescriptor(
            PARTY_NETWORK_HANDLE network,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor)
        {
            networkDescriptor = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_NETWORK_DESCRIPTOR descriptorInterop;
            UInt32 err = PFPInterop.PartyNetworkGetNetworkDescriptor(
                network.InteropHandle,
                out descriptorInterop);
            if (PartyError.SUCCEEDED(err))
            {
                networkDescriptor = new PARTY_NETWORK_DESCRIPTOR(descriptorInterop);
            }

            return err;
        }

        public static UInt32 PartyNetworkCreateInvitation(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_INVITATION_CONFIGURATION invitationConfiguration,
            Object asyncIdentifier,
            out PARTY_INVITATION_HANDLE invitation)
        {
            UInt32 err;
            invitation = null;
            if (network == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                var inviteConfig = new Interop.PARTY_INVITATION_CONFIGURATION(invitationConfiguration, dc);
                var asyncId = IntPtr.Zero;
                if (asyncIdentifier != null)
                {
                    asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                }

                Interop.PARTY_INVITATION_HANDLE interopHandle;
                unsafe
                {
                    err = PFPInterop.PartyNetworkCreateInvitation(
                        network.InteropHandle,
                        localUser.InteropHandle,
                        &inviteConfig,
                        asyncId,
                        out interopHandle);
                }
                if (PartyError.SUCCEEDED(err))
                {
                    invitation = new PARTY_INVITATION_HANDLE(interopHandle);
                }
                else
                {
                    if (asyncId != IntPtr.Zero)
                    {
                        GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                        asyncGcHandle.Free();
                    }
                }

                return err;
            }
        }

        public static UInt32 PartyNetworkDestroyEndpoint(
            PARTY_NETWORK_HANDLE network,
            PARTY_ENDPOINT_HANDLE localEndpoint,
            Object asyncIdentifier)
        {
            if (network == null || localEndpoint == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkDestroyEndpoint(
                network.InteropHandle,
                localEndpoint.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkGetChatControls(
            PARTY_NETWORK_HANDLE network,
            out PARTY_CHAT_CONTROL_HANDLE[] chatControls)
        {
            chatControls = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_NETWORK_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE, PARTY_CHAT_CONTROL_HANDLE>(
                PFPInterop.PartyNetworkGetChatControls,
                s => new PARTY_CHAT_CONTROL_HANDLE(s),
                network.InteropHandle,
                out chatControls);
        }

        public static UInt32 PartyNetworkFindEndpointByUniqueIdentifier(
            PARTY_NETWORK_HANDLE network,
            UInt16 uniqueIdentifier,
            out PARTY_ENDPOINT_HANDLE endpoint)
        {
            endpoint = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_ENDPOINT_HANDLE endpointInterop;
            UInt32 err = PFPInterop.PartyNetworkFindEndpointByUniqueIdentifier(
                network.InteropHandle,
                uniqueIdentifier,
                out endpointInterop);
            return PARTY_ENDPOINT_HANDLE.WrapAndReturnError(err, endpointInterop, out endpoint);
        }

        public static UInt32 PartyNetworkGetCustomContext(
            PARTY_NETWORK_HANDLE network,
            out Object customContext)
        {
            if (network == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_NETWORK_HANDLE>(
                PFPInterop.PartyNetworkGetCustomContext,
                network.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyNetworkSetCustomContext(
            PARTY_NETWORK_HANDLE network,
            Object customContext)
        {
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_NETWORK_HANDLE>(
                PFPInterop.PartyNetworkGetCustomContext,
                PFPInterop.PartyNetworkSetCustomContext,
                network.InteropHandle,
                customContext);
        }

        public static UInt32 PartyNetworkGetDevices(
            PARTY_NETWORK_HANDLE network,
            out PARTY_DEVICE_HANDLE[] devices)
        {
            devices = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_NETWORK_HANDLE, Interop.PARTY_DEVICE_HANDLE, PARTY_DEVICE_HANDLE>(
                PFPInterop.PartyNetworkGetDevices,
                s => new PARTY_DEVICE_HANDLE(s),
                network.InteropHandle,
                out devices);
        }

        public static UInt32 PartyNetworkGetEndpoints(
            PARTY_NETWORK_HANDLE network,
            out PARTY_ENDPOINT_HANDLE[] endpoints)
        {
            endpoints = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_NETWORK_HANDLE, Interop.PARTY_ENDPOINT_HANDLE, PARTY_ENDPOINT_HANDLE>(
                PFPInterop.PartyNetworkGetEndpoints,
                s => new PARTY_ENDPOINT_HANDLE(s),
                network.InteropHandle,
                out endpoints);
        }

        public static UInt32 PartyNetworkGetInvitations(
            PARTY_NETWORK_HANDLE network,
            out PARTY_INVITATION_HANDLE[] invitations)
        {
            invitations = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_NETWORK_HANDLE, Interop.PARTY_INVITATION_HANDLE, PARTY_INVITATION_HANDLE>(
                PFPInterop.PartyNetworkGetInvitations,
                s => new PARTY_INVITATION_HANDLE(s),
                network.InteropHandle,
                out invitations);
        }

        public static UInt32 PartyNetworkGetLocalUsers(
            PARTY_NETWORK_HANDLE network,
            out PARTY_LOCAL_USER_HANDLE[] users)
        {
            users = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_NETWORK_HANDLE, Interop.PARTY_LOCAL_USER_HANDLE, PARTY_LOCAL_USER_HANDLE>(
                PFPInterop.PartyNetworkGetLocalUsers,
                s => new PARTY_LOCAL_USER_HANDLE(s),
                network.InteropHandle,
                out users);
        }

        public static UInt32 PartyNetworkGetNetworkConfiguration(
            PARTY_NETWORK_HANDLE network,
            out PARTY_NETWORK_CONFIGURATION networkConfiguration)
        {
            networkConfiguration = null;
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            IntPtr configurationPtr;
            UInt32 err = PFPInterop.PartyNetworkGetNetworkConfiguration(
                    network.InteropHandle,
                    out configurationPtr);
            if (PartyError.SUCCEEDED(err))
            {
                networkConfiguration = Converters.PtrToClass<PARTY_NETWORK_CONFIGURATION, Interop.PARTY_NETWORK_CONFIGURATION>(
                    configurationPtr,
                    s => new PARTY_NETWORK_CONFIGURATION(s));
            }

            return err;
        }

        public static UInt32 PartyNetworkGetNetworkStatistics(
            PARTY_NETWORK_HANDLE network,
            PARTY_NETWORK_STATISTIC[] statisticTypes,
            out UInt64[] statisticValues)
        {
            statisticValues = new UInt64[statisticTypes.Length];
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyNetworkGetNetworkStatistics(
                network.InteropHandle,
                (UInt32)statisticTypes.Length,
                statisticTypes,
                statisticValues);
        }

        public static UInt32 PartyNetworkKickDevice(
            PARTY_NETWORK_HANDLE network,
            PARTY_DEVICE_HANDLE targetDevice,
            Object asyncIdentifier)
        {
            if (network == null || targetDevice == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkKickDevice(
                network.InteropHandle,
                targetDevice.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkKickUser(
            PARTY_NETWORK_HANDLE network,
            string targetEntityId,
            Object asyncIdentifier)
        {
            if (network == null || targetEntityId == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkKickUser(
                network.InteropHandle,
                Converters.StringToNullTerminatedUTF8ByteArray(targetEntityId),
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkLeaveNetwork(
            PARTY_NETWORK_HANDLE network,
            Object asyncIdentifier)
        {
            if (network == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkLeaveNetwork(
                network.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkRemoveLocalUser(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            Object asyncIdentifier)
        {
            if (network == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkRemoveLocalUser(
                network.InteropHandle,
                localUser.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkRevokeInvitation(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            PARTY_INVITATION_HANDLE invitation,
            Object asyncIdentifier)
        {
            if (network == null || localUser == null || invitation == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkRevokeInvitation(
                network.InteropHandle,
                localUser.InteropHandle,
                invitation.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartySerializeNetworkDescriptor(
            PARTY_NETWORK_DESCRIPTOR networkDescriptor,
            out string serializedNetworkDescriptorString)
        {
            UInt32 err;
            serializedNetworkDescriptorString = null;
            if (networkDescriptor == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_NETWORK_DESCRIPTOR descriptorInterop = new Interop.PARTY_NETWORK_DESCRIPTOR(networkDescriptor);
            using (DisposableBuffer buffer = new DisposableBuffer(PartyConstants.c_maxSerializedNetworkDescriptorStringLength + 1))
            {
                var serializedNetworkDescriptor = buffer.IntPtr;
                unsafe
                {
                    err = PFPInterop.PartySerializeNetworkDescriptor(
                        &descriptorInterop,
                        serializedNetworkDescriptor);
                }
                if (PartyError.SUCCEEDED(err))
                {
                    serializedNetworkDescriptorString = Converters.PtrToStringUTF8(serializedNetworkDescriptor);
                }
            }

            return err;
        }

        public static UInt32 PartyDeserializeNetworkDescriptor(
            string serializedNetworkDescriptorString,
            out PARTY_NETWORK_DESCRIPTOR networkDescriptor)
        {
            networkDescriptor = null;
            if (serializedNetworkDescriptorString == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_NETWORK_DESCRIPTOR descriptorInterop;
            UInt32 err = PFPInterop.PartyDeserializeNetworkDescriptor(
                Converters.StringToNullTerminatedUTF8ByteArray(serializedNetworkDescriptorString),
                out descriptorInterop);
            if (PartyError.SUCCEEDED(err))
            {
                networkDescriptor = new PARTY_NETWORK_DESCRIPTOR(descriptorInterop);
            }

            return err;
        }

        public static UInt32 PartyNetworkConnectChatControl(
            PARTY_NETWORK_HANDLE network,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Object asyncIdentifier)
        {
            if (network == null || chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkConnectChatControl(
                network.InteropHandle,
                chatControl.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyNetworkDisconnectChatControl(
            PARTY_NETWORK_HANDLE network,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Object asyncIdentifier)
        {
            if (network == null || chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyNetworkDisconnectChatControl(
                network.InteropHandle,
                chatControl.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        // PartyInvitation
        public static UInt32 PartyInvitationGetCreatorEntityId(
            PARTY_INVITATION_HANDLE invitation,
            out string entityId)
        {
            entityId = null;
            if (invitation == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr entityIdPtr;
            UInt32 err = PFPInterop.PartyInvitationGetCreatorEntityId(
                invitation.InteropHandle,
                out entityIdPtr);
            if (PartyError.SUCCEEDED(err))
            {
                entityId = entityIdPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyInvitationGetInvitationConfiguration(
            PARTY_INVITATION_HANDLE invitation,
            out PARTY_INVITATION_CONFIGURATION configuration)
        {
            configuration = null;
            if (invitation == null)
            {
                return PartyError.InvalidArg;
            }

            IntPtr configurationPtr;
            UInt32 err = PFPInterop.PartyInvitationGetInvitationConfiguration(
                    invitation.InteropHandle,
                    out configurationPtr);
            if (PartyError.SUCCEEDED(err))
            {
                configuration = Converters.PtrToClass<PARTY_INVITATION_CONFIGURATION, Interop.PARTY_INVITATION_CONFIGURATION>(
                    configurationPtr,
                    s => new PARTY_INVITATION_CONFIGURATION(s));
            }

            return err;
        }

        public static UInt32 PartyInvitationGetCustomContext(
            PARTY_INVITATION_HANDLE invitation,
            out Object customContext)
        {
            if (invitation == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_INVITATION_HANDLE>(
                PFPInterop.PartyInvitationGetCustomContext,
                invitation.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyInvitationSetCustomContext(
            PARTY_INVITATION_HANDLE invitation,
            Object customContext)
        {
            if (invitation == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_INVITATION_HANDLE>(
                PFPInterop.PartyInvitationGetCustomContext,
                PFPInterop.PartyInvitationSetCustomContext,
                invitation.InteropHandle,
                customContext);
        }

        public static UInt32 PartyNetworkCreateEndpoint(
            PARTY_NETWORK_HANDLE network,
            PARTY_LOCAL_USER_HANDLE localUser,
            Dictionary<string, Byte[]> keyValuePairs,
            Object asyncIdentifier,
            out PARTY_ENDPOINT_HANDLE endpoint)
        {
            UInt32 err;
            endpoint = null;
            if (network == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                UInt32 propertyCount = 0;
                var keyPtrs = IntPtr.Zero;
                var valuePtrs = IntPtr.Zero;
                if (keyValuePairs != null)
                {
                    propertyCount = (UInt32)keyValuePairs.Count;
                    if (propertyCount > 0)
                    {
                        List<string> keys = new List<string>();
                        List<Byte[]> values = new List<byte[]>();
                        foreach (var pair in keyValuePairs)
                        {
                            keys.Add(pair.Key);
                            values.Add(pair.Value);
                        }

                        SizeT size;
                        keyPtrs = Converters.ClassArrayToPtr<string, UTF8StringPtr>(
                            keys.ToArray(),
                            (x, d) => new UTF8StringPtr(x, d),
                            dc,
                            out size);
                        valuePtrs = Converters.ClassArrayToPtr<Byte[], Interop.PARTY_DATA_BUFFER>(
                            values.ToArray(),
                            (x, d) => new Interop.PARTY_DATA_BUFFER(x, d),
                            dc,
                            out size);
                    }
                }

                var asyncId = IntPtr.Zero;
                if (asyncIdentifier != null)
                {
                    asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
                }

                Interop.PARTY_ENDPOINT_HANDLE interopHandle;
                err = PFPInterop.PartyNetworkCreateEndpoint(
                    network.InteropHandle,
                    localUser.InteropHandle,
                    propertyCount,
                    keyPtrs,
                    valuePtrs,
                    asyncId,
                    out interopHandle);
                if (PartyError.SUCCEEDED(err))
                {
                    endpoint = new PARTY_ENDPOINT_HANDLE(interopHandle);
                }
                else
                {
                    if (asyncId != IntPtr.Zero)
                    {
                        GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                        asyncGcHandle.Free();
                    }
                }
            }

            return err;
        }

        // PartyEndpoint
        public static UInt32 PartyEndpointSendMessage(
            PARTY_ENDPOINT_HANDLE endpoint,
            PARTY_ENDPOINT_HANDLE[] targetEndpoints,
            PARTY_SEND_MESSAGE_OPTIONS options,
            PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION queuingConfiguration,
            Byte[] dataBuffer)
        {
            UInt32 err;
            if (dataBuffer == null)
            {
                return PartyError.InvalidArg;
            }

            GCHandle bufferHandle = GCHandle.Alloc(dataBuffer, GCHandleType.Pinned);
            err = PartyEndpointSendMessage(
                endpoint,
                targetEndpoints,
                options,
                queuingConfiguration,
                bufferHandle.AddrOfPinnedObject(),
                (UInt32)dataBuffer.Length);
            bufferHandle.Free();
            return err;
        }

        unsafe public static UInt32 PartyEndpointSendMessage(
            PARTY_ENDPOINT_HANDLE endpoint,
            PARTY_ENDPOINT_HANDLE[] targetEndpoints,
            PARTY_SEND_MESSAGE_OPTIONS options,
            PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION queuingConfiguration,
            IntPtr dataBuffer,
            UInt32 dataBufferSize)
        {
            if (endpoint == null || queuingConfiguration == null)
            {
                return PartyError.InvalidArg;
            }

            UInt32 targetCount = 0;
            IntPtr targetPtr = IntPtr.Zero;
            if (targetEndpoints != null)
            {
                IntPtr* targetPtrArray = stackalloc IntPtr[targetEndpoints.Length];
                for (int i = 0; i < targetEndpoints.Length; ++i)
                {
                    targetPtrArray[i] = targetEndpoints[i].InteropHandle.handle;
                }

                targetPtr = new IntPtr(targetPtrArray);
                targetCount = (UInt32)targetEndpoints.Length;
            }

            // struct type should not invoke GC
            Interop.PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION configurationInterop = new Interop.PARTY_SEND_MESSAGE_QUEUING_CONFIGURATION(queuingConfiguration);
            Interop.PARTY_DATA_BUFFER buffer = new Interop.PARTY_DATA_BUFFER(dataBuffer, dataBufferSize);
            return PFPInterop.PartyEndpointSendMessage(
                endpoint.InteropHandle,
                targetCount,
                targetPtr,
                options,
                &configurationInterop,
                1,
                &buffer,
                IntPtr.Zero);
        }

        public static UInt32 PartyEndpointCancelMessages(
            PARTY_ENDPOINT_HANDLE endpoint,
            PARTY_ENDPOINT_HANDLE[] targetEndpoints,
            PARTY_CANCEL_MESSAGES_FILTER_EXPRESSION filterExpression,
            UInt32 messageIdentityFilterMask,
            UInt32 filteredMessageIdentitiesToMatch,
            out UInt32 canceledMessagesCount)
        {
            UInt32 err;
            canceledMessagesCount = 0;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                SizeT targetCount;
                var targetPtrs = Converters.ClassArrayToPtr<PARTY_ENDPOINT_HANDLE, Interop.PARTY_ENDPOINT_HANDLE>(
                    targetEndpoints,
                    (x, d) => x.InteropHandle,
                    dc,
                    out targetCount);
                err = PFPInterop.PartyEndpointCancelMessages(
                    endpoint.InteropHandle,
                    targetCount.ToUInt32(),
                    targetPtrs,
                    filterExpression,
                    messageIdentityFilterMask,
                    filteredMessageIdentitiesToMatch,
                    out canceledMessagesCount);
            }

            return err;
        }

        public static UInt32 PartyEndpointFlushMessages(
            PARTY_ENDPOINT_HANDLE endpoint,
            PARTY_ENDPOINT_HANDLE[] targetEndpoints)
        {
            UInt32 err;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                SizeT targetCount;
                var targetPtrs = Converters.ClassArrayToPtr<PARTY_ENDPOINT_HANDLE, Interop.PARTY_ENDPOINT_HANDLE>(
                    targetEndpoints,
                    (x, d) => x.InteropHandle,
                    dc,
                    out targetCount);
                err = PFPInterop.PartyEndpointFlushMessages(
                    endpoint.InteropHandle,
                    targetCount.ToUInt32(),
                    targetPtrs);
            }

            return err;
        }

        public static UInt32 PartyEndpointGetEndpointStatistics(
            PARTY_ENDPOINT_HANDLE endpoint,
            PARTY_ENDPOINT_HANDLE[] targetEndpoints,
            PARTY_ENDPOINT_STATISTIC[] statisticTypes,
            out UInt64[] statisticValues)
        {
            UInt32 err;
            statisticValues = new UInt64[statisticTypes.Length];
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                SizeT targetCount;
                var targetPtrs = Converters.ClassArrayToPtr<PARTY_ENDPOINT_HANDLE, Interop.PARTY_ENDPOINT_HANDLE>(
                    targetEndpoints,
                    (x, d) => x.InteropHandle,
                    dc,
                    out targetCount);
                err = PFPInterop.PartyEndpointGetEndpointStatistics(
                    endpoint.InteropHandle,
                    targetCount.ToUInt32(),
                    targetPtrs,
                    (UInt32)statisticTypes.Length,
                    statisticTypes,
                    statisticValues);
            }

            return err;
        }

        public static UInt32 PartyEndpointGetCustomContext(
            PARTY_ENDPOINT_HANDLE endpoint,
            out Object customContext)
        {
            if (endpoint == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_ENDPOINT_HANDLE>(
                PFPInterop.PartyEndpointGetCustomContext,
                endpoint.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyEndpointSetCustomContext(
            PARTY_ENDPOINT_HANDLE endpoint,
            Object customContext)
        {
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_ENDPOINT_HANDLE>(
                PFPInterop.PartyEndpointGetCustomContext,
                PFPInterop.PartyEndpointSetCustomContext,
                endpoint.InteropHandle,
                customContext);
        }

        public static UInt32 PartyEndpointGetDevice(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_DEVICE_HANDLE device)
        {
            device = null;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_DEVICE_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyEndpointGetDevice(endpoint.InteropHandle, out interopHandle);
            return PARTY_DEVICE_HANDLE.WrapAndReturnError(err, interopHandle, out device);
        }

        public static UInt32 PartyEndpointGetEntityId(
            PARTY_ENDPOINT_HANDLE endpoint,
            out string entityId)
        {
            entityId = null;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr entityIdPtr;
            UInt32 err = PFPInterop.PartyEndpointGetEntityId(
                endpoint.InteropHandle,
                out entityIdPtr);
            if (PartyError.SUCCEEDED(err))
            {
                entityId = entityIdPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyEndpointGetLocalUser(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_LOCAL_USER_HANDLE localUser)
        {
            localUser = null;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_LOCAL_USER_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyEndpointGetLocalUser(endpoint.InteropHandle, out interopHandle);
            return PARTY_LOCAL_USER_HANDLE.WrapAndReturnError(err, interopHandle, out localUser);
        }

        public static UInt32 PartyEndpointGetNetwork(
            PARTY_ENDPOINT_HANDLE endpoint,
            out PARTY_NETWORK_HANDLE network)
        {
            network = null;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_NETWORK_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyEndpointGetNetwork(endpoint.InteropHandle, out interopHandle);
            return PARTY_NETWORK_HANDLE.WrapAndReturnError(err, interopHandle, out network);
        }

        public static UInt32 PartyEndpointGetUniqueIdentifier(
            PARTY_ENDPOINT_HANDLE endpoint,
            out UInt16 uniqueIdentifier)
        {
            uniqueIdentifier = 0;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyEndpointGetUniqueIdentifier(endpoint.InteropHandle, out uniqueIdentifier);
        }

        public static UInt32 PartyEndpointIsLocal(
            PARTY_ENDPOINT_HANDLE endpoint,
            out bool isLocal)
        {
            isLocal = false;
            if (endpoint == null)
            {
                return PartyError.InvalidArg;
            }

            Byte isLocalByte;
            UInt32 err = PFPInterop.PartyEndpointIsLocal(endpoint.InteropHandle, out isLocalByte);
            if (PartyError.SUCCEEDED(err))
            {
                isLocal = (isLocalByte != 0);
            }

            return err;
        }

        // PartyDevice
        public static UInt32 PartyDeviceIsLocal(
            PARTY_DEVICE_HANDLE device,
            out bool isLocal)
        {
            isLocal = false;
            if (device == null)
            {
                return PartyError.InvalidArg;
            }

            Byte isLocalByte;
            UInt32 err = PFPInterop.PartyDeviceIsLocal(device.InteropHandle, out isLocalByte);
            if (PartyError.SUCCEEDED(err))
            {
                isLocal = (isLocalByte != 0);
            }

            return err;
        }

        public static UInt32 PartyDeviceGetCustomContext(
            PARTY_DEVICE_HANDLE device,
            out Object customContext)
        {
            if (device == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_DEVICE_HANDLE>(
                PFPInterop.PartyDeviceGetCustomContext,
                device.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyDeviceSetCustomContext(
            PARTY_DEVICE_HANDLE device,
            Object customContext)
        {
            if (device == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_DEVICE_HANDLE>(
                PFPInterop.PartyDeviceGetCustomContext,
                PFPInterop.PartyDeviceSetCustomContext,
                device.InteropHandle,
                customContext);
        }

        public static UInt32 PartyDeviceCreateChatControl(
            PARTY_DEVICE_HANDLE device,
            PARTY_LOCAL_USER_HANDLE localUser,
            string languageCode,
            Object asyncIdentifier,
            out PARTY_CHAT_CONTROL_HANDLE chatControl)
        {
            chatControl = null;
            if (device == null || localUser == null)
            {
                return PartyError.InvalidArg;
            }

            Byte[] languageCodeByteArray = null;
            if (!String.IsNullOrEmpty(languageCode))
            {
                languageCodeByteArray = Converters.StringToNullTerminatedUTF8ByteArray(languageCode);
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            Interop.PARTY_CHAT_CONTROL_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyDeviceCreateChatControl(
                device.InteropHandle,
                localUser.InteropHandle,
                languageCodeByteArray,
                asyncId,
                out interopHandle);
            if (PartyError.SUCCEEDED(err))
            {
                chatControl = new PARTY_CHAT_CONTROL_HANDLE(interopHandle);
            }
            else
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyDeviceDestroyChatControl(
            PARTY_DEVICE_HANDLE device,
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Object asyncIdentifier)
        {
            if (device == null || chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyDeviceDestroyChatControl(
                device.InteropHandle,
                chatControl.InteropHandle,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyDeviceGetChatControls(
            PARTY_DEVICE_HANDLE device,
            out PARTY_CHAT_CONTROL_HANDLE[] chatControls)
        {
            chatControls = null;
            if (device == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_DEVICE_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE, PARTY_CHAT_CONTROL_HANDLE>(
                PFPInterop.PartyDeviceGetChatControls,
                s => new PARTY_CHAT_CONTROL_HANDLE(s),
                device.InteropHandle,
                out chatControls);
        }

        // PartyChatControl
        public static UInt32 PartyChatControlSendText(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE[] targetChatControls,
            string chatText,
            Byte[][] dataBuffers)
        {
            UInt32 err;
            if (chatControl == null || targetChatControls == null || chatText == null)
            {
                return PartyError.InvalidArg;
            }

            using (DisposableCollection dc = new DisposableCollection())
            {
                SizeT targetCount;
                var targetPtrs = Converters.ClassArrayToPtr<PARTY_CHAT_CONTROL_HANDLE, Interop.PARTY_CHAT_CONTROL_HANDLE>(
                    targetChatControls,
                    (x, d) => x.InteropHandle,
                    dc,
                    out targetCount);
                SizeT bufferCount;
                var bufferPtrs = Converters.ClassArrayToPtr<Byte[], Interop.PARTY_DATA_BUFFER>(
                    dataBuffers,
                    (x, d) => new Interop.PARTY_DATA_BUFFER(x, dc),
                    dc,
                    out bufferCount);
                err = PFPInterop.PartyChatControlSendText(
                    chatControl.InteropHandle,
                    targetCount.ToUInt32(),
                    targetPtrs,
                    Converters.StringToNullTerminatedUTF8ByteArray(chatText),
                    bufferCount.ToUInt32(),
                    bufferPtrs);
            }

            return err;
        }

        public static UInt32 PartyChatControlSetAudioInput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            string audioDeviceSelectionContext,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte[] deviceSelectionByteArray = null;
            if (!String.IsNullOrEmpty(audioDeviceSelectionContext))
            {
                deviceSelectionByteArray = Converters.StringToNullTerminatedUTF8ByteArray(audioDeviceSelectionContext);
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetAudioInput(
               chatControl.InteropHandle,
               audioDeviceSelectionType,
               deviceSelectionByteArray,
               asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetAudioInput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            out string audioDeviceSelectionContext,
            out string deviceId)
        {
            audioDeviceSelectionType = PARTY_AUDIO_DEVICE_SELECTION_TYPE.PARTY_AUDIO_DEVICE_SELECTION_TYPE_NONE;
            audioDeviceSelectionContext = null;
            deviceId = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr contextStrPtr;
            UTF8StringPtr deviceStrPtr;
            UInt32 err = PFPInterop.PartyChatControlGetAudioInput(
                chatControl.InteropHandle,
                out audioDeviceSelectionType,
                out contextStrPtr,
                out deviceStrPtr);
            if (PartyError.SUCCEEDED(err))
            {
                audioDeviceSelectionContext = contextStrPtr.GetString();
                deviceId = deviceStrPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyChatControlSetAudioOutput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            string audioDeviceSelectionContext,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte[] deviceSelectionByteArray = null;
            if (!String.IsNullOrEmpty(audioDeviceSelectionContext))
            {
                deviceSelectionByteArray = Converters.StringToNullTerminatedUTF8ByteArray(audioDeviceSelectionContext);
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetAudioOutput(
               chatControl.InteropHandle,
               audioDeviceSelectionType,
               deviceSelectionByteArray,
               asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetAudioOutput(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_AUDIO_DEVICE_SELECTION_TYPE audioDeviceSelectionType,
            out string audioDeviceSelectionContext,
            out string deviceId)
        {
            audioDeviceSelectionType = PARTY_AUDIO_DEVICE_SELECTION_TYPE.PARTY_AUDIO_DEVICE_SELECTION_TYPE_NONE;
            audioDeviceSelectionContext = null;
            deviceId = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr contextStrPtr;
            UTF8StringPtr deviceStrPtr;
            UInt32 err = PFPInterop.PartyChatControlGetAudioOutput(
                chatControl.InteropHandle,
                out audioDeviceSelectionType,
                out contextStrPtr,
                out deviceStrPtr);
            if (PartyError.SUCCEEDED(err))
            {
                audioDeviceSelectionContext = contextStrPtr.GetString();
                deviceId = deviceStrPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyChatControlSetAudioEncoderBitrate(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            UInt32 bitrate,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetAudioEncoderBitrate(
                chatControl.InteropHandle,
                bitrate,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetAudioEncoderBitrate(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out UInt32 bitrate)
        {
            bitrate = 0;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetAudioEncoderBitrate(
                chatControl.InteropHandle,
                out bitrate);
        }

        public static UInt32 PartyChatControlSetAudioRenderVolume(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            float volume)
        {
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlSetAudioRenderVolume(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                volume);
        }

        public static UInt32 PartyChatControlGetAudioRenderVolume(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out float volume)
        {
            volume = 0;
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetAudioRenderVolume(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                out volume);
        }

        public static UInt32 PartyChatControlSetAudioInputMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            bool muted)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlSetAudioInputMuted(
                chatControl.InteropHandle,
                Convert.ToByte(muted));
        }

        public static UInt32 PartyChatControlGetAudioInputMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out bool muted)
        {
            muted = false;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte mutedByte;
            UInt32 err = PFPInterop.PartyChatControlGetAudioInputMuted(
                chatControl.InteropHandle,
                out mutedByte);
            if (PartyError.SUCCEEDED(err))
            {
                muted = (mutedByte != 0);
            }

            return err;
        }

        public static UInt32 PartyChatControlSetIncomingAudioMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            bool muted)
        {
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlSetIncomingAudioMuted(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                Convert.ToByte(muted));
        }

        public static UInt32 PartyChatControlGetIncomingAudioMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out bool muted)
        {
            muted = false;
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte mutedByte;
            UInt32 err = PFPInterop.PartyChatControlGetIncomingAudioMuted(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                out mutedByte);
            if (PartyError.SUCCEEDED(err))
            {
                muted = (mutedByte != 0);
            }

            return err;
        }

        public static UInt32 PartyChatControlSetIncomingTextMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            bool muted)
        {
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlSetIncomingTextMuted(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                Convert.ToByte(muted));
        }

        public static UInt32 PartyChatControlGetIncomingTextMuted(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out bool muted)
        {
            muted = false;
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte mutedByte;
            UInt32 err = PFPInterop.PartyChatControlGetIncomingTextMuted(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                out mutedByte);
            if (PartyError.SUCCEEDED(err))
            {
                muted = (mutedByte != 0);
            }

            return err;
        }

        public static UInt32 PartyChatControlIsLocal(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out bool isLocal)
        {
            isLocal = false;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte isLocalByte;
            UInt32 err = PFPInterop.PartyChatControlIsLocal(
                chatControl.InteropHandle,
                out isLocalByte);
            if (PartyError.SUCCEEDED(err))
            {
                isLocal = (isLocalByte != 0);
            }

            return err;
        }

        public static UInt32 PartyChatControlSetPermissions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            PARTY_CHAT_PERMISSION_OPTIONS chatPermissionOptions)
        {
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlSetPermissions(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                chatPermissionOptions);
        }

        public static UInt32 PartyChatControlGetPermissions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out PARTY_CHAT_PERMISSION_OPTIONS chatPermissionOptions)
        {
            chatPermissionOptions = PARTY_CHAT_PERMISSION_OPTIONS.PARTY_CHAT_PERMISSION_OPTIONS_NONE;
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetPermissions(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                out chatPermissionOptions);
        }



        public static UInt32 PartyChatControlGetCustomContext(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out Object customContext)
        {
            if (chatControl == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_CHAT_CONTROL_HANDLE>(
                PFPInterop.PartyChatControlGetCustomContext,
                chatControl.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyChatControlSetCustomContext(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Object customContext)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_CHAT_CONTROL_HANDLE>(
                PFPInterop.PartyChatControlGetCustomContext,
                PFPInterop.PartyChatControlSetCustomContext,
                chatControl.InteropHandle,
                customContext);
        }

        public static UInt32 PartyChatControlGetLanguage(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out string languageCode)
        {
            languageCode = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr stringPtr;
            UInt32 err = PFPInterop.PartyChatControlGetLanguage(
                chatControl.InteropHandle,
                out stringPtr);
            if (PartyError.SUCCEEDED(err))
            {
                languageCode = stringPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyChatControlSetLanguage(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            string languageCode,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Byte[] languageCodeByteArray = null;
            if (!String.IsNullOrEmpty(languageCode))
            {
                languageCodeByteArray = Converters.StringToNullTerminatedUTF8ByteArray(languageCode);
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetLanguage(
                chatControl.InteropHandle,
                languageCodeByteArray,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlSetTextChatOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_TEXT_CHAT_OPTIONS options,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetTextChatOptions(
                chatControl.InteropHandle,
                options,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetTextChatOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_TEXT_CHAT_OPTIONS options)
        {
            options = PARTY_TEXT_CHAT_OPTIONS.PARTY_TEXT_CHAT_OPTIONS_NONE;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetTextChatOptions(
                chatControl.InteropHandle,
                out options);
        }

        public static UInt32 PartyChatControlSynthesizeTextToSpeech(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            string textToSynthesize,
            Object asyncIdentifier)
        {
            if (chatControl == null || textToSynthesize == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSynthesizeTextToSpeech(
                chatControl.InteropHandle,
                type,
                Converters.StringToNullTerminatedUTF8ByteArray(textToSynthesize),
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlSetTranscriptionOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetTranscriptionOptions(
                chatControl.InteropHandle,
                options,
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetTranscriptionOptions(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS options)
        {
            options = PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS.PARTY_VOICE_CHAT_TRANSCRIPTION_OPTIONS_NONE;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetTranscriptionOptions(
                chatControl.InteropHandle,
                out options);
        }

        public static UInt32 PartyChatControlSetTextToSpeechProfile(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            string profileIdentifier,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlSetTextToSpeechProfile(
                chatControl.InteropHandle,
                type,
                Converters.StringToNullTerminatedUTF8ByteArray(profileIdentifier),
                asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetTextToSpeechProfile(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_SYNTHESIZE_TEXT_TO_SPEECH_TYPE type,
            out PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile)
        {
            profile = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyChatControlGetTextToSpeechProfile(
                chatControl.InteropHandle,
                type,
                out interopHandle);
            return PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE.WrapAndReturnError(err, interopHandle, out profile);
        }

        public static UInt32 PartyChatControlPopulateAvailableTextToSpeechProfiles(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            Object asyncIdentifier)
        {
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PFPInterop.PartyChatControlPopulateAvailableTextToSpeechProfiles(
               chatControl.InteropHandle,
               asyncId);
            if (PartyError.FAILED(err))
            {
                if (asyncId != IntPtr.Zero)
                {
                    GCHandle asyncGcHandle = GCHandle.FromIntPtr(asyncId);
                    asyncGcHandle.Free();
                }
            }

            return err;
        }

        public static UInt32 PartyChatControlGetAvailableTextToSpeechProfiles(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE[] profiles)
        {
            profiles = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_CHAT_CONTROL_HANDLE, Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE, PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE>(
                PFPInterop.PartyChatControlGetAvailableTextToSpeechProfiles,
                s => new PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE(s),
                chatControl.InteropHandle,
                out profiles);
        }

        public static UInt32 PartyTextToSpeechProfileGetCustomContext(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out Object customContext)
        {
            if (profile == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE>(
                PFPInterop.PartyTextToSpeechProfileGetCustomContext,
                profile.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyTextToSpeechProfileSetCustomContext(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            Object customContext)
        {
            if (profile == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE>(
                PFPInterop.PartyTextToSpeechProfileGetCustomContext,
                PFPInterop.PartyTextToSpeechProfileSetCustomContext,
                profile.InteropHandle,
                customContext);
        }

        public static UInt32 PartyTextToSpeechProfileGetGender(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out PARTY_GENDER gender)
        {
            gender = PARTY_GENDER.PARTY_GENDER_NEUTRAL;
            if (profile == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyTextToSpeechProfileGetGender(
                profile.InteropHandle,
                out gender
                );
        }

        public static UInt32 PartyTextToSpeechProfileGetIdentifier(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out string identifier)
        {
            identifier = null;
            if (profile == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr identifierPtr;
            UInt32 err = PFPInterop.PartyTextToSpeechProfileGetIdentifier(
                profile.InteropHandle,
                out identifierPtr);
            if (PartyError.SUCCEEDED(err))
            {
                identifier = identifierPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyTextToSpeechProfileGetLanguageCode(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out string languageCode)
        {
            languageCode = null;
            if (profile == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr stringPtr;
            UInt32 err = PFPInterop.PartyTextToSpeechProfileGetLanguageCode(
                profile.InteropHandle,
                out stringPtr);
            if (PartyError.SUCCEEDED(err))
            {
                languageCode = stringPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyTextToSpeechProfileGetName(
            PARTY_TEXT_TO_SPEECH_PROFILE_HANDLE profile,
            out string name)
        {
            name = null;
            if (profile == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr stringPtr;
            UInt32 err = PFPInterop.PartyTextToSpeechProfileGetName(
                profile.InteropHandle,
                out stringPtr);
            if (PartyError.SUCCEEDED(err))
            {
                name = stringPtr.GetString();
            }

            return err;
        }

        public static UInt32 PartyChatControlGetChatIndicator(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            PARTY_CHAT_CONTROL_HANDLE targetChatControl,
            out PARTY_CHAT_CONTROL_CHAT_INDICATOR chatIndicator)
        {
            chatIndicator = PARTY_CHAT_CONTROL_CHAT_INDICATOR.PARTY_CHAT_CONTROL_CHAT_INDICATOR_SILENT;
            if (chatControl == null || targetChatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetChatIndicator(
                chatControl.InteropHandle,
                targetChatControl.InteropHandle,
                out chatIndicator);
        }

        public static UInt32 PartyChatControlGetLocalChatIndicator(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_LOCAL_CHAT_CONTROL_CHAT_INDICATOR chatIndicator)
        {
            chatIndicator = PARTY_LOCAL_CHAT_CONTROL_CHAT_INDICATOR.PARTY_LOCAL_CHAT_CONTROL_CHAT_INDICATOR_SILENT;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return PFPInterop.PartyChatControlGetLocalChatIndicator(
                chatControl.InteropHandle,
                out chatIndicator);
        }

        public static UInt32 PartyChatControlGetDevice(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_DEVICE_HANDLE device)
        {
            device = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_DEVICE_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyChatControlGetDevice(
                chatControl.InteropHandle,
                out interopHandle);
            return PARTY_DEVICE_HANDLE.WrapAndReturnError(err, interopHandle, out device);
        }

        public static UInt32 PartyChatControlGetLocalUser(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_LOCAL_USER_HANDLE localUser)
        {
            localUser = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_LOCAL_USER_HANDLE interopHandle;
            UInt32 err = PFPInterop.PartyChatControlGetLocalUser(
                chatControl.InteropHandle,
                out interopHandle);
            return PARTY_LOCAL_USER_HANDLE.WrapAndReturnError(err, interopHandle, out localUser);
        }

        public static UInt32 PartyChatControlGetNetworks(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out PARTY_NETWORK_HANDLE[] networks)
        {
            networks = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_CHAT_CONTROL_HANDLE, Interop.PARTY_NETWORK_HANDLE, PARTY_NETWORK_HANDLE>(
                PFPInterop.PartyChatControlGetNetworks,
                s => new PARTY_NETWORK_HANDLE(s),
                chatControl.InteropHandle,
                out networks);
        }

        public static UInt32 PartyChatControlGetEntityId(
            PARTY_CHAT_CONTROL_HANDLE chatControl,
            out string entityId)
        {
            entityId = null;
            if (chatControl == null)
            {
                return PartyError.InvalidArg;
            }

            UTF8StringPtr stringPtr;
            UInt32 err = PFPInterop.PartyChatControlGetEntityId(
                chatControl.InteropHandle,
                out stringPtr);
            if (PartyError.SUCCEEDED(err))
            {
                entityId = stringPtr.GetString();
            }

            return err;
        }
    }
}
