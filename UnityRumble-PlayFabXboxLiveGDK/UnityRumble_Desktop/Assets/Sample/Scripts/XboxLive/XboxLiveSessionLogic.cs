//--------------------------------------------------------------------------------------
// XboxLiveSessionLogic.cs
//
// Xbox Live logic for handling MPM user lobby and game session state and properties.
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
// Advanced Technology Group (ATG)
// Copyright (C) Microsoft Corporation. All rights reserved.
//--------------------------------------------------------------------------------------

using System;
using UnityEngine;
using System.Linq;
using UnityEngine.Assertions;

#if USE_MS_GAMECORE
using XGamingRuntime;
using HR = XGamingRuntime.Interop.HR;
#elif USE_UNITY_GAMECORE
using Unity.GameCore;
#endif

public partial class XboxLiveLogic
{
    public enum SessionState : int
    {
        NoSession = 0,
        InHostedLobby = 1,
        InHostedGame = 2,
        InMatchedLobby = 3,
        InMatchedGame = 4,
    }

    public event System.Action<SessionDocument> OnSessionPropertiesChanged;

    public SessionDocument CurrentSessionDocument { get; private set; }
    public SessionState CurrentSessionState { get; private set; }

    public void CreateLobby()
    {
        Debug.LogFormat("XboxLiveLogic.CreateLobby()");

        // make sure that an invite is required, or the user is a friend of the creator, to join the sessions
        // NOTE: "JoinableByFriends" is actually the default so no call is needed
        var hresult = HR.S_OK;// SDK.XBL.XblMultiplayerManagerSetJoinability(XblMultiplayerJoinability.JoinableByFriends, this);

        // NOTE: On Unity, this can fail with an error of 0x8000FFFF which just means we could not override
        // the joinability from the session lobby session template specified in the initialization.

        if (HR.FAILED(hresult))
        {
            Debug.LogErrorFormat("XblMultiplayerManagerSetJoinability() failed with hresult = {0}.", hresult.ToString("X8"));
        }

        // When hosting a game we first join the local user to MPM and wait for the UserAdded event
        hresult = SDK.XBL.XblMultiplayerManagerLobbySessionAddLocalUser(MyUserHandle);

        if (HR.FAILED(hresult))
        {
            Debug.LogErrorFormat("XblMultiplayerManagerLobbySessionAddLocalUser() failed with hresult = {0}.", hresult.ToString("X8"));
            OnMultiplayerError?.Invoke("XblMultiplayerManagerLobbySessionAddLocalUser() failed", hresult);
            return;
        }

        IsHost = true;
        HostXuid = MyXUID;
    }

    public void SetSessionProperties(SessionDocument sessionDocument)
    {
        Debug.LogFormat("XboxLiveLogic.SetSessionProperties({0})", JsonUtility.ToJson(sessionDocument));

        Assert.IsTrue(IsHost);
        Assert.IsTrue(sessionDocument.HostMemberID == MyXUID);

        CurrentSessionDocument = sessionDocument;
        HostXuid = CurrentSessionDocument.HostMemberID;

        var sessionDocumentAsJson = JsonUtility.ToJson(sessionDocument);
        Debug.LogFormat(">>> properties: {0}", sessionDocumentAsJson);

        if (CurrentSessionState == SessionState.InHostedLobby)
        {
            var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionSetProperties(
                EnclosingSessionDocument.SessionDocumentPropertyName,
                sessionDocumentAsJson,
                null);

            if (HR.FAILED(hresult))
            {
                Debug.LogErrorFormat("XblMultiplayerManagerLobbySessionSetProperties() failed with hresult = {0}.", hresult.ToString("X8"));
            }
        }
        else
        {
            var hresult = SDK.XBL.XblMultiplayerManagerGameSessionSetProperties(
                EnclosingSessionDocument.SessionDocumentPropertyName,
                sessionDocumentAsJson,
                null);

            if (HR.FAILED(hresult))
            {
                Debug.LogErrorFormat("XblMultiplayerManagerGameSessionSetProperties() failed with hresult = {0}.", hresult.ToString("X8"));
            }
        }
    }

