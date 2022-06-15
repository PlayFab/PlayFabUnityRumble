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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    public ScreenManager Screens;
    public GameController TheGameController;
    public TMPro.TMP_Text StatusText;
    public PopupView PopupView;

    public MainState CurrentState { get; private set; }

    private void OnValidate()
    {
        Assert.IsNotNull(Screens);
        Assert.IsNotNull(TheGameController);
        Assert.IsNotNull(StatusText);
    }

    void Awake()
    {

#if STEAMWORKS_NET
        SteamSessionNetwork.CreateInstance();
        SteamVoiceChatManager.CreateInstance();
        SteamLobbyManager.CreateInstance();
        SteamProfileManager.CreateInstance();
        SteamLeaderboardManager.CreateInstance();
        SteamAchievementManager.CreateInstance();
        SteamInventoryManager.CreateInstance();
#endif
        Screens.gameObject.SetActive(true);
    }

    void Start()
    {
        Debug.Log("Main.Start()");

#if STEAMWORKS_NET
        if (!SteamManager.Initialized)
        {
            Debug.Log("SteamManager Initialize failed");
            Application.Quit();
        }
        else
        {
#endif

            CurrentState = MainState.Starting;

            TheGameController.gameObject.SetActive(false);

            Screens.Initialize();

            LobbyManager.Instance.OnJoinedLobbyAction += HandleInviteJoinLobby;
            LobbyManager.Instance.OnInvitedLobbyMemberMaxEvent += HandleInviteLobbyMemberMax;
            Screens.UserStartupScreen.UserStartupCompleted += HandleUserStartupCompleted;
            Screens.MainMenuScreen.FindingGameCompleted += HandleFindGameCompleted;
            Screens.MainMenuScreen.HostingGameCompleted += HandleHostingGameCompleted;
            Screens.MainMenuScreen.JoiningGameCompleted += HandleJoiningGameCompleted;
            Screens.MainMenuScreen.FindingGameCancel += HandleFindingGameCancel;
            Screens.MainMenuScreen.FriendLobbiesObtained += HandleFriendLobbiesObtained;
            Screens.MainMenuScreen.OpenLeaderBoardScreen += HandleMainMenuOpenLeaderBoardScreen;
            Screens.JoinFriendScreen.JoiningFriendCompleted += HandleJoiningFriendCompleted;
            Screens.JoinFriendScreen.JoiningFriendCancelled += HandleJoiningFriendCancelled;
            Screens.GameLobbyScreen.LeaveLobbyCompleted += HandleLeaveLobbyCompleted;
            Screens.GameLobbyScreen.TransitionToGameCompleted += HandleTransitionToGameCompleted;
            Screens.GamePlayScreen.GameOver += HandleGameOver;
            Screens.GamePlayScreen.QuitGameCompleted += HandleQuitGameCompleted;
            Screens.GamePlayScreen.QuitMatchCompleted += HandleQuitMatchCompleted;
            Screens.LeaderboardScreen.CloseLeaderBoardScreenEvent += HandleCloseLeaderBoardScreen;

            HandleResume(0);

#if UNITY_GAMECORE && !UNITY_STANDALONE && !UNITY_EDITOR
        UnityEngine.GameCore.GameCorePLM.OnResumingEvent += HandleResume;
        UnityEngine.GameCore.GameCorePLM.OnSuspendingEvent += HandleSuspend;
#endif

#if STEAMWORKS_NET
        }
#endif
    }

    private void Update()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        SessionNetwork.Instance?.Tick();
        VoiceChatManager.Instance?.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        HandleSuspend();
    }

    private void HandleResume(double secondsSuspended)
    {
        CurrentState = MainState.Resuming;

        Screens.SwitchTo<UserStartupScreen>();

        CurrentState = MainState.Running;
    }

    private void HandleSuspend()
    {
        CurrentState = MainState.Suspending;

        SessionNetwork.Instance.StopNetworking();

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
        Screens.SwitchTo<LeaderboardScreen>();
    }

    private void HandleJoiningGameCompleted()
    {
        Screens.SwitchTo<GameLobbyScreen>();
    }

    private void HandleFindingGameCancel()
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

    private void HandleInviteLobbyMemberMax()
    {
        PopupView.ShowCannotJoinedMessage("Join lobby failed!");
    }
}
