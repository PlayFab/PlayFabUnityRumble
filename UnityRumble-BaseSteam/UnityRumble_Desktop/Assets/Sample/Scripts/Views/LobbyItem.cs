using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    public Button JoinLobbyBtn;

    public MainMenuScreen MainMenu;

    public SteamLobbyManager.LobbyObject LobbyObject;

    public void JoinLobbyBtnClick()
    {
        LobbyManager.Instance.JoinLobby(LobbyObject.LobbyId);
    }
  
}
