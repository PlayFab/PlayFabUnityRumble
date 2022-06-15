//--------------------------------------------------------------------------------------
// PlayFabInvitationManager.cs
//
// Manage invitation from friend.
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
using System;
using UnityEngine;
using Steamworks;
using PlayFab.Multiplayer;

namespace Custom_PlayFab
{
    public class PlayFabInvitationManager
    {
        // Event triggered when the player accept invitation
        public event Action PlayerAcceptInvitationEvent;
        // Event triggered when the player accept invitation to login failure
        public event Action<byte> LoginRetryTimesEvent;
        // Event triggered when the player accept invitation and join invition lobby failure
        public event Action<string> JoinInvitingLobbyFailureEvent;
        // Event triggered when the player accept invitation and join invition lobby completed
        public event Action JoinInvitingLobbyCompletedEvent;
        private const byte LOGIN_RETRY_TIMES = 3;
        private byte curLoginTimes = 0;

        private Callback<GameRichPresenceJoinRequested_t> gameRichPresenceJoinRequestCallback;
        private string savedInvitingConnectString;
        private const int connectStringLength = 256;

        private static PlayFabInvitationManager instance;
        public static PlayFabInvitationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabInvitationManager();
                    instance.Initialize();
                }
                return instance;
            }
        }

        private PlayFabInvitationManager()
        {
        }

        private void Initialize()
        {
            gameRichPresenceJoinRequestCallback = Callback<GameRichPresenceJoinRequested_t>.Create(OnReceiveJoinRequest);
            string connectStringFromCommandline;
            SteamApps.GetLaunchCommandLine(out connectStringFromCommandline, connectStringLength);
            if (!string.IsNullOrEmpty(connectStringFromCommandline))
            {
                Debug.Log($"Received an invitation in launch command line, connect string is: {connectStringFromCommandline}");
                LoginAndJoinLobby(connectStringFromCommandline);
            }
        }

        private void OnReceiveJoinRequest(GameRichPresenceJoinRequested_t request)
        {
            LoginAndJoinLobby(request.m_rgchConnect);
        }

        private void LoginAndJoinLobby(string connectString)
        {
            PlayerAcceptInvitationEvent?.Invoke();
            if (PlayFabRuntimeInfos.Instance.MyEntityContext == null)
            {
                PlayFabLoginManager.Instance.LoginSuccessEvent += OnLoginSuccess;
                PlayFabLoginManager.Instance.LoginFailureEvent += OnLoginFailure;

                savedInvitingConnectString = connectString;

                PlayFabLoginManager.Instance.LoginWithSteam();

                return;
            }
            PlayFabLobbyManager.Instance.JoinLobby(connectString);
        }

        private void OnLoginSuccess(string steamId)
        {
            curLoginTimes = 0;
            PlayFabLoginManager.Instance.LoginSuccessEvent -= OnLoginSuccess;
            PlayFabLoginManager.Instance.LoginFailureEvent -= OnLoginFailure;
            PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent += OnLobbyJoinCompltetd;
            PlayFabLobbyManager.Instance.JoinLobby(savedInvitingConnectString);
        }

        private void OnLoginFailure(string msg)
        {
            if (curLoginTimes < LOGIN_RETRY_TIMES)
            {
                PlayFabLoginManager.Instance.LoginWithSteam();
                curLoginTimes++;
                LoginRetryTimesEvent?.Invoke(curLoginTimes);
            }
            else
            {
                PlayFabLoginManager.Instance.LoginSuccessEvent -= OnLoginSuccess;
                PlayFabLoginManager.Instance.LoginFailureEvent -= OnLoginFailure;
                JoinInvitingLobbyFailureEvent?.Invoke("Invitation to join lobby failed");
            }
        }

        private void OnLobbyJoinCompltetd(string connectionString, Lobby lobby)
        {
            PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent -= OnLobbyJoinCompltetd;
            JoinInvitingLobbyCompletedEvent?.Invoke();
        }
    }
}
