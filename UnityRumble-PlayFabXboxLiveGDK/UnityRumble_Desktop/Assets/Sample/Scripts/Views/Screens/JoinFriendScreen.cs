//--------------------------------------------------------------------------------------
// JoinFriendScreen.cs
//
// The UI class for the join friend screen and functionality.
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class JoinFriendScreen : BaseUiScreen
{
    public event Action JoiningFriendCompleted;
    public event Action JoiningFriendCancelled;

    public Text NoInTitleFriendLobbiesText;
    public Button[] FriendGameButtons;
    public Button CancelButton;

    public void HandleFriendGameButtonPressed(int buttonIndex)
    {
        Debug.LogFormat("MainMenuScreen.HandleJoinInviteButtonPressed()");
    }

    public void HandleCancelButtonPressed()
    {
        JoiningFriendCancelled?.Invoke();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        Assert.IsNotNull(NoInTitleFriendLobbiesText);
        foreach (var friendGameButton in FriendGameButtons)
        {
            Assert.IsNotNull(friendGameButton);
        }
        Assert.IsNotNull(CancelButton);
    }

    protected override void OnShown()
    {
        base.OnShown();

        CancelButton.navigation = new Navigation() 
        { 
            mode = Navigation.Mode.Explicit,
        };
    }

    protected override void OnEnabled()
    {
        CancelButton.interactable = true;

        base.OnEnabled();
    }

    protected override void OnDisabled()
    {
        base.OnDisabled();

        foreach (var friendGameButton in FriendGameButtons)
        {
            friendGameButton.interactable = false;
        }
        CancelButton.interactable = false;
    }

    protected override void OnHidden()
    {
        base.OnHidden();
    }

    private void WaitForSessionNetworkId()
    {
    }

    private void HandleSessionPropertiesChanged(SessionDocument sessionDocument)
    {
    }

    private void HandleLobbyJoined()
    {
        Debug.LogFormat("JoinFriendScreen.__HandleSessionJoined()");

        AsyncOpUI.Finished();

        WaitForSessionNetworkId();
    }

    private void HandleNetworkStarted(string networkId)
    {
        Debug.LogFormat("JoinFriendScreen.__HandleNetworkStarted()");

        AsyncOpUI.Finished();
        Networking.OnNetworkStarted -= HandleNetworkStarted;

        JoiningFriendCompleted?.Invoke();
    }

    private void HandleError(string errorMessage, params object[] args)
    {
        AsyncOpUI.Finished();

        Debug.LogFormat(errorMessage, args);
        StatusBarText.text = string.Format(errorMessage, args);

        Enable();
    }
}
