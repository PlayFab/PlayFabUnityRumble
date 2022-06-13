//--------------------------------------------------------------------------------------
// Main.cs
//
// Root level sample logic.
//
// MIT License
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
// 
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

using Custom_PlayFab;
using UnityEngine;
using UnityEngine.Assertions;
#if USE_MS_GAMECORE
using XGamingRuntime;
#elif USE_UNITY_GAMECORE
using Unity.GameCore;
#endif

public class Main : MonoBehaviour
{
    public enum MainState : int
    {
        Starting = 0,
        Running = 1,
        Suspending = 2,
        Suspended = 3,
        Resuming = 4,
    }
    public XboxLiveLogic XboxLive;
    public ScreenManager Screens;
    public GameController TheGameController;
    public TMPro.TMP_Text StatusText;

    public MainState CurrentState { get; private set; }

    private void OnValidate()
    {
        Assert.IsNotNull(Screens);
        Assert.IsNotNull(TheGameController);
        Assert.IsNotNull(StatusText);
    }

    void Awake()
    {
        PlayFabEnvInitializer.Instance.InitPlayFabEnv();
        PlayFabSessionNetwork.CreateInstance();
        // Show screens after all managers create their instances
        Screens.gameObject.SetActive(true);
    }

    void Start()
    {
        Debug.Log("Main.Start()");
        CurrentState = MainState.Starting;
        TheGameController.gameObject.SetActive(false);
        Configuration.InitializeConfiguration();
        var hresult = SDK.XGameRuntimeInitialize();
        if (hresult != 0)
        {
            var statusText = string.Format("XGameRuntimeInitialize() failed with hresult={0}", hresult.ToString("X8"));
            Debug.LogError(statusText);
            StatusText.text = statusText;
            return;
        }
        Screens.Initialize();
        Screens.UserStartupScreen.UserStartupCompleted += HandleUserStartupCompleted;
        Screens.MainMenuScreen.FindingGameCompleted += HandleFindGameCompleted;
        Screens.MainMenuScreen.FriendLobbiesObtained += HandleFriendLobbiesObtained;
        Screens.MainMenuScreen.JoinedLobbyCompleted += HandleJoiningGameCompleted;
        Screens.MainMenuScreen.RequestOpenLeaderboard += HandleRequestOpenLeaderboard;
        Screens.MainMenuScreen.Return += HandleMainMenuReturn;
        Screens.JoinFriendScreen.JoiningFriendCompleted += HandleJoiningFriendCompleted;
        Screens.JoinFriendScreen.JoiningFriendCancelled += HandleJoiningFriendCancelled;
        Screens.GameLobbyScreen.LeaveLobbyCompleted += HandleLeaveLobbyCompleted;
        Screens.GameLobbyScreen.TransitionToGameCompleted += HandleTransitionToGameCompleted;
        Screens.GamePlayScreen.GameOver += HandleGameOver;
        Screens.GamePlayScreen.QuitGameCompleted += HandleQuitGameCompleted;
        Screens.GamePlayScreen.QuitMatchCompleted += HandleQuitMatchCompleted;
        HandleResume(0);

#if UNITY_GAMECORE && !UNITY_STANDALONE && !UNITY_EDITOR
        UnityEngine.GameCore.GameCorePLM.OnResumingEvent += HandleResume;
        UnityEngine.GameCore.GameCorePLM.OnSuspendingEvent += HandleSuspend;
#endif
    }

    private void Update()
    {
        SDK.XTaskQueueDispatch();
        SessionNetwork.Instance?.Tick();
    }

    private void OnDestroy()
    {
        HandleSuspend();
        SDK.XGameRuntimeInitialize();
    }

    private void OnApplicationQuit()
    {
        PlayFabRuntimeInfos.Instance.OnApplicationQuit();
        PlayFabLobbyManager.Instance.RemoveEventListeners();
        PlayFabMatchmakingManager.Instance.RemoveEventListeners();
    }

    private void HandleResume(double secondsSuspended)
    {
        CurrentState = MainState.Resuming;
        XboxLive.Initialize();
        Screens.SwitchTo<UserStartupScreen>();
        CurrentState = MainState.Running;
    }

    private void HandleSuspend()
    {
        CurrentState = MainState.Suspending;
        Screens.SwitchTo<UserStartupScreen>();
        SessionNetwork.Instance.StopNetworking();
        PlayFabSessionNetwork.Instance.Cleanup();
        XboxLive.Cleanup();
        CurrentState = MainState.Suspended;
    }

    private void HandleUserStartupCompleted()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleFindGameCompleted()
    {
        Screens.SwitchTo<GameLobbyScreen>();
    }

    private void HandleHostingGameCompleted()
    {
        Screens.SwitchTo<GameLobbyScreen>();
    }

    private void HandleFriendLobbiesObtained()
    {
        Screens.SwitchTo<JoinFriendScreen>();
    }

    private void HandleMainMenuOpenLeaderBoardScreen()
    {
    }

    private void HandleJoiningGameCompleted()
    {
        Screens.SwitchTo<GameLobbyScreen>();
    }

    private void HandleRequestOpenLeaderboard()
    {
    }

    private void HandleMainMenuReturn()
    {
        Screens.SwitchTo<UserStartupScreen>();
    }

    private void HandleJoiningFriendCompleted()
    {
        Screens.SwitchTo<GameLobbyScreen>();
    }

    private void HandleJoiningFriendCancelled()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleLeaveLobbyCompleted()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleTransitionToGameCompleted()
    {
        Screens.SwitchTo<GamePlayScreen>();
    }

    private void HandleGameOver()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleQuitGameCompleted()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleQuitMatchCompleted()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleCloseLeaderBoardScreen()
    {
        Screens.SwitchTo<MainMenuScreen>();
    }

    private void HandleInviteJoinLobby()
    {
        Screens.SwitchTo<GameLobbyScreen>();
        Screens.UserStartupScreen.GamerProfile.Show();
    }
}
