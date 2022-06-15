namespace PlayFab.Multiplayer.Interop
{
    public static partial class Methods
    {
#if !UNITY_EDITOR && (MICROSOFT_GAME_CORE || UNITY_GAMECORE)
        const string ThunkDllName = @"PlayFabMultiplayerGDK.dll";
#else
        const string ThunkDllName = @"PlayFabMultiplayerWin.dll";
#endif
    }
}