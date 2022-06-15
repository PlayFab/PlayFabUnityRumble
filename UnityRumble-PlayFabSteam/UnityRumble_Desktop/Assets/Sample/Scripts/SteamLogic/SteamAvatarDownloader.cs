//--------------------------------------------------------------------------------------
// SteamAvatarDownloader.cs
//
// Steam player's head image downloader
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
using UnityEngine;
using Steamworks;

public class SteamAvatarDownloader : MonoBehaviour
{
    public static SteamAvatarDownloader Instance { get; private set; }

    private class DownloadInfo
    {
        public CSteamID PlayerSteamId;
        public string AvatarUrl;
        public Action<Texture2D> SuccessCallback;
        public Action<string> FailCallback;
    }

    private enum DownloadState
    {
        DOWNLOAD_STATE_FREE,
        DOWNLOAD_STATE_BUSY
    }

    private List<DownloadInfo> downloadInfoList = new List<DownloadInfo>();
    private DownloadState downloadState = DownloadState.DOWNLOAD_STATE_FREE;

    private Dictionary<ulong, Texture2D> steamPlayerAvatarCache = new Dictionary<ulong, Texture2D>();

    private const int SIZE_OF_COLOR32 = 4;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (downloadState != DownloadState.DOWNLOAD_STATE_FREE || downloadInfoList.Count <= 0)
        {
            return;
        }

        var info = downloadInfoList[0];
        downloadInfoList.RemoveAt(0);

        StartCoroutine(DoDownload(info));
    }

    private IEnumerator DoDownload(DownloadInfo info)
    {
        downloadState = DownloadState.DOWNLOAD_STATE_BUSY;

        var avatarInt = SteamFriends.GetMediumFriendAvatar(info.PlayerSteamId);
        while (avatarInt == -1)
        {
            yield return null;
        }

        if (avatarInt > 0)
        {
            uint imageWidth, imageHeight;
            SteamUtils.GetImageSize(avatarInt, out imageWidth, out imageHeight);
            if (imageWidth > 0 && imageHeight > 0)
            {
                var imageSize = imageWidth * imageHeight * SIZE_OF_COLOR32;
                byte[] avatarStream = new byte[imageSize];
                SteamUtils.GetImageRGBA(avatarInt, avatarStream, (int)imageSize);

                Texture2D texture = new Texture2D((int)imageWidth, (int)imageHeight, TextureFormat.RGBA32, false);
                texture.LoadRawTextureData(avatarStream);
                texture.Apply();
                texture = FlipTexture(texture);
                steamPlayerAvatarCache[info.PlayerSteamId.m_SteamID] = texture;

                info.SuccessCallback?.Invoke(texture);
            }
            else
            {
                info.FailCallback?.Invoke("Downloaded Image Width Or Height is Error");
            }
        }
        else
        {
            info.FailCallback?.Invoke("Download User Avatar Failed");
        }

        downloadState = DownloadState.DOWNLOAD_STATE_FREE;
    }

    public void GetPlayerAvatar(CSteamID playerSteamId, bool forceDownload, Action<Texture2D> OnSuccess, Action<String> OnFail)
    {
        if (!forceDownload)
        {
            if (steamPlayerAvatarCache.ContainsKey(playerSteamId.m_SteamID))
            {
                OnSuccess(steamPlayerAvatarCache[playerSteamId.m_SteamID]);
                return;
            }
        }

        var downloadInfo = new DownloadInfo { PlayerSteamId = playerSteamId, SuccessCallback = OnSuccess, FailCallback = OnFail };
        if (downloadState == DownloadState.DOWNLOAD_STATE_FREE)
        {
            StartCoroutine(DoDownload(downloadInfo));
        }
        else
        {
            downloadInfoList.Add(downloadInfo);
        }
    }

    static Texture2D FlipTexture(Texture2D texture2d)
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
        return flipTexture;
    }
}
