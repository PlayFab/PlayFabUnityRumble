//--------------------------------------------------------------------------------------
// PlayFabEnvInitializer.cs
//
// Setup and initialization of Playfab environments.
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
using UnityEngine;
using UnityEngine.Assertions;
using PlayFab;

namespace Custom_PlayFab
{
    public class PlayFabEnvInitializer : MonoBehaviour
    {
        public static PlayFabEnvInitializer Instance { get; private set; }

        public GameObject PlayFabMultiPlayerManagerPrefab;
        public GameObject PlayfabMultiPlayerEventProcessorPrefab;

        private GameObject PlayFabMultiPlayerManagerObj;
        private GameObject PlayFabMultiPlayerEventProcessorObj;

        private readonly string TITLE_ID = "00000";

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple PlayFabEnvInitializer instances were added to the scene!");
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnValidate()
        {
            Assert.IsNotNull(PlayFabMultiPlayerManagerPrefab);
            Assert.IsNotNull(PlayfabMultiPlayerEventProcessorPrefab);
        }

        public void InitPlayFabEnv()
        {
            PlayFabSettings.staticSettings.TitleId = TITLE_ID;

            InstantiatePartyPrefab();
            GameObject.Instantiate(PlayfabMultiPlayerEventProcessorPrefab);
        }

        public void DestroyPartyPrefab()
        {
            if (PlayFabMultiPlayerManagerObj != null)
            {
                PlayFabMultiPlayerManagerObj.gameObject.SetActive(false);
                Destroy(PlayFabMultiPlayerManagerObj);
            }
        }

        public void DestroyMultiplayerPrefab()
        {
            if (PlayFabMultiPlayerEventProcessorObj != null)
            {
                PlayFabMultiPlayerEventProcessorObj.gameObject.SetActive(false);
                Destroy(PlayFabMultiPlayerEventProcessorObj);
            }
        }

        public void InstantiatePartyPrefab()
        {
            PlayFabMultiPlayerManagerObj = GameObject.Instantiate(PlayFabMultiPlayerManagerPrefab);
        }

        public void InstantiateMultiplayerPrefab()
        {
            PlayFabMultiPlayerEventProcessorObj = GameObject.Instantiate(PlayfabMultiPlayerEventProcessorPrefab);
        }
    }
}
