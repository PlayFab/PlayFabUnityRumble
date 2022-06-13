//--------------------------------------------------------------------------------------
// XboxLiveSocialLogic.cs
//
// Xbox Live logic that wraps Social Manager friend functionality.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Linq;

#if USE_MS_GAMECORE
using XGamingRuntime;
using HR = XGamingRuntime.Interop.HR;
#elif USE_UNITY_GAMECORE
using Unity.GameCore;
#endif

public partial class XboxLiveLogic
{
    // parameters are: xuid, profile
    public event Action<ulong, SocialProfile> OnSocialProfileObtained;
    // parameters are:
    public event Action OnSocialFriendActivitiesObtained;
    // parameters are: error message, hresult
    public event System.Action<string, int> OnSocialError;

    public class SocialProfile
    {
        public ulong XUID { get; internal set; }
        public string GamerTag { get; internal set; }
        public string DisplayName { get; internal set; }
        public string DisplayPicUrl { get; internal set; }
    }

    public class SocialFriend : SocialProfile
    {
        public XblMultiplayerActivityDetails CurrentActivityDetails { get; internal set; }
    }

    public void InitializeSocial()
    {
        Debug.LogFormat("XboxLiveLogic.InitializeSocial()");

        SDK.XBL.XblSocialManagerAddLocalUser(
            MyUserHandle,
            XblSocialManagerExtraDetailLevel.NoExtraDetail);
    }

    public void CleanupSocial()
    {
        Debug.LogFormat("XboxLiveLogic.CleanupSocial()");

        if (null != _socialGroupHandle)
        {
            SDK.XBL.XblSocialManagerDestroySocialUserGroup(_socialGroupHandle);
            _socialGroupHandle = null;
        }

        SDK.XBL.XblSocialManagerRemoveLocalUser(
            MyUserHandle,
            XblSocialManagerExtraDetailLevel.NoExtraDetail);
    }

    public void DownloadUserProfilePic(string profileName, System.Action<string, byte[]> profilePicCallback)
    {
        Debug.LogFormat("XboxLiveLogic.DownloadUserProfilePic({0})", profileName);

        if (_profilePicBytes.ContainsKey(profileName))
        {
            profilePicCallback?.Invoke(profileName, _profilePicBytes[profileName]);
        }
        else
        {
            // we can use the social manager to get the friend's profile pic URL
            // and then use UnityWebRequest to get that bytes from that URL
            if (profileName == MyGamerTag)
            {
                SDK.XUserGetGamerPictureAsync(
                    MyUserHandle, 
                    XUserGamerPictureSize.Medium, 
                    (int hresult, byte[] buffer) =>
                    {
                        if (HR.FAILED(hresult))
                        {
                            OnGeneralError?.Invoke(
                                string.Format("XUserGetGamerPictureAsync({0}) failed", profileName),
                                hresult);
                        }
                        else
                        {
                            profilePicCallback(profileName, buffer);
                        }
                    });
            }
            else
            {
                StartCoroutine(DownloadProfilePicCoroutine(profileName, profilePicCallback));
            }
        }
    }

    public void GetSocialProfile(ulong xuid)
    {
        Debug.LogFormat("XboxLiveLogic.GetSocialProfile()");

        if (_cachedSocialProfiles.ContainsKey(xuid))
        {
            OnSocialProfileObtained?.Invoke(xuid, _cachedSocialProfiles[xuid]);
        }

        SDK.XBL.XblProfileGetUserProfileAsync(
            MyContextHandle,
            xuid,
            (Int32 hresult, XblUserProfile result) =>
            {
                if (HR.FAILED(hresult))
                {
                    OnSocialError?.Invoke(string.Format("XblProfileGetUserProfileAsync({0}) failed", xuid), hresult);
                }
                else
                {
                    OnSocialProfileObtained?.Invoke(
                        xuid,
                        new SocialProfile()
                        {
                            XUID = result.XboxUserId,
                            GamerTag = result.Gamertag,
                            DisplayName = result.AppDisplayName,
                            DisplayPicUrl = result.AppDisplayPictureResizeUri
                        });
                }
            });
    }

    public bool HasOnlineInTitleFriend(ulong friendXuid)
    {
        return _onlineInTitleFriends.ContainsKey(friendXuid);
    }

    public SocialFriend GetOnlineInTitleFriend(ulong friendXuid)
    {
        SocialFriend friend = null;
        _onlineInTitleFriends.TryGetValue(friendXuid, out friend);
        return friend;
    }

