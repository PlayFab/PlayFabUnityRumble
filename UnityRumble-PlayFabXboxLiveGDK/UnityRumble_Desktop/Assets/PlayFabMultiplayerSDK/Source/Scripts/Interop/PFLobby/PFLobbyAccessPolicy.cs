namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFLobbyAccessPolicy : uint
    {
        Public = 0,
        Friends = 1,
        Private = 2,
    }
}
