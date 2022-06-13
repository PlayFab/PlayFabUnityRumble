using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PartyXBLCSharpSDK.Interop;
using PartyCSharpSDK;
using System.Text;

namespace PartyXBLCSharpSDK
{
    public partial class XBLSDK
    {
        private const UInt32 PartyErrorXblChatUserAlreadyExists = 0x5001;

        // For storing high frequency objects
        internal static ObjectPool objectPool;

        static XBLSDK()
        {
            objectPool = new ObjectPool();
            objectPool.AddEntry<List<PARTY_XBL_STATE_CHANGE>>(4, new Type[] { });
        }

        public static UInt32 PartyXblChatUserIsLocal(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out bool isLocal)
        {
            isLocal = false;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            Byte isLocalByte;
            UInt32 err = PartyXblInterop.PartyXblChatUserIsLocal(
                handle.InteropHandle,
                out isLocalByte);
            if (PartyError.SUCCEEDED(err))
            {
                isLocal = Convert.ToBoolean(isLocalByte);
            }

            return err;
        }

        public static UInt32 PartyXblChatUserGetXboxUserId(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out UInt64 xboxUserId)
        {
            xboxUserId = 0;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return PartyXblInterop.PartyXblChatUserGetXboxUserId(
                handle.InteropHandle,
                out xboxUserId);
        }

        public static UInt32 PartyXblChatUserSetCustomContext(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            Object customContext)
        {
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.SetCustomContext<Interop.PARTY_XBL_CHAT_USER_HANDLE>(
                PartyXblInterop.PartyXblChatUserGetCustomContext,
                PartyXblInterop.PartyXblChatUserSetCustomContext,
                handle.InteropHandle,
                customContext);
        }

