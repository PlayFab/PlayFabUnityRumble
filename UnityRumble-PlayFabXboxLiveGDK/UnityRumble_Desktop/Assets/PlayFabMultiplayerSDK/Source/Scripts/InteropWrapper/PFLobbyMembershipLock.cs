namespace PlayFab.Multiplayer.InteropWrapper
{
    /// <summary>
    /// Values representing the state of the lobby's membership lock.
    /// </summary>
    public enum PFLobbyMembershipLock : uint
    {
        /// <summary>
        /// Lobby membership is unlocked. New members will not be prevented from joining.
        /// </summary>
        Unlocked = Interop.PFLobbyMembershipLock.Unlocked,

        /// <summary>
        /// Lobby membership is locked. New members will be prevented from joining.
        /// </summary>
        Locked = Interop.PFLobbyMembershipLock.Locked,
    }
}
