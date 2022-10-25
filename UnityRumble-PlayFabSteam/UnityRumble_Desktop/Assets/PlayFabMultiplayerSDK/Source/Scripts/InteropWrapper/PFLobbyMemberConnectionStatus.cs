namespace PlayFab.Multiplayer.InteropWrapper
{
    public enum PFLobbyMemberConnectionStatus : uint
    {
        NotConnected = Interop.PFLobbyMemberConnectionStatus.NotConnected,
        Connected = Interop.PFLobbyMemberConnectionStatus.Connected
    }
}
