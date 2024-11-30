using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Group : MonoBehaviour
{
    public PlayerData playerData; //each slot have their own player info

    public Image playerImage;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerKillsNumText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        UpdateGroup();
    }

    // Update is called once per frame
    public void UpdateGroup()
    {
        playerImage.sprite = playerData.playerSprite;
        playerNameText.text = playerData.playerName;
        playerKillsNumText.text = playerData.playerKills.ToString();
    }
}
