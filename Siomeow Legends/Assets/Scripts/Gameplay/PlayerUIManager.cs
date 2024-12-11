using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : NetworkBehaviour // change this to player overlay
{
    public static PlayerUIManager Instance { get; private set; }
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text killCount;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image defenseBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SetPlayerPortrait(Sprite portrait)
    {
        if (portrait != null)
        {
            this.portrait.sprite = portrait;
        }
    }

    public void SetKillCount(int count)
    {
        killCount.text = count.ToString("D2"); // Assumes `killCount` is a TMP_Text for displaying the value.
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
            {
                healthBar.fillAmount = (float)currentHealth / maxHealth;
            }
    }

    public void SetDefense(int currentDefense, int maxDefense)
    {
        if (defenseBar != null)
            {
                defenseBar.fillAmount = (float)currentDefense / maxDefense;
            }
    }

}
