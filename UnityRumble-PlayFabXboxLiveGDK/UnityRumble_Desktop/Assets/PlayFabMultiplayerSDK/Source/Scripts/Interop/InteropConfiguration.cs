namespace PlayFab.Multiplayer.Interop
{
    public static partial class Methods
    {
#if (UNITY_GAMECORE || MICROSOFT_GAME_CORE) && !UNITY_EDITOR
        const string ThunkDllName = @"PlayFabMultiplayerGDK.dll";
#elif UNITY_SWITCH && !UNITY_EDITOR
        const string ThunkDllName = "libPlayFabMultiplayer";
#elif UNITY_IOS && !UNITY_EDITOR
        const string ThunkDllName = "__Internal";
#elif UNITY_ANDROID && !UNITY_EDITOR
        const string ThunkDllName = "multiplayer";
#elif UNITY_PS4 && !UNITY_EDITOR
        const string ThunkDllName = "multiplayer.prx";
#elif UNITY_PS5 && !UNITY_EDITOR
        const string ThunkDllName = "multiplayer.prx";
#else
        const string ThunkDllName = @"PlayFabMultiplayerWin.dll";
#endif
    }
}
