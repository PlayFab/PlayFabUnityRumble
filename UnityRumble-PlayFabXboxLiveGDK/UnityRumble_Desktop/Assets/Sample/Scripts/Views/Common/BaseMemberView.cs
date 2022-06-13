//--------------------------------------------------------------------------------------
// BaseMemberView.cs
//
// The base class for a single member item within a session members list.
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

using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public abstract class BaseMemberView : MonoBehaviour
{
    [SerializeField]
    protected Text GamertagText;

    [SerializeField]
    protected Image UserPlatformImage;

    [SerializeField]
    protected Image HostIndicatorImage;

    [SerializeField]
    protected Sprite XboxSprite;

    [SerializeField]
    protected Sprite SteamSprite;

    public void SetGamertag(string gamertag)
    {
        string[] playerInfo = Regex.Split(gamertag, ", ", RegexOptions.IgnoreCase);
        GamertagText.text = playerInfo.Length > 0 ? playerInfo[0] : gamertag;
        string userPlatform = playerInfo.Length > 1 ? playerInfo[1] : "Unknown";

        if (userPlatform == "Steam")
        {
            UserPlatformImage.sprite = SteamSprite;
        }
        else if (userPlatform == "Xbox")
        {
            UserPlatformImage.sprite = XboxSprite;
        }
        HandleUserPlatformSet();
    }

    public string GetGamertag()
    {
        return GamertagText.text;
    }

    public void MakeHost()
    {
        HostIndicatorImage.enabled = true;
        HostIndicatorImage.color = Color.green;
        HostIndicatorImage.transform.Find("Text").GetComponent<Text>().text = "HOST";
        HandleHostSet();
    }

    protected abstract void HandleUserPlatformSet();

    protected abstract void HandleHostSet();

    protected virtual void OnValidate()
    {
        Assert.IsNotNull(GamertagText, $"Set the gamertag text for the {name} object");
        Assert.IsNotNull(HostIndicatorImage, $"Set the HostIndicatorImage image for the {name} object");
        Assert.IsNotNull(SteamSprite, $"Set the Steam sprite for the {name} object");
        Assert.IsNotNull(XboxSprite, $"Set the Xbox sprite for the {name} object");
        Assert.IsNotNull(UserPlatformImage, $"Set the UserPlatformImage image for the {name} object");
    }
}
