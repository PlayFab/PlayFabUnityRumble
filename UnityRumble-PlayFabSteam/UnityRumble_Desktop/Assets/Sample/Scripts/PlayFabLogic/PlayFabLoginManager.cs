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
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using UnityEngine;

namespace Custom_PlayFab
{
    public sealed class PlayFabLoginManager
    {
        private const string UserPlatform = "Steam";

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

        private HAuthTicket hTicket;
        private string steamTicket;

        private PlayFabLoginManager()
        { }

        private void Initialize()
        {
            Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicket);
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

        public void LoginWithSteam()
        {
            steamTicket = GetSteamAuthTicket();
        }

        private string GetSteamAuthTicket()
        {
            byte[] ticketBlob = new byte[1024];
            uint ticketSize;

            // Retrieve ticket; hTicket should be a field in the class so you can use it to cancel the ticket later
            // When you pass an object, the object can be modified by the callee. This function modifies the byte array you've passed to it.
            hTicket = SteamUser.GetAuthSessionTicket(ticketBlob, ticketBlob.Length, out ticketSize);

            // Resize the buffer to actual length
            Array.Resize(ref ticketBlob, (int)ticketSize);

            // Convert bytes to string
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ticketBlob)
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }

        private void DoLoginWithSteam()
        {
            GetPlayerCombinedInfoRequestParams infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            infoRequestParams.GetCharacterInventories = true;
            infoRequestParams.GetUserAccountInfo = true;
            infoRequestParams.GetTitleData = true;

            var request = new LoginWithSteamRequest();
            request.InfoRequestParameters = infoRequestParams;
            request.TitleId = PlayFabSettings.staticSettings.TitleId;
            request.SteamTicket = steamTicket;
            request.CreateAccount = true;

            PlayFabClientAPI.LoginWithSteam(
                request,
                OnLoginSuccess,
                OnLoginFailure);
        }

        private void OnGetAuthSessionTicket(GetAuthSessionTicketResponse_t response)
        {
            if (response.m_eResult == EResult.k_EResultOK)
            {
                DoLoginWithSteam();
            }
            else
            {
                LoginFailureEvent?.Invoke(@"Get Steam Auth Ticket Failed");
            }
        }

        private void CancelSteamTicket()
        {
            steamTicket = string.Empty;
            SteamUser.CancelAuthTicket(hTicket);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            var playerName = SteamFriends.GetPersonaName();
            string playerInfo = playerName + ", " + UserPlatform;
            PlayFabRuntimeInfos.Instance.SetSelfPlayerInfo(playerInfo, result.AuthenticationContext);

            PlayFabEntityToken = result.EntityToken.EntityToken;

            LoginSuccessEvent?.Invoke(result.EntityToken.Entity.Id);

            CancelSteamTicket();
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogFormat("Error: {0}", error.ErrorMessage);
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
