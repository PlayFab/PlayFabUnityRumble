namespace PlayFab.Multiplayer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayfabMultiplayerEventProcessor : MonoBehaviour
    {
        private void Awake()
        {
            // Exists across scenes for convenience.
            DontDestroyOnLoad(this);

            if (!PlayFabMultiplayer.IsInitialized)
            {
                PlayFabMultiplayer.Initialize();
            }
        }

        private void Update()
        {
            // This object just automatically processes the state changes which in turn triggers
            // the PlayFabMultiplayer.On* events that your other scripts can listen to.
            PlayFabMultiplayer.ProcessLobbyStateChanges();
            PlayFabMultiplayer.ProcessMatchmakingStateChanges();
        }

        private void OnDestroy()
        {
            PlayFabMultiplayer.Uninitialize();
        }
    }
}
