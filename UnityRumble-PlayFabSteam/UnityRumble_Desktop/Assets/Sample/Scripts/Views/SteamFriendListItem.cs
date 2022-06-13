//--------------------------------------------------------------------------------------
// SteamFriendListItem.cs
//
// Item in steam friend list view of GameLobbyScreen
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
using Steamworks;
using Custom_PlayFab;

public class SteamFriendListItem : MonoBehaviour
{
    public Action<CSteamID> InviteFriendAction;

    public RawImage FriendHeadImage;
    public Text FriendNameText;
    public Text FriendStateText;
    public Button InviteBtn;

    private CSteamID friendSteamId;
    private string friendName;
    private EPersonaState friendState;

    private void Start()
    {
        InviteBtn.onClick.AddListener(OnInviteBtnClick);
    }

    private void OnEnable()
    {
        SteamFriendManager.Instance.FriendPersonalStateChangeEvent += HandleFriendStateUpdate;
        PlayFabRuntimeInfos.Instance.PlayerInfoUpdatedEvent += UpdateInviteBtnState;
    }

    private void OnDisable()
    {
        SteamFriendManager.Instance.FriendPersonalStateChangeEvent -= HandleFriendStateUpdate;
        PlayFabRuntimeInfos.Instance.PlayerInfoUpdatedEvent -= UpdateInviteBtnState;
    }

    private void HandleFriendStateUpdate(ulong changedFriendSteamId, EPersonaChange change)
    {
        if (friendSteamId.m_SteamID == changedFriendSteamId)
        {
            var data = SteamFriendManager.Instance.GetFriendData(changedFriendSteamId);

            UpdateInviteBtnStateWithSteamState(data);

            if (change == EPersonaChange.k_EPersonaChangeAvatar)
            {
                SteamAvatarDownloader.Instance.GetPlayerAvatar(data.FriendSteamId, true,
                    (tex) =>
                    {
                        FriendHeadImage.texture = tex;
                    },
                    (msg) =>
                    {
                        Debug.LogError(msg);
                    });
            }
        }
    }

    private void OnInviteBtnClick()
    {
        InviteFriendAction?.Invoke(friendSteamId);
    }

    public void SetFriendData(SteamFriendData data)
    {
        UpdateInviteBtnStateWithSteamState(data);

        SteamAvatarDownloader.Instance.GetPlayerAvatar(data.FriendSteamId, false,
            (tex) =>
            {
                FriendHeadImage.texture = tex;
            },
            (msg) =>
            {
                Debug.LogError(msg);
            });
    }

    private void UpdateInviteBtnStateWithSteamState(SteamFriendData data)
    {
        friendSteamId = data.FriendSteamId;
        friendName = data.FriendName;
        friendState = data.FriendState;

        FriendNameText.text = friendName;
        FriendStateText.text = data.FriendState.ToString().Replace("k_EPersonaState", string.Empty);

        UpdateInviteBtnState();
    }

    private void UpdateInviteBtnState()
    {
        bool isFriendInLobby = PlayFabRuntimeInfos.Instance.IsPlayerInLobbyOrGame(friendName);
        bool isReachMaxCount = PlayFabRuntimeInfos.Instance.IsMaxPlayerReached();

        InviteBtn.interactable = (friendState == EPersonaState.k_EPersonaStateOnline && !isReachMaxCount && !isFriendInLobby);
    }
}
