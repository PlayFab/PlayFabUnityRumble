using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    public RawImage userAvatarRawImage;
    public Text userNameText;
    public Text userRankText;
    public Text userScoreText;
    public Texture2D texture2D;

    public void ItemInit(ulong cSteamID, string userName, int userRank, int userScore)
    {
        userNameText.text = userName.ToString();
        userRankText.text = userRank.ToString();
        userScoreText.text = "Score: " + userScore;
        SteamAvatarManager.GetUserAvatar(new Steamworks.CSteamID(cSteamID), userAvatarRawImage);
    }
}
