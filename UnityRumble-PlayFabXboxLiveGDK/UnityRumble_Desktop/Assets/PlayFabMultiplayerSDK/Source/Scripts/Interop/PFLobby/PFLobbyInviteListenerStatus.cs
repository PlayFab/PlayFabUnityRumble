namespace PlayFab.Multiplayer.Interop
{
    [Interop.NativeTypeName("uint32_t")]
    public enum PFLobbyInviteListenerStatus : uint
    {
        NotListening = 0,
        Listening = 1,
        NotAuthorized = 2,
    }
}
