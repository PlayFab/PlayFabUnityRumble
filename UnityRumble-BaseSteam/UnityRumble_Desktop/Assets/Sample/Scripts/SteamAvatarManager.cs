//--------------------------------------------------------------------------------------
// SteamAvatarManager.cs
//
// The Manager class for the SteamAvatar Manager and avatar download queue.
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
using UnityEngine.UI;
using Steamworks;

public class SteamAvatarManager
{
    static ulong AvatarTaskCount = 0;
    static Dictionary<ulong, Callback<AvatarImageLoaded_t>> AvatarTaskList = new Dictionary<ulong, Callback<AvatarImageLoaded_t>>();

    public static void GetUserAvatar(CSteamID steamID, RawImage rawImage = null)
    {
        int userAvatar = SteamFriends.GetLargeFriendAvatar(steamID);
        uint imageWidth;
        uint imageHeight;
        bool restartAvatarLoad = true;
        bool success = SteamUtils.GetImageSize(userAvatar, out imageWidth, out imageHeight);
        if (success && imageWidth > 0 && imageHeight > 0)
        {
            byte[] data = new byte[imageWidth * imageHeight * sizeof(int)];
            var returnTex = new Texture2D((int)imageWidth, (int)imageHeight, TextureFormat.RGBA32, false, false);
            success = SteamUtils.GetImageRGBA(userAvatar, data, (int)(imageWidth * imageHeight * sizeof(int)));
            if (success)
            {
                restartAvatarLoad = false;
                returnTex.LoadRawTextureData(data);
                returnTex.Apply();
                Texture2D result = null;
                if (rawImage != null)
                {
                    result = FlipTexture(returnTex, rawImage);
                }
                Texture2D.DestroyImmediate(returnTex);
            }
        }

        if (restartAvatarLoad)
        {
            ulong key = AvatarTaskCount;
            AvatarTaskCount++;
            var task = new Callback<AvatarImageLoaded_t>(delegate (AvatarImageLoaded_t param)
            {
                AvatarTaskList.Remove(key);
            });

            AvatarTaskList.Add(key, task);
        }
    }

    static Texture2D FlipTexture(Texture2D texture2d, RawImage rawImage = null)
    {
        Texture2D flipTexture = new Texture2D(texture2d.width, texture2d.height);

        int width = texture2d.width;
        int height = texture2d.height;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                flipTexture.SetPixel(i, height - j - 1, texture2d.GetPixel(i, j));
            }
        }

        flipTexture.Apply();
        if (rawImage != null)
        {
            rawImage.texture = flipTexture;
        }
        return flipTexture;
    }
}
