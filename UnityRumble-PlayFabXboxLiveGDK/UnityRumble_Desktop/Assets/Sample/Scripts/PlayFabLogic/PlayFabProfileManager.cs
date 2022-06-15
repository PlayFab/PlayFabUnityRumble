//--------------------------------------------------------------------------------------
// PlayFabProfileManager.cs
//
// Manage infomations of player in PlayFab.
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
using PlayFab;
using PlayFab.ClientModels;

namespace Custom_PlayFab
{
    public class PlayFabProfileManager
    {
        private static PlayFabProfileManager instance;
        public static PlayFabProfileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayFabProfileManager();
                }
                return instance;
            }
        }

        private PlayFabProfileManager()
        {
        }

        public void ShowUserName(Text userNameText, string userId)
        {
            GetPlayerProfileRequest profileRequest = new GetPlayerProfileRequest();
            profileRequest.PlayFabId = userId;
            PlayFabClientAPI.GetPlayerProfile(
                profileRequest,
                completed =>
                {
                    Debug.Log("Get User Profile is Completed.");
                    string userName = completed.PlayerProfile.DisplayName;
                    if (userName == null)
                    {
                        userNameText.text = PlayFabRuntimeInfos.Instance.MySteamUserName;
                    }
                },
                failure =>
                {
                    Debug.LogFormat("Error : {0}", failure.Error);
                }
                );
        }

        public void ShowAvatarImage(RawImage rawImage, string userId)
        {
            GetPlayerProfileRequest profileRequest = new GetPlayerProfileRequest();
            profileRequest.PlayFabId = userId;
            PlayFabClientAPI.GetPlayerProfile(
                profileRequest,
                completed =>
                {
                    PlayFabAvatarDownloader.Instance.GetPlayerAvatar(Convert.ToUInt64(userId, 16), completed.PlayerProfile.AvatarUrl,
                        tex =>
                        {
                            rawImage.texture = tex;
                        },
                        errorMessage =>
                        {
                            Debug.LogError("Download User Avatar failed");
                        });
                },
                failure =>
                {
                    Debug.LogFormat("Error : {0}", failure.Error);
                });
        }
    }
}
