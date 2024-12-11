using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour // change this to player overlay
{
    public static PlayerUIManager Instance { get; private set; }
    [SerializeField] private Image portrait;
    [SerializeField] private TMP_Text killCount;

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

}