    public void UpdateSessionProperties(string sessionPropertiesJson)
    {
        Debug.LogFormat("XboxLive.UpdateSessionProperties({0})", sessionPropertiesJson);

        if (!string.IsNullOrEmpty(sessionPropertiesJson))
        {
            var enclosingSessionDocument = JsonUtility.FromJson<EnclosingSessionDocument>(sessionPropertiesJson);
            CurrentSessionDocument = enclosingSessionDocument.SessionDocument;
            HostXuid = CurrentSessionDocument.HostMemberID;
            OnSessionPropertiesChanged?.Invoke(enclosingSessionDocument.SessionDocument);
        }
        else
        {
            CurrentSessionDocument = new SessionDocument();
        }
    }

    public void InviteToLobby(string activationString)
    {
        Debug.LogFormat("XboxLiveLogic.InviteToLobby({0})", activationString);

        SDK.XBL.XblMultiplayerManagerLobbySessionInviteFriends(
            MyUserHandle, 
            null,
            activationString);
    }

    public void JoinInvitedLobby()
    {
        Debug.LogFormat("XboxLiveLogic.JoinInvitedLobby()");

        JoinLobby(ReceivedInviteSessionHandleId);
    }

    public void JoinLobby(string sessionHandle)
    {
        Debug.LogFormat("XboxLiveLogic.JoinLobby()");

        // clean up any pending invite handle if it happens to be that one
        if (sessionHandle == ReceivedInviteSessionHandleId)
        {
            ReceivedInviteSessionHandleId = null;
        }

        // When joining a game we first join the MPM lobby and wait for the UserAdded event
        var hresult = SDK.XBL.XblMultiplayerManagerJoinLobby(sessionHandle, MyUserHandle);

        if (HR.FAILED(hresult))
        {
            OnMultiplayerError?.Invoke("XblMultiplayerManagerJoinLobby() failed.", hresult);
        }

        IsHost = false;
    }

    public void LeaveLobby()
    {
        Debug.LogFormat("XboxLiveLogic.LeaveLobby()");

        var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionRemoveLocalUser(MyUserHandle);

        if (HR.FAILED(hresult))
        {
            OnMultiplayerError?.Invoke("XblMultiplayerManagerLobbySessionRemoveLocalUser() failed.", hresult);
        }

        IsHost = false;
        HostXuid = 0;

        _sessionReference = null;
    }

    public ulong[] GetLobbyMembers()
    {
        Debug.LogFormat("XboxLiveLogic.GetLobbyMembers()");

        XblMultiplayerManagerMember[] members = null;
        var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionMembers(out members);

        if (HR.FAILED(hresult))
        {
            Debug.LogErrorFormat("XblMultiplayerManagerLobbySessionMembers() failed, hresult = {0}", hresult.ToString("X8"));
        }

        if (null == members)
        {
            return new ulong[0];
        }

        return members.Select(member => { return member.Xuid; }).ToArray();
    }

    public void StartGame()
    {
    }

    public void LeaveGame()
    {
        Debug.LogFormat("XboxLiveLogic.LeaveGame()");

        var hresult = SDK.XBL.XblMultiplayerManagerLeaveGame();

        if (HR.FAILED(hresult))
        {
            OnMultiplayerError?.Invoke("XblMultiplayerManagerLeaveGame() failed", hresult);
        }
    }

    public ulong[] GetGameMembers()
    {
        Debug.LogFormat("XboxLiveLogic.GetGameMembers()");

        XblMultiplayerManagerMember[] members = null;
        var hresult = SDK.XBL.XblMultiplayerManagerGameSessionMembers(out members);

        if (HR.FAILED(hresult))
        {
            Debug.LogErrorFormat("XblMultiplayerManagerGameSessionMembers() failed, hresult = {0}", hresult.ToString("X8"));
        }

        if (null == members)
        {
            return new ulong[0];
        }

        return members.Select(member => { return member.Xuid; }).ToArray();
    }

