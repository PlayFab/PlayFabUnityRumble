using System;
using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    public static unsafe partial class Methods
    {
        [NativeTypeName("const uint64_t")]
        public const ulong PFMultiplayerAnyProcessor = 0xffffffffffffffff;

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [return: NativeTypeName("const char *")]
        public static extern sbyte* PFMultiplayerGetErrorMessage(int error);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerSetMemoryCallbacks([NativeTypeName("PFMultiplayerAllocateMemoryCallback")] IntPtr allocateMemoryCallback, [NativeTypeName("PFMultiplayerFreeMemoryCallback")] IntPtr freeMemoryCallback);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerSetThreadAffinityMask(PFMultiplayerThreadId threadId, [NativeTypeName("uint64_t")] ulong threadAffinityMask);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerInitialize([NativeTypeName("const char *")] sbyte* playFabTitleId, [NativeTypeName("PFMultiplayerHandle *")] PFMultiplayer** handle);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerUninitialize([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle);

        [DllImport(ThunkDllName, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern int PFMultiplayerSetEntityToken([NativeTypeName("PFMultiplayerHandle")] PFMultiplayer* handle, [NativeTypeName("const PFEntityKey *")] PFEntityKey* entity, [NativeTypeName("const char *")] sbyte* entityToken);
    }
}
