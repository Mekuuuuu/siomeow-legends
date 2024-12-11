using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingEntry : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image playerPortraitImage;
    [SerializeField] private TMP_Text killCountText;

    public void SetPlayerName(string playerName)
    {
        playerNameText.text = playerName;
    }

    public void SetPlayerPortrait(int characterId)
    {
        Character[] allCharacters = characterDatabase.GetAllCharacters();
        foreach (Character character in allCharacters)
        {
            if (character.Id == characterId)
            {
                playerPortraitImage.sprite = character.Portrait;
            }
        }
    }

    public void SetKills(int killCount)
    {
        killCountText.text = killCount.ToString("D2");
    }

}
