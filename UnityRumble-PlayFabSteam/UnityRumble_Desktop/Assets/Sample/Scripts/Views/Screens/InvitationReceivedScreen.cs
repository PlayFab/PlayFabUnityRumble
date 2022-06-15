//--------------------------------------------------------------------------------------
// InvitationReceivedScreen.cs
//
// Display this screen when player receives invitation of friends 
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
using UnityEngine.UI;
using Custom_PlayFab;
using TMPro;

public class InvitationReceivedScreen : MonoBehaviour
{
    public enum ButtonDisplayStyle
    {
        DISPLAY_NONE,
        DISPLAY_CONFIRM,
        DISPLAY_CANCEL,
        DISPLAY_BOTH
    }

    public TextMeshProUGUI MessageText;
    public Button ConfirmBtn;
    public Button CancelBtn;

    private Action<bool> ConfirmOrCancelAction;

    public event Action JoinInvitingLobbySuccess;

    void Start()
    {
        PlayFabInvitationManager.Instance.PlayerAcceptInvitationEvent += HandlePlayerAcceptInvitation;
        PlayFabInvitationManager.Instance.LoginRetryTimesEvent += HandleInvitationReceivedLoginRetryTimes;
        PlayFabInvitationManager.Instance.JoinInvitingLobbyFailureEvent += HandleInviteJoinLobbyFailure;
        ConfirmBtn.onClick.AddListener(OnConfirmBtnClick);
        CancelBtn.onClick.AddListener(OnCancelBtnClick);
    }

    private void OnConfirmBtnClick()
    {
        ConfirmOrCancelAction?.Invoke(true);
    }

    private void OnCancelBtnClick()
    {
        ConfirmOrCancelAction?.Invoke(false);
    }

    public void ShowInvatitionMessage(string message, ButtonDisplayStyle buttonStyle = ButtonDisplayStyle.DISPLAY_NONE, Action<bool> confirmOrCancelCallback = null)
    {
        Show();

        MessageText.text = message;
        ConfirmBtn.gameObject.SetActive((buttonStyle & ButtonDisplayStyle.DISPLAY_CONFIRM) > 0);
        CancelBtn.gameObject.SetActive((buttonStyle & ButtonDisplayStyle.DISPLAY_CANCEL) > 0);

        ConfirmOrCancelAction = confirmOrCancelCallback;
    }

    public void Show()
    {
        PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent += HandleInviteJoinLobbySuccess;
        PlayFabLobbyManager.Instance.OnLobbyJoinFailureEvent += HandleInviteJoinLobbyFailure;
        GetComponent<Canvas>().enabled = true;
    }

    public void Hide()
    {
        PlayFabLobbyManager.Instance.OnLobbyJoinCompletedEvent -= HandleInviteJoinLobbySuccess;
        PlayFabLobbyManager.Instance.OnLobbyJoinFailureEvent -= HandleInviteJoinLobbyFailure;
        ConfirmOrCancelAction = null;
        GetComponent<Canvas>().enabled = false;
    }

    private void HandlePlayerAcceptInvitation()
    {
        ShowInvatitionMessage("Accepted friend invitation, join lobby...");
    }

    private void HandleInvitationReceivedLoginRetryTimes(byte times)
    {
        ShowInvatitionMessage($"Login Failed {times}, retrying");
    }

    private void HandleInviteJoinLobbySuccess(string connectString, PlayFab.Multiplayer.Lobby lobby)
    {
        Debug.Log("Invitation to join a lobby success");
        JoinInvitingLobbySuccess?.Invoke();
        Hide();
    }

    private void HandleInviteJoinLobbyFailure(string error)
    {
        Debug.Log(error);
        Hide();
    }
}
