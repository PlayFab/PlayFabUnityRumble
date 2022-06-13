//--------------------------------------------------------------------------------------
// LobbyMembersView.cs
//
// The session members list for the members currently connected to a game lobby.
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
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class LobbyMembersView : BaseMembersView
{
    public event Action<bool> AllMembersReady;

    [SerializeField]
    private LobbyMemberView LobbyMemberViewPrefab;

    public void SetMemberShipIndex(ulong xuid, GameAssetManager assetManager, int shipIndex)
    {
        BaseMemberView existingMember;
        if (_members.TryGetValue(xuid, out existingMember))
        {
            (existingMember as LobbyMemberView).SetShipImage(assetManager, shipIndex);
        }
    }

    public void SetMemberColorIndex(ulong xuid, GameAssetManager assetManager, int colorIndex)
    {
        BaseMemberView existingMember;
        if (_members.TryGetValue(xuid, out existingMember))
        {
            (existingMember as LobbyMemberView).SetShipColor(assetManager, colorIndex);
        }
    }

    public void MakeMemberReady(ulong xuid)
    {
        BaseMemberView existingMember;
        if (_members.TryGetValue(xuid, out existingMember))
        {
            (existingMember as LobbyMemberView).MakeReady();

            bool isAllReady = _members.Count >= MinimumRequiredMembers &&
                _members.All(member =>
                {
                    var lobbyMemberView = member.Value as LobbyMemberView;
                    return lobbyMemberView.IsReady();
                });
            
            AllMembersReady?.Invoke(isAllReady);
        }
    }

    public void MakeMemberNotReady(ulong xuid)
    {
        BaseMemberView existingMember;
        if (_members.TryGetValue(xuid, out existingMember))
        {
            (existingMember as LobbyMemberView).MakeNotReady();
            AllMembersReady?.Invoke(false);
        }
    }

    protected override BaseMemberView MakeMemberView()
    {
        var newMember = Instantiate<LobbyMemberView>(LobbyMemberViewPrefab, MembersList);
        newMember.MakeNotReady();
        return newMember;
    }

    protected override void OnValidate()
    {
        Assert.IsNotNull(LobbyMemberViewPrefab, $"Make sure to set lobby member view prefab for {name} object.");
    }
}
