using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOverlay : MonoBehaviour
{
    public PlayerStats playerStats;
    public RectTransform barRect;
    public RectMask2D mask;

    private float maxRightMask;
    private float initialRightMask;

    public enum StatType { Health, Defense } 
    public StatType type; 

    private void Start()
    {
        maxRightMask = barRect.rect.width - mask.padding.x - mask.padding.z;
        initialRightMask = mask.padding.z;

        playerStats.OnStatsChanged += UpdateBar;
        UpdateBar();
    }


    public void SetValue(int maxStat, int currentStat)
    {
        if (maxStat <= 0)
        {
            Debug.LogError("SetValue: maxStat must be greater than 0.");
            return;
        }

        // Calculate new mask values based on health.
        var targetWidth = (float)currentStat / maxStat * maxRightMask;
        var newRightMask = maxRightMask + initialRightMask - targetWidth;

        // Update the mask's padding.
        var padding = mask.padding;
        padding.z = Mathf.Clamp(newRightMask, 0, maxRightMask + initialRightMask);
        mask.padding = padding;

        Debug.Log($"{newRightMask}");
    }

    private void OnDestroy()
    {
        playerStats.OnStatsChanged -= UpdateBar;
    }

    private void UpdateBar()
    {
         switch (type)
        {
            case StatType.Health:
                SetValue(PlayerStats.MAX_HEALTH, playerStats.health.Value); // change this
                break;

            case StatType.Defense:
                SetValue(PlayerStats.MAX_DEFENSE, playerStats.defense.Value); // change this 
                break;

            default:
                Debug.LogWarning("Status type not recognized!");
                break;
        }
    }
}
