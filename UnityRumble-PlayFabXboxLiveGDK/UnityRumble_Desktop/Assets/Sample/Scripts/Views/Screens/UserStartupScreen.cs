//--------------------------------------------------------------------------------------
// UserStartupScreen.cs
//
// The UI screen class that handles the functionality for logging the user in.
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
using UnityEngine.Assertions;
using UnityEngine.UI;
using Custom_PlayFab;

public class UserStartupScreen : BaseUiScreen
{
    public event Action UserStartupCompleted;

    public Button StartButton;
    public GamerProfile GamerProfile;

    public void HandleStartButtonPressed()
    {
        Debug.LogFormat("UserStartupScreen.HandleStartButtonPressed()");

        Disable();
        // sign in to Xbox Live
        XboxLive.OnUserLoggedIn += HandleLiveUserSignedIn;
        XboxLive.OnUserLoginError += (error, hresult) =>
        {
            HandleError("Logging into Xbox Live failed: {0}", hresult.ToString("X8"));
        };
        AsyncOpUI.Started(@"Logging in to Xbox Live...");
        XboxLive.LoginLiveUser(true);
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        Assert.IsNotNull(StartButton);
        Assert.IsNotNull(GamerProfile);
    }

    protected override void OnShown()
    {
        GamerProfile.Hide();

        base.OnShown();
    }

    protected override void OnEnabled()
    {
        if (StartButton != null)
        {
            StartButton.interactable = true;
            StartButton.gameObject.SetActive(true);
        }
        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();
        StartButton.gameObject.SetActive(false);
        StartButton.interactable = false;
    }

    private void HandleLiveUserSignedIn()
    {
        Debug.LogFormat("UserStartupScreen.__HandleLiveUserSignedIn()");
        AsyncOpUI.Finished();
        XboxLive.ClearEventHandlers();
        // get the social manager started
        XboxLive.InitializeSocial();
        // get the Live user token
        XboxLive.OnUserTokenReceived += HandleUserTokenReceived;
        XboxLive.OnGeneralError += (message, hresult) => { HandleError("Getting the Xbox Live user token failed: {0}", hresult.ToString("X8")); };
        AsyncOpUI.Started(@"Requesting Xbox Live user token...");
        XboxLive.RequestLiveUserToken(false);
    }

    private void HandleUserTokenReceived()
    {
        Debug.LogFormat("UserStartupScreen.__HandleUserTokenReceived()");
        XboxLive.ClearEventHandlers();
        // login in to PlayFab
        PlayFabLoginManager.Instance.LoginSuccessEvent += HandlePlayFabLoginSteamSuccess;
        PlayFabLoginManager.Instance.LoginFailureEvent += HandlePlayFabLoginSteamFail;
        AsyncOpUI.Started(@"XBOX: Xbox login completed.");
        PlayFabLoginManager.Instance.LoginWithXbox(XboxLive.MyUserToken);
    }

    private void HandlePlayFabUserLoggedIn()
    {
        Debug.LogFormat("UserStartupScreen.__HandlePlayFabUserLoggedIn()");
        AsyncOpUI.Finished();
        XboxLive.ClearEventHandlers();
        GamerProfile.Show();
        UserStartupCompleted?.Invoke();
    }

    private void HandlePlayFabLoginSteamSuccess(string steamId)
    {
        AsyncOpUI.Finished();
        PlayFabLoginManager.Instance.LoginSuccessEvent -= HandlePlayFabLoginSteamSuccess;
        PlayFabLoginManager.Instance.LoginFailureEvent -= HandlePlayFabLoginSteamFail;
        HandlePlayFabUserLoggedIn();
    }

    private void HandlePlayFabLoginSteamFail(string msg)
    {
        PlayFabLoginManager.Instance.LoginSuccessEvent -= HandlePlayFabLoginSteamSuccess;
        PlayFabLoginManager.Instance.LoginFailureEvent -= HandlePlayFabLoginSteamFail;

        AsyncOpUI.Finished();

        AsyncOpUI.Started(@"Log in to PlayFab failed, please try again...");

        Show();
    }

    private void HandleError(string errorMessage, params object[] args)
    {
        AsyncOpUI.Finished();
        Debug.LogErrorFormat(errorMessage, args);
        StatusBarText.text = string.Format(errorMessage, args);
        Enable();
    }
}
