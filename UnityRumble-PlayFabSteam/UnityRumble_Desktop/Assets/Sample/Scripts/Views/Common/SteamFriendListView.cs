//--------------------------------------------------------------------------------------
// SteamFriendList.cs
//
// Display steam friend list in GameLobbyScene
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Steamworks;
using Custom_PlayFab;

public class SteamFriendListView : MonoBehaviour
{
    public Transform FriendListItemParent;
    public Button CloseBtn;
    public GameObject FriendListItem;

    private void Start()
    {
        CloseBtn.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        List<SteamFriendData> friendDataList;
        SteamFriendManager.Instance.GetAllFriendDatas(out friendDataList);
        foreach (var item in friendDataList)
        {
            var friendItem = Instantiate(FriendListItem, FriendListItemParent);
            var itemBehaviour = friendItem.GetComponent<SteamFriendListItem>();

            itemBehaviour.SetFriendData(item);
            itemBehaviour.InviteFriendAction += OnFriendItemInviteBtnClick;
        }
    }

    private void OnDisable()
    {
        ClearFriendItems();
    }

    private void OnFriendItemInviteBtnClick(CSteamID friendSteamId)
    {
        PlayFabLobbyManager.Instance.HandleInviteFriend(friendSteamId);
    }

    public void ClearFriendItems()
    {
        foreach (Transform item in FriendListItemParent)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    private void OnValidate()
    {
        Assert.IsNotNull(FriendListItemParent);
        Assert.IsNotNull(CloseBtn);
    }
}
