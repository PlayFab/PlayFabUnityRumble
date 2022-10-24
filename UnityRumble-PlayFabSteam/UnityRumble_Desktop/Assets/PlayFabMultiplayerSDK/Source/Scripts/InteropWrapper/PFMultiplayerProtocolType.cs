namespace PlayFab.Multiplayer.InteropWrapper
{
    public enum PFMultiplayerProtocolType : uint
    {
        Tcp = Interop.PFMultiplayerProtocolType.Tcp,
        Udp = Interop.PFMultiplayerProtocolType.Udp
    }
}
