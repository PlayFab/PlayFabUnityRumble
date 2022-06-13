using System.Runtime.InteropServices;

namespace PlayFab.Multiplayer.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void PFMultiplayerFreeMemoryCallback(void* pointer, [NativeTypeName("uint32_t")] uint memoryTypeId);
}