    public void GetOnlineInTitleFriends(out SocialFriend[] friends)
    {
        Debug.LogFormat("XboxLiveLogic.GetOnlineInTitleFriends()");

        if (_onlineInTitleFriends.Count == 0)
        {
            friends = null;
            return;
        }

        friends = new SocialFriend[_onlineInTitleFriends.Count];
        var index = 0;
        foreach (var friend in _onlineInTitleFriends)
        {
            friends[index] = friend.Value;
            index++;
        }

        Debug.LogFormat(">>> returned {0} friends.", friends.Length);
    }

    public void GetSocialFriendActivities()
    {
        Debug.LogFormat("XboxLiveLogic.GetSocialFriendActivities()");

        SDK.XBL.XblMultiplayerGetActivitiesWithPropertiesForSocialGroupAsync(
            MyContextHandle,
            Configuration.SCID,
            MyXUID,
            "People",
            HandleFriendActivitiesObtained);
    }

    public void UpdateSocial()
    {
        XblSocialManagerEvent[] socialEvents;
        var hr = SDK.XBL.XblSocialManagerDoWork(out socialEvents);

        if (HR.FAILED(hr))
        {
            OnSocialError?.Invoke("XblSocialManagerDoWork() failed", hr);
            return;
        }

        if (socialEvents == null)
        {
            return;
        }

        foreach (var socialEvent in socialEvents)
        {
            switch (socialEvent.EventType)
            {
                case XblSocialManagerEventType.LocalUserAdded:
                    {
                        Debug.Log("XblSocialManagerEventType::LocalUserAdded");
                        var result = SDK.XBL.XblSocialManagerCreateSocialUserGroupFromFilters(
                            MyUserHandle,
                            XblPresenceFilter.All/*TitleOnline*/, 
                            XblRelationshipFilter.Friends,
                            out _socialGroupHandle);
                        if (HR.FAILED(result))
                        {
                            OnSocialError?.Invoke("XblSocialManagerCreateSocialUserGroupFromFilters() failed", result);
                        }
                        break;
                    }

                case XblSocialManagerEventType.UsersAddedToSocialGraph:
                    Debug.Log("XblSocialManagerEventType::UsersAddedToSocialGraph");
                    break;

                case XblSocialManagerEventType.UsersRemovedFromSocialGraph:
                    Debug.Log("XblSocialManagerEventType::UsersRemovedFromSocialGraph");
                    break;

                case XblSocialManagerEventType.SocialRelationshipsChanged:
                    Debug.Log("XblSocialManagerEventType::SocialRelationshipsChanged");
                    break;

                case XblSocialManagerEventType.SocialUserGroupLoaded:
                    Debug.Log("XblSocialManagerEventType::SocialUserGroupLoaded");
                    UpdateFriendDisplayNames(socialEvent.LoadedGroup);
                    break;

                case XblSocialManagerEventType.SocialUserGroupUpdated:
                    Debug.Log("XblSocialManagerEventType::SocialUserGroupUpdated");
                    UpdateFriendDisplayNames(socialEvent.LoadedGroup);
                    break;

                case XblSocialManagerEventType.PresenceChanged:
                    Debug.Log("XblSocialManagerEventType::PresenceChanged");
                    break;

                case XblSocialManagerEventType.ProfilesChanged:
                    Debug.Log("XblSocialManagerEventType::ProfilesChanged");
                    break;

                default:
                    throw new NotImplementedException(string.Format("Unexpected social event type: {0}", socialEvent.EventType.ToString()));
            }
        }
    }

