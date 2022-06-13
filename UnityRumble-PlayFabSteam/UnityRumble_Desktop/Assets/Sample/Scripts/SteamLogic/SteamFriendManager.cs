//--------------------------------------------------------------------------------------
// SteamFriendManager.cs
//
// Maintains the status of the friends list based on their status.
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
using System.Collections.Generic;
using Steamworks;

public class SteamFriendData
{
    public CSteamID FriendSteamId;
    public string FriendName;
    public EPersonaState FriendState;
}

public class SteamFriendManager
{
    public event Action<ulong, EPersonaChange> FriendPersonalStateChangeEvent;

    private EFriendFlags findFriendFlag = EFriendFlags.k_EFriendFlagAll;

    private Callback<PersonaStateChange_t> OnSteamUserPersonalStateChangeCallback;

    private static SteamFriendManager instance;
    public static SteamFriendManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SteamFriendManager();
                instance.Initailize();
            }
            return instance;
        }
    }

    private SteamFriendManager()
    {
    }

    private void Initailize()
    {
        OnSteamUserPersonalStateChangeCallback = Callback<PersonaStateChange_t>.Create(OnSteamUserStateChange);
    }

    private void OnSteamUserStateChange(PersonaStateChange_t stateChange)
    {
        FriendPersonalStateChangeEvent?.Invoke(stateChange.m_ulSteamID, stateChange.m_nChangeFlags);
    }

    public void GetAllFriendDatas(out List<SteamFriendData> friendDataList)
    {
        friendDataList = new List<SteamFriendData>();
        for (int i = 0; i < SteamFriends.GetFriendCount(findFriendFlag); i++)
        {
            // if a friend is not subscribed to this game, continue
            if (!SteamApps.BIsSubscribedApp(SteamUtils.GetAppID()))
            {
                continue;
            }

            var friendSteamId = SteamFriends.GetFriendByIndex(i, findFriendFlag);
            var friendData = new SteamFriendData
            {
                FriendSteamId = friendSteamId,
                FriendName = SteamFriends.GetFriendPersonaName(friendSteamId),
                FriendState = SteamFriends.GetFriendPersonaState(friendSteamId)
            };

            friendDataList.Add(friendData);
        }
    }

    public SteamFriendData GetFriendData(ulong friendSteamId)
    {
        var steamId = new CSteamID(friendSteamId);

        var result = new SteamFriendData
        {
            FriendSteamId = steamId,
            FriendName = SteamFriends.GetFriendPersonaName(steamId),
            FriendState = SteamFriends.GetFriendPersonaState(steamId)
        };

        return result;
    }
}
