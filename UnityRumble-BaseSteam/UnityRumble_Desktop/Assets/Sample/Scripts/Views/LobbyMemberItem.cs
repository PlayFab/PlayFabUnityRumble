using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMemberItem : MonoBehaviour
{
    public ulong UserId;

    public Text NameText;

    public RawImage AvatarImage;

    public Image ShipTypeImage;

    public Image ShipColorImage;

    public Toggle ReadyState;

    public GameLobbyScreen GameLobbyScreen;

    public void Show(ulong userId, string name, bool isReady, int shipIndex, int colorIndex)
    {
        UserId = userId;

        NameText.text = name;

        ReadyState.isOn = isReady;

        SteamAvatarManager.GetUserAvatar(new Steamworks.CSteamID(userId), AvatarImage);

        ShipTypeImage.sprite = GameLobbyScreen.TheGameAssetManager.PlayerShipSprites[shipIndex];

        ShipColorImage.color = GameLobbyScreen.TheGameAssetManager.PlayerShipColorChoices[colorIndex];
    }
}
