//--------------------------------------------------------------------------------------
// PlayFabAvatarDownloader.cs
//
// PlayFab player's head image downloader
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
using UnityEngine.Networking;

public class PlayFabAvatarDownloader : MonoBehaviour
{
    public static PlayFabAvatarDownloader Instance { get; private set; }

    private class DownloadInfo
    {
        public ulong UserId;
        public string AvatarUrl;
        public Action<Texture2D> SuccessCallback;
        public Action<string> ErrorCallback;
    }

    private enum DownloadState
    {
        DOWNLOAD_STATE_FREE,
        DOWNLOAD_STATE_BUSY
    }

    private List<DownloadInfo> downloadInfoList = new List<DownloadInfo>();
    private DownloadState downloadState = DownloadState.DOWNLOAD_STATE_FREE;

    private Dictionary<ulong, Texture2D> playfabPlayerAvatarCache = new Dictionary<ulong, Texture2D>();

    private const int SIZE_OF_COLOR32 = 4;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple PlayFabAvatarDownloader instances were added to the scene!");

            Destroy(this);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (downloadState != DownloadState.DOWNLOAD_STATE_FREE || downloadInfoList.Count == 0)
        {
            return;
        }

        var info = downloadInfoList[0];
        downloadInfoList.RemoveAt(0);

        StartCoroutine(Download(info));
    }

    private IEnumerator Download(DownloadInfo info)
    {
        downloadState = DownloadState.DOWNLOAD_STATE_BUSY;

        using (UnityWebRequest request = new UnityWebRequest())
        {
            DownloadHandlerTexture handler = new DownloadHandlerTexture();
            request.downloadHandler = handler;

            yield return request.SendWebRequest();

            if (!(request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError))
            {
                var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                playfabPlayerAvatarCache.Add(info.UserId, texture);
                info.SuccessCallback?.Invoke(texture);
            }
            else
            {
                info.ErrorCallback?.Invoke($"Avatar Download Failed, url = {info.AvatarUrl}");
            }
        }

        downloadState = DownloadState.DOWNLOAD_STATE_FREE;
    }

    public void GetPlayerAvatar(ulong playerSteamId, string avatarUrl, Action<Texture2D> successCallback, Action<string> errorCallback)
    {
        var downloadInfo = new DownloadInfo { UserId = playerSteamId, AvatarUrl = avatarUrl, SuccessCallback = successCallback, ErrorCallback = errorCallback };
        if (downloadState == DownloadState.DOWNLOAD_STATE_FREE)
        {
            StartCoroutine(Download(downloadInfo));
        }
        else
        {
            downloadInfoList.Add(downloadInfo);
        }
    }
}
