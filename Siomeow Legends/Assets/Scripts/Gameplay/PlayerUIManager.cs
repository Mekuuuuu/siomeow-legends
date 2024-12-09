using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour // change this to player overlay
{
    
    public static PlayerUIManager Instance { get; private set; }
    
    [Header("Player Portrait")]
    [SerializeField] private Image portrait;

    [Header("Stat Bars")]
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private RectMask2D healthMask;
    [SerializeField] private RectTransform defenseBarRect;
    [SerializeField] private RectMask2D defenseMask;

    private float maxHealthMask;
    private float initialHealthMask;
    private float maxDefenseMask;
    private float initialDefenseMask;

    private PlayerStats playerStats;

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

    public void Initialize(PlayerStats stats)
    {
        playerStats = stats;

        if (healthBarRect != null && healthMask != null)
        {
            maxHealthMask = healthBarRect.rect.width - healthMask.padding.x - healthMask.padding.z;
            initialHealthMask = healthMask.padding.z;
        }

        if (defenseBarRect != null && defenseMask != null)
        {
            maxDefenseMask = defenseBarRect.rect.width - defenseMask.padding.x - defenseMask.padding.z;
            initialDefenseMask = defenseMask.padding.z;
        }

        if (playerStats != null)
        {
            playerStats.OnStatsChanged += UpdateUI;
            UpdateUI();
        }
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnStatsChanged -= UpdateUI;
        }
    }

    public void SetPlayerPortrait(Sprite portrait)
    {
        if (portrait != null)
        {
            this.portrait.sprite = portrait;
        }
    }

    private void UpdateUI()
    {
        UpdateStatBar(healthMask, PlayerStats.MAX_HEALTH, playerStats.health.Value, maxHealthMask, initialHealthMask);
        UpdateStatBar(defenseMask, PlayerStats.MAX_DEFENSE, playerStats.defense.Value, maxDefenseMask, initialDefenseMask);
    }

    private void UpdateStatBar(RectMask2D mask, int maxStat, int currentStat, float maxMaskWidth, float initialMaskWidth)
    {
        if (maxStat <= 0)
        {
            Debug.LogError("Max stat must be greater than zero.");
            return;
        }

        // Calculate the new mask width based on the current stat value.
        float targetWidth = (float)currentStat / maxStat * maxMaskWidth;
        float newRightMask = maxMaskWidth + initialMaskWidth - targetWidth;

        // Update the mask's padding.
        var padding = mask.padding;
        padding.z = Mathf.Clamp(newRightMask, 0, maxMaskWidth + initialMaskWidth);
        mask.padding = padding;
    }
}