    private void HandleFriendActivitiesObtained(Int32 hresult, XblMultiplayerActivityDetails[] result)
    {
        Debug.LogFormat("XboxLiveLogic.__HandleFriendActivitiesObtained()");

        if (HR.FAILED(hresult))
        {
            OnSocialError?.Invoke("XblMultiplayerGetActivitiesWithPropertiesForSocialGroupAsync() failed", hresult);
        }

        foreach(var friend in _onlineInTitleFriends)
        {
            friend.Value.CurrentActivityDetails = null;
        }

        Debug.LogFormat(">>> sifting through {0} activities...", result.Length);

        foreach (var activity in result)
        {
            // make sure we have the friend tracked
            if (!_onlineInTitleFriends.ContainsKey(activity.OwnerXuid))
            {
                Debug.LogFormat(">>> found non-friend xuid: {0}", activity.OwnerXuid.ToString());
                continue;
            }

            // check the visibility
            if (activity.Visibility != XblMultiplayerSessionVisibility.Open)
            {
                Debug.LogFormat(">>> found invisible activity for xuid: {0}", activity.OwnerXuid.ToString());
                continue;
            }

            // check the join restriction
            if (activity.JoinRestriction != XblMultiplayerSessionRestriction.Followed &&
                activity.JoinRestriction != XblMultiplayerSessionRestriction.None)
            {
                Debug.LogFormat(">>> found restricted activity for xuid: {0}", activity.OwnerXuid.ToString());
                continue;
            }

            // check the closed flag
            if (activity.Closed)
            {
                Debug.LogFormat(">>> found closed activity for xuid: {0}", activity.OwnerXuid.ToString());
                continue;
            }

            // check the member count against the max member count
            if (activity.MembersCount == activity.MaxMembersCount)
            {
                Debug.LogFormat(">>> found maxed out activity for xuid: {0}", activity.OwnerXuid.ToString());
                continue;
            }

            Debug.LogFormat(">>> FOUND JOINABLE ACTIVITY for xuid: {0}", activity.OwnerXuid.ToString());
            _onlineInTitleFriends[activity.OwnerXuid].CurrentActivityDetails = activity;
        }

        OnSocialFriendActivitiesObtained?.Invoke();
    }

    private IEnumerator DownloadProfilePicCoroutine(
        string contextData,
        System.Action<string, byte[]> completionCallback)
    {
        const float FriendLoadTimeoutInS = 10F;

        SocialFriend[] friends;

        // wait for the friend data to load, up to a time limit
        var timeLeft = FriendLoadTimeoutInS;
        do
        {
            GetOnlineInTitleFriends(out friends);
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        while (friends == null && timeLeft > 0F);

        if (timeLeft <= 0F && friends == null)
        {
            OnSocialError?.Invoke("Loading social friends timed out.", 0);
            yield break;
        }

        // get the friend whose name corresponds with the given context name
        var friendWithGamertag = friends.First(friend => { return friend.GamerTag == contextData; });
        if (friendWithGamertag != null)
        {
            var webRequest = UnityWebRequest.Get(friendWithGamertag.DisplayPicUrl);
            yield return webRequest.SendWebRequest();

            completionCallback?.Invoke(contextData, webRequest.downloadHandler.data);
        }
        else
        {
            OnSocialError?.Invoke($"Friend with gamertag '{contextData}' not found.", 0);
        }

        yield break;
    }

    private void UpdateFriendDisplayNames(XblSocialManagerUserGroupHandle groupHandle)
    {
        Debug.LogFormat("XboxLiveLogic.UpdateFriendDisplayNames()");

        XblSocialManagerUser[] users;

        var hr = SDK.XBL.XblSocialManagerUserGroupGetUsers(
            groupHandle,
            out users);

        if (HR.FAILED(hr))
        {
            OnSocialError?.Invoke("XblSocialManagerUserGroupGetUsers() failed", hr);
            return;
        }
        else
        {
            Debug.LogFormat("... a total of {0} users found.", users.Length);
        }

        _onlineInTitleFriends.Clear();

        foreach (var user in users)
        {
            AddOnlineInTitleFriend(user);
        }
    }

    private void AddOnlineInTitleFriend(XblSocialManagerUser user)
    {
        _onlineInTitleFriends[user.XboxUserId] = new SocialFriend()
        {
            XUID = user.XboxUserId,
            GamerTag = user.Gamertag,
            DisplayName = user.DisplayName,
            DisplayPicUrl = user.DisplayPicUrlRaw
        };
    }

    private XblSocialManagerUserGroupHandle _socialGroupHandle = null;
    // note: maps form XUID -> SocialFriend
    private readonly Dictionary<ulong, SocialFriend> _onlineInTitleFriends =
        new Dictionary<ulong, SocialFriend>();
    // note: maps form XUID -> SocialProfile
    private readonly Dictionary<ulong, SocialProfile> _cachedSocialProfiles =
        new Dictionary<ulong, SocialProfile>();
    // note: maps from profileName -> profilePicPngBytes
    private readonly Dictionary<string, byte[]> _profilePicBytes = new Dictionary<string, byte[]>();
}
