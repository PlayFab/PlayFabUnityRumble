//--------------------------------------------------------------------------------------
// GamerProfile.cs
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

using Custom_PlayFab;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GamerProfile : MonoBehaviour
{
    public Text ProfileName;

    public RawImage ProfileAvatar;

    public XboxLiveLogic XboxLiveLogic;

    public XboxLiveProfilePicUI XboxLiveProfilePicUI;

    private void Awake()
    {

    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ProfileName.text = XboxLiveLogic.MyGamerTag;
        XboxLiveProfilePicUI.LoadProfilePic(XboxLiveLogic.MyGamerTag);
        string playerInfo = XboxLiveLogic.MyGamerTag + ", " + PlayFabLoginManager.UserPlatform;
        PlayFabRuntimeInfos.Instance.SetSelfPlayerName(playerInfo);
    }

    private void OnValidate()
    {
        Assert.IsNotNull(ProfileName);
    }
}
