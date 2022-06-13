namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFLobbyOwnerMigrationPolicy : uint
    {
        Automatic = 0,
        Manual = 1,
        None = 2,
        Server = 3,
    }
}
