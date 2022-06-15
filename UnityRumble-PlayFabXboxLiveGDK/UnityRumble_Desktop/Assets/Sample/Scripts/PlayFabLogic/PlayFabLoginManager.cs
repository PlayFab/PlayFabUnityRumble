//--------------------------------------------------------------------------------------
// PlayFabLoginManager.cs
//
// Manage messages and functions of Loging into PlayFab.
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
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Custom_PlayFab
{
    public sealed class PlayFabLoginManager
    {
        public const string UserPlatform = "Xbox";

        private static PlayFabLoginManager instance;
        public static PlayFabLoginManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabLoginManager();
                    instance.Initialize();
                }

                return instance;
            }
        }

        public string PlayFabEntityToken { get; private set; }

        public event Action<string> LoginSuccessEvent;
        public event Action<string> LoginFailureEvent;
        public event Action<string> RegisterSuccessEvent;
        public event Action<string> RegisterFailureEvent;

        private PlayFabLoginManager()
        { }

        private void Initialize()
        {

        }

        public void LoginWithPlayFab(string userName, string password)
        {
            GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            infoRequestParams.GetCharacterInventories = true;
            infoRequestParams.GetUserAccountInfo = true;
            infoRequestParams.GetTitleData = true;

            var request = new LoginWithPlayFabRequest();
            request.InfoRequestParameters = infoRequestParams;

            request.TitleId = PlayFabSettings.staticSettings.TitleId;
            request.Username = userName;
            request.Password = password;

            PlayFabClientAPI.LoginWithPlayFab(
                request,
                OnLoginSuccess,
                OnLoginFailure);
        }

        public void LoginWithEmailAddress(string emailAddress, string password)
        {
            GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            infoRequestParams.GetCharacterInventories = true;
            infoRequestParams.GetUserAccountInfo = true;
            infoRequestParams.GetTitleData = true;

            var request = new LoginWithEmailAddressRequest();
            request.InfoRequestParameters = infoRequestParams;

            request.TitleId = PlayFabSettings.staticSettings.TitleId;
            request.Email = emailAddress;
            request.Password = password;

            PlayFabClientAPI.LoginWithEmailAddress(
                request,
                OnLoginSuccess,
                OnLoginFailure);
        }

        public void LoginWithCustomId()
        {
            GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            infoRequestParams.GetCharacterInventories = true;
            infoRequestParams.GetUserAccountInfo = true;
            infoRequestParams.GetTitleData = true;

            var request = new LoginWithCustomIDRequest();
            request.InfoRequestParameters = infoRequestParams;
            request.TitleId = PlayFabSettings.staticSettings.TitleId;
            request.CustomId = SystemInfo.deviceUniqueIdentifier;
            request.CreateAccount = true;

            PlayFabClientAPI.LoginWithCustomID(
                request,
                OnLoginSuccess,
                OnLoginFailure);
        }

        public void LoginWithXbox(string xboxToken)
        {
            GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            infoRequestParams.GetCharacterInventories = true;
            infoRequestParams.GetUserAccountInfo = true;
            infoRequestParams.GetTitleData = true;
            var request = new LoginWithXboxRequest();
            request.InfoRequestParameters = infoRequestParams;
            request.TitleId = PlayFabSettings.staticSettings.TitleId;
            request.XboxToken = xboxToken;
            request.CreateAccount = true;
            PlayFabClientAPI.LoginWithXbox(
                request,
                OnLoginSuccess,
                OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            var playerName = result.PlayFabId;
            PlayFabRuntimeInfos.Instance.SetSelfPlayerInfo(playerName, result.AuthenticationContext);

            PlayFabEntityToken = result.EntityToken.EntityToken;

            LoginSuccessEvent?.Invoke(result.EntityToken.Entity.Id);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            LoginFailureEvent?.Invoke(error.ErrorMessage);
        }

        public void RegisterPlayFabUser(string userName, string emailAddr, string password)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
            request.Email = emailAddr;
            request.Username = userName;
            request.Password = password;

            PlayFabClientAPI.RegisterPlayFabUser(
                request,
                (registerResult) =>
                {
                    RegisterSuccessEvent?.Invoke(registerResult.PlayFabId);
                },
                (error) =>
                {
                    RegisterFailureEvent?.Invoke(error.ErrorMessage);
                });
        }
    }
}