    private void ChooseNewHost()
    {
        Debug.LogFormat("XboxLiveLogic.__ChooseNewHost()");

        Assert.IsTrue(CurrentSessionState != SessionState.NoSession);

        XblMultiplayerManagerMember[] members = null;

        if (CurrentSessionState == SessionState.InHostedLobby)
        {
            var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionMembers(out members);
            Debug.LogFormat(">>> lobby session members: {0}, result = {1}", members.Length, hresult.ToString("X8"));
        }
        else
        {
            var hresult = SDK.XBL.XblMultiplayerManagerGameSessionMembers(out members);
            Debug.LogFormat(">>> game session members: {0}, result = {1}", members.Length, hresult.ToString("X8"));
        }

        var memberXuids = members.Select(member => { return member.Xuid; }).ToArray();
        var newHostXuid = ChooseDeterministicRandomXuid(memberXuids);

        IsHost = newHostXuid == MyXUID;
        // provisionally keep track of who the host will be
        HostXuid = newHostXuid;

        if (IsHost)
        {
            MakeMyselfBeHost();
        }

        var currentSessionDocument = CurrentSessionDocument;

        if (IsHost)
        {
            currentSessionDocument = currentSessionDocument ?? new SessionDocument();
            currentSessionDocument.HostMemberID = newHostXuid;
            SetSessionProperties(currentSessionDocument);
        }
        else if (null == currentSessionDocument || string.IsNullOrEmpty(currentSessionDocument.NetworkID))
        {
            // null out the existing cached document and wait for a refresh from the host
            UpdateSessionProperties(string.Empty);
        }
    }

    private void MakeMyselfBeHost()
    {
        Debug.LogFormat("XboxLiveLogic.__MakeMyselfBeHost()");

        Assert.IsTrue(IsHost);
        Assert.IsTrue(CurrentSessionState != SessionState.NoSession);

        OnHostChanged?.Invoke();

        string myDeviceToken = null;
        XblMultiplayerManagerMember[] members = null;

        if (CurrentSessionState == SessionState.InHostedLobby)
        {
            var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionMembers(out members);

            if (HR.FAILED(hresult) || members.Length == 0)
            {
                Debug.LogErrorFormat("XblMultiplayerManagerLobbySessionMembers() failed with hresult = {0}.", hresult.ToString("X8"));
                return;
            }
        }
        else
        {
            var hresult = SDK.XBL.XblMultiplayerManagerGameSessionMembers(out members);

            if (HR.FAILED(hresult) || members.Length == 0)
            {
                Debug.LogErrorFormat("XblMultiplayerManagerGameSessionMembers() failed with hresult = {0}.", hresult.ToString("X8"));
                return;
            }
        }

        myDeviceToken = members.First(member => { return member.Xuid == MyXUID; }).DeviceToken;

        // NOTE: On Unity, there does not ever appear to have a DeviceToken so we just early out since we
        // will not be able to provide it to the API if it does not exist ... and in that case we 
        // rely purely on the session document to set the host.

        if (string.IsNullOrEmpty(myDeviceToken))
        {
            Debug.LogErrorFormat("Failed to call XblMultiplayerManagerLobbySessionSetSynchronizedHost() because of empty device token.");
            return;
        }

        if (CurrentSessionState == SessionState.InHostedLobby)
        {
            var hresult = SDK.XBL.XblMultiplayerManagerLobbySessionSetSynchronizedHost(myDeviceToken, null);

            if (HR.FAILED(hresult))
            {
                Debug.LogErrorFormat("XblMultiplayerManagerLobbySessionSetSynchronizedHost() failed with hresult = {0}.", hresult.ToString("X8"));
            }
        }
        else
        {
            var hresult = SDK.XBL.XblMultiplayerManagerGameSessionSetSynchronizedHost(myDeviceToken, null);

            if (HR.FAILED(hresult))
            {
                Debug.LogErrorFormat("XblMultiplayerManagerGameSessionSetSynchronizedHost() failed with hresult = {0}.", hresult.ToString("X8"));
            }
        }
    }

    private ulong ChooseDeterministicRandomXuid(ulong[] xuids)
    {
        Assert.IsTrue(xuids.Length > 0);

        int total = 0;
        
        for(var i = 0; i < xuids.Length; i++)
        {
            total += Convert.ToInt32(xuids[i] & 0x7FFFFFFF);
        }

        var randomGenerator = new System.Random(total);
        var randomIndex = randomGenerator.Next(0, xuids.Length - 1);

        return xuids[randomIndex];
    }
}
