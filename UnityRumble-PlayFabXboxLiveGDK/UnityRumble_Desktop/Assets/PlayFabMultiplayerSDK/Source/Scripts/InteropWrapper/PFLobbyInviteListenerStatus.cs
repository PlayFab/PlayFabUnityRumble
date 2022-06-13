namespace PlayFab.Multiplayer.InteropWrapper
{
    public enum PFLobbyInviteListenerStatus : uint
    {
        NotListening = Interop.PFLobbyInviteListenerStatus.NotListening,
        Listening = Interop.PFLobbyInviteListenerStatus.Listening,
        NotAuthorized = Interop.PFLobbyInviteListenerStatus.NotAuthorized,
    }
}
