namespace PlayFab.Multiplayer
{
    using System;
    using PlayFab.Multiplayer.InteropWrapper;

    public class LobbyError
    {
        public const int Success = 0x00000000;
        public const int InvalidArg = unchecked((int)0x80070057);

        // Generic test for success on any status value (non-negative numbers
        // indicate success).
        public static bool SUCCEEDED(int error)
        {
            return error >= 0;
        }

        public static bool FAILED(int error)
        {
            return !SUCCEEDED(error);
        }
    }
}