        public static UInt32 PartyXblChatUserGetCustomContext(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out Object customContext)
        {
            if (handle == null)
            {
                customContext = null;
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetCustomContext<Interop.PARTY_XBL_CHAT_USER_HANDLE>(
                PartyXblInterop.PartyXblChatUserGetCustomContext,
                handle.InteropHandle,
                out customContext);
        }

        public static UInt32 PartyXblLocalChatUserGetAccessibilitySettings(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out PARTY_XBL_ACCESSIBILITY_SETTINGS settings)
        {
            settings = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_XBL_ACCESSIBILITY_SETTINGS interopStruct;
            UInt32 err = PartyXblInterop.PartyXblLocalChatUserGetAccessibilitySettings(
                handle.InteropHandle,
                out interopStruct);
            if (PartyError.SUCCEEDED(err))
            {
                settings = new PARTY_XBL_ACCESSIBILITY_SETTINGS(interopStruct);
            }

            return err;
        }

        public static UInt32 PartyXblLocalChatUserGetRequiredChatPermissionInfo(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            PARTY_XBL_CHAT_USER_HANDLE targetChaUser,
            out PARTY_XBL_CHAT_PERMISSION_INFO chatPermissionInfo)
        {
            chatPermissionInfo = null;
            if (handle == null || targetChaUser == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_XBL_CHAT_PERMISSION_INFO interopStruct;
            UInt32 err = PartyXblInterop.PartyXblLocalChatUserGetRequiredChatPermissionInfo(
                handle.InteropHandle,
                targetChaUser.InteropHandle,
                out interopStruct);
            if (PartyError.SUCCEEDED(err))
            {
                chatPermissionInfo = new PARTY_XBL_CHAT_PERMISSION_INFO(interopStruct);
            }

            return err;
        }

        public static UInt32 PartyXblLocalChatUserGetCrossNetworkCommunicationPrivacySetting(
            PARTY_XBL_CHAT_USER_HANDLE handle,
            out PARTY_XBL_CROSS_NETWORK_COMMUNICATION_PRIVACY_SETTING setting)
        {
            setting = PARTY_XBL_CROSS_NETWORK_COMMUNICATION_PRIVACY_SETTING.PARTY_XBL_CROSS_NETWORK_COMMUNICATION_PRIVACY_SETTING_ALLOWED;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return PartyXblInterop.PartyXblLocalChatUserGetCrossNetworkCommunicationPrivacySetting(
                handle.InteropHandle,
                out setting);
        }

        public static UInt32 PartyXblGetErrorMessage(
            UInt32 error,
            out string errorMessage)
        {
            UTF8StringPtr errorMessagePtr;
            UInt32 err = PartyXblInterop.PartyXblGetErrorMessage(
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

        public static UInt32 PartyXblSetThreadAffinityMask(
            PARTY_XBL_THREAD_ID threadId,
            UInt64 threadAffinityMask)
        {
            return PartyXblInterop.PartyXblSetThreadAffinityMask(threadId, threadAffinityMask);
        }

        public static UInt32 PartyXblGetThreadAffinityMask(
            PARTY_XBL_THREAD_ID threadId,
            out UInt64 threadAffinityMask)
        {
            return PartyXblInterop.PartyXblGetThreadAffinityMask(threadId, out threadAffinityMask);
        }

        public static UInt32 PartyXblInitialize(
            string titleId,
            out PARTY_XBL_HANDLE handle)
        {
            Interop.PARTY_XBL_HANDLE interopHandle;
            UInt32 err = PartyXblInterop.PartyXblInitialize(
                IntPtr.Zero,
                Converters.StringToNullTerminatedUTF8ByteArray(titleId),
                out interopHandle);
            return PARTY_XBL_HANDLE.WrapAndReturnError(err, interopHandle, out handle);
        }

        public static UInt32 PartyXblCleanup(
            PARTY_XBL_HANDLE handle)
        {
            return PartyXblInterop.PartyXblCleanup(handle.InteropHandle);
        }

        public static unsafe UInt32 PartyXblStartProcessingStateChanges(
            PARTY_XBL_HANDLE handle,
            out List<PARTY_XBL_STATE_CHANGE> stateChanges)
        {
            UInt32 err;
            stateChanges = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            UInt32 stateChangeCount;
            IntPtr rawStateChanges;
            stateChanges = objectPool.Retrieve<List<PARTY_XBL_STATE_CHANGE>>();
            err = PartyXblInterop.PartyXblStartProcessingStateChanges(handle.InteropHandle, out stateChangeCount, out rawStateChanges);
            if (PartyError.SUCCEEDED(err) && stateChangeCount > 0)
            {
                List<PARTY_XBL_STATE_CHANGE> unsupportStateChanges = null;
                IntPtr* arrayPtr = (IntPtr*)rawStateChanges.ToPointer();
                for (Int32 i = 0; i < stateChangeCount; i++)
                {
                    PARTY_XBL_STATE_CHANGE stateChangeObj = PARTY_XBL_STATE_CHANGE.CreateFromPtr(arrayPtr[i]);
                    if (stateChangeObj.GetType() != typeof(PARTY_XBL_STATE_CHANGE))
                    {
                        stateChanges.Add(stateChangeObj);
                    }
                    else
                    {
                        // Remove and immediately finish processing state changes that aren't supported by the
                        // CSharp wrapper, which will all be of the basic PARTY_STATE_CHANGE type
                        if (unsupportStateChanges == null)
                        {
                            unsupportStateChanges = objectPool.Retrieve<List<PARTY_XBL_STATE_CHANGE>>();
                        }

                        unsupportStateChanges.Add(stateChangeObj);
                    }
                }

                if (unsupportStateChanges != null)
                {
                    err = PartyXblFinishProcessingStateChanges(handle, unsupportStateChanges);
                }
            }

            return err;
        }

        public static unsafe UInt32 PartyXblFinishProcessingStateChanges(
            PARTY_XBL_HANDLE handle,
            List<PARTY_XBL_STATE_CHANGE> stateChanges)
        {
            UInt32 err;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            IntPtr* stateChangeIds = stackalloc IntPtr[stateChanges.Count];
            for (Int32 i = 0; i < stateChanges.Count; i++)
            {
                stateChangeIds[i] = stateChanges[i].StateChangeId;
            }

            stateChanges.Clear();
            objectPool.Return(stateChanges);

            err = PartyXblInterop.PartyXblFinishProcessingStateChanges(
                handle.InteropHandle,
                (UInt32)stateChanges.Count,
                new IntPtr(stateChangeIds));

            return err;
        }

        public static UInt32 PartyXblCreateLocalChatUser(
            PARTY_XBL_HANDLE handle,
            UInt64 xboxUserId,
            Object asyncIdentifier,
            out PARTY_XBL_CHAT_USER_HANDLE localXboxLiveUser)
        {
            localXboxLiveUser = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            Interop.PARTY_XBL_CHAT_USER_HANDLE interopHandle;
            UInt32 err = PartyXblInterop.PartyXblCreateLocalChatUser(
                handle.InteropHandle,
                xboxUserId,
                asyncId,
                out interopHandle);
            if (PartyError.SUCCEEDED(err))
            {
                localXboxLiveUser = new PARTY_XBL_CHAT_USER_HANDLE(interopHandle);
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

        public static UInt32 PartyXblCompleteGetTokenAndSignatureRequest(
            PARTY_XBL_HANDLE handle,
            UInt32 correlationId,
            bool succeeded,
            string token,
            string signature)
        {
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return PartyXblInterop.PartyXblCompleteGetTokenAndSignatureRequest(
                handle.InteropHandle,
                correlationId,
                Convert.ToByte(succeeded),
                Converters.StringToNullTerminatedUTF8ByteArray(token),
                Converters.StringToNullTerminatedUTF8ByteArray(signature));
        }

        public static UInt32 PartyXblCreateRemoteChatUser(
            PARTY_XBL_HANDLE handle,
            UInt64 xboxUserId,
            out PARTY_XBL_CHAT_USER_HANDLE chatUser)
        {
            chatUser = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            Interop.PARTY_XBL_CHAT_USER_HANDLE interopHandle;
            UInt32 err = PartyXblInterop.PartyXblCreateRemoteChatUser(
                handle.InteropHandle,
                xboxUserId,
                out interopHandle);
            if (err == PartyErrorXblChatUserAlreadyExists) // if remote chat user already exists, return its handle without an error
            {
                err = PartyError.Success;
            }
            return PARTY_XBL_CHAT_USER_HANDLE.WrapAndReturnError(err, interopHandle, out chatUser);
        }

        public static UInt32 PartyXblDestroyChatUser(
            PARTY_XBL_HANDLE handle,
            PARTY_XBL_CHAT_USER_HANDLE chatUser)
        {
            if (handle == null || chatUser == null)
            {
                return PartyError.InvalidArg;
            }

            return PartyXblInterop.PartyXblDestroyChatUser(handle.InteropHandle, chatUser.InteropHandle);
        }

        public static UInt32 PartyXblGetChatUsers(
            PARTY_XBL_HANDLE handle,
            out PARTY_XBL_CHAT_USER_HANDLE[] chatUsers)
        {
            chatUsers = null;
            if (handle == null)
            {
                return PartyError.InvalidArg;
            }

            return MarshalHelpers.GetArrayOfObjects<Interop.PARTY_XBL_HANDLE, Interop.PARTY_XBL_CHAT_USER_HANDLE, PARTY_XBL_CHAT_USER_HANDLE>(
                PartyXblInterop.PartyXblGetChatUsers,
                s => new PARTY_XBL_CHAT_USER_HANDLE(s),
                handle.InteropHandle,
                out chatUsers);
        }

        public static UInt32 PartyXblLoginToPlayFab(
            PARTY_XBL_CHAT_USER_HANDLE localChatUser,
            Object asyncIdentifier)
        {
            if (localChatUser == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PartyXblInterop.PartyXblLoginToPlayFab(
                localChatUser.InteropHandle,
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

        public static UInt32 PartyXblGetEntityIdsFromXboxLiveUserIds(
            PARTY_XBL_HANDLE handle,
            UInt64[] xboxLiveUserIds,
            PARTY_XBL_CHAT_USER_HANDLE localChatUser,
            Object asyncIdentifier)
        {
            if (handle == null || xboxLiveUserIds == null || localChatUser == null)
            {
                return PartyError.InvalidArg;
            }

            var asyncId = IntPtr.Zero;
            if (asyncIdentifier != null)
            {
                asyncId = GCHandle.ToIntPtr(GCHandle.Alloc(asyncIdentifier));
            }

            UInt32 err = PartyXblInterop.PartyXblGetEntityIdsFromXboxLiveUserIds(
                handle.InteropHandle,
                (UInt32)xboxLiveUserIds.Length,
                xboxLiveUserIds,
                localChatUser.InteropHandle,
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
    }
}
