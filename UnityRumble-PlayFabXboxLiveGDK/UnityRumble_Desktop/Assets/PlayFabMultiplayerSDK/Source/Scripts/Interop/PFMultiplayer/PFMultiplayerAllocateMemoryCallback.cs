using System;
using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void* PFMultiplayerAllocateMemoryCallback([NativeTypeName("size_t")] UIntPtr size, [NativeTypeName("uint32_t")] uint memoryTypeId);
}
