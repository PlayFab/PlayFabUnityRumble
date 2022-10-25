using System;
using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    public static unsafe partial class Methods
    {
        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyMaxMemberCountLowerLimit = 2;

        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyMaxMemberCountUpperLimit = 128;

        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyMaxSearchPropertyCount = 30;

        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyMaxLobbyPropertyCount = 30;

        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyMaxMemberPropertyCount = 30;

        [NativeTypeName("const uint32_t")]
        public const uint PFLobbyClientRequestedSearchResultCountUpperLimit = 50;

#if !UNITY_2017_1_OR_NEWER
        [NativeTypeName("const char [18]")]
        public static ReadOnlySpan<byte> PFLobbyMemberCountSearchKey => new byte[] { 0x6C, 0x6F, 0x62, 0x62, 0x79, 0x2F, 0x6D, 0x65, 0x6D, 0x62, 0x65, 0x72, 0x43, 0x6F, 0x75, 0x6E, 0x74, 0x00 };
#endif

#if !UNITY_2017_1_OR_NEWER
        [NativeTypeName("const char [15]")]
        public static ReadOnlySpan<byte> PFLobbyAmMemberSearchKey => new byte[] { 0x6C, 0x6F, 0x62, 0x62, 0x79, 0x2F, 0x61, 0x6D, 0x4D, 0x65, 0x6D, 0x62, 0x65, 0x72, 0x00 };
#endif

#if !UNITY_2017_1_OR_NEWER
        [NativeTypeName("const char [14]")]
        public static ReadOnlySpan<byte> PFLobbyAmOwnerSearchKey => new byte[] { 0x6C, 0x6F, 0x62, 0x62, 0x79, 0x2F, 0x61, 0x6D, 0x4F, 0x77, 0x6E, 0x65, 0x72, 0x00 };
#endif

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetLobbyId([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const char **")] sbyte** id);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMaxMemberCount([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("uint32_t *")] uint* maxMemberCount);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetOwner([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey **")] PFEntityKey** owner);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetOwnerMigrationPolicy([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, PFLobbyOwnerMigrationPolicy* ownerMigrationPolicy);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetAccessPolicy([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, PFLobbyAccessPolicy* accessPolicy);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMembershipLock([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, PFLobbyMembershipLock* lockState);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetConnectionString([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const char **")] sbyte** connectionString);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMembers([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("uint32_t *")] uint* memberCount, [NativeTypeName("const PFEntityKey **")] PFEntityKey** members);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyAddMember([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* localUser, [NativeTypeName("uint32_t")] uint memberPropertyCount, [NativeTypeName("const char *const *")] sbyte** memberPropertyKeys, [NativeTypeName("const char *const *")] sbyte** memberPropertyValues, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyForceRemoveMember([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* targetMember, [NativeTypeName("bool")] byte preventRejoin, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyLeave([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* localUser, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetSearchPropertyKeys([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("uint32_t *")] uint* propertyCount, [NativeTypeName("const char *const **")] sbyte*** keys);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetSearchProperty([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const char *")] sbyte* key, [NativeTypeName("const char **")] sbyte** value);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetLobbyPropertyKeys([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("uint32_t *")] uint* propertyCount, [NativeTypeName("const char *const **")] sbyte*** keys);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetLobbyProperty([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const char *")] sbyte* key, [NativeTypeName("const char **")] sbyte** value);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMemberPropertyKeys([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* member, [NativeTypeName("uint32_t *")] uint* propertyCount, [NativeTypeName("const char *const **")] sbyte*** keys);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMemberProperty([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* member, [NativeTypeName("const char *")] sbyte* key, [NativeTypeName("const char **")] sbyte** value);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetMemberConnectionStatus([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* member, PFLobbyMemberConnectionStatus* connectionStatus);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyPostUpdate([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* localUser, [NativeTypeName("const PFLobbyDataUpdate *")] PFLobbyDataUpdate* lobbyUpdate, [NativeTypeName("const PFLobbyMemberDataUpdate *")] PFLobbyMemberDataUpdate* memberUpdate, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbySendInvite([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, [NativeTypeName("const PFEntityKey *")] PFEntityKey* sender, [NativeTypeName("const PFEntityKey *")] PFEntityKey* invitee, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbyGetCustomContext([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, void** customContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFLobbySetCustomContext([NativeTypeName("PFLobbyHandle")] PFLobby* lobby, void* customContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerStartProcessingLobbyStateChanges([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t *")] uint* stateChangeCount, [NativeTypeName("const PFLobbyStateChange *const **")] PFLobbyStateChange*** stateChanges);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerFinishProcessingLobbyStateChanges([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("uint32_t")] uint stateChangeCount, [NativeTypeName("const PFLobbyStateChange *const *")] PFLobbyStateChange** stateChanges);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerCreateAndJoinLobby([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* creator, [NativeTypeName("const PFLobbyCreateConfiguration *")] PFLobbyCreateConfiguration* createConfiguration, [NativeTypeName("const PFLobbyJoinConfiguration *")] PFLobbyJoinConfiguration* joinConfiguration, void* asyncContext, [NativeTypeName("PFLobbyHandle *")] PFLobby** lobby);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerJoinLobby([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* newMember, [NativeTypeName("const char *")] sbyte* connectionString, [NativeTypeName("const PFLobbyJoinConfiguration *")] PFLobbyJoinConfiguration* configuration, void* asyncContext, [NativeTypeName("PFLobbyHandle *")] PFLobby** lobby);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerJoinArrangedLobby([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* newMember, [NativeTypeName("const char *")] sbyte* arrangementString, [NativeTypeName("const PFLobbyArrangedJoinConfiguration *")] PFLobbyArrangedJoinConfiguration* configuration, void* asyncContext, [NativeTypeName("PFLobbyHandle *")] PFLobby** lobby);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerFindLobbies([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* searchingEntity, [NativeTypeName("const PFLobbySearchConfiguration *")] PFLobbySearchConfiguration* searchConfiguration, void* asyncContext);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerStartListeningForLobbyInvites([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* listeningEntity);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerStopListeningForLobbyInvites([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* listeningEntity);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerGetLobbyInviteListenerStatus([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* listeningEntity, PFLobbyInviteListenerStatus* status);
    }
}
