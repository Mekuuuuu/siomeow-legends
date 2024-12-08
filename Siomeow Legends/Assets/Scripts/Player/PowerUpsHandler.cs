using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsHandler : MonoBehaviour
{
    private PlayerMovement player;
    private PlayerStats playerStats;

    public void Initialize(PlayerMovement player, PlayerStats playerStats)
    {
        this.player = player;
        this.playerStats = playerStats;
    }

    public void ApplyPowerUp(PickupItem.PowerUp type)
    {
        switch (type)
        {
            case PickupItem.PowerUp.Berserk:
                StartCoroutine(ApplyBerserk());
                break;

            case PickupItem.PowerUp.Movement:
                StartCoroutine(ApplySpeedBoost());
                break;

            case PickupItem.PowerUp.Stamina:
                StartCoroutine(ApplyStaminaBoost());
                break;

            case PickupItem.PowerUp.Heal:
                ApplyHeal();
                break;

            case PickupItem.PowerUp.Shield:
                ApplyShield();
                break;

            default:
                Debug.LogWarning("Power-up type not recognized!");
                break;
        }
    }
    
    private IEnumerator ApplyBerserk()
    {
        float originalMultiplier = 1f;
        float boostMultiplier = 1.5f;
        float duration = 10f;

        playerStats.damageMultiplier = boostMultiplier;
        Debug.Log($"Berserk activated! Increased damage by {playerStats.damageMultiplier}!");

        yield return new WaitForSeconds(duration);

        playerStats.damageMultiplier = originalMultiplier;
        Debug.Log($"Berserk ended. Damage reverted to {playerStats.damageMultiplier}.");
    }

    private IEnumerator ApplySpeedBoost()
    {
        float originalSpeed = 5f;
        float boostAmount = 15f;
        float maxSpeed = 20f;
        float duration = 5f;

        player.speed = Mathf.Min(originalSpeed + boostAmount, maxSpeed);
        Debug.Log($"Speed boosted to: {player.speed}");

        yield return new WaitForSeconds(duration);

        player.speed = originalSpeed;
        Debug.Log($"Speed reverted to original value: {player.speed}");
    }

    private IEnumerator ApplyStaminaBoost()
    {
        player.canDash = true; 
        player.dashingCooldown = 0f;
        Debug.Log("Stamina replenished. Dash cooldown reset to 0, you can doshge now!");

        yield return new WaitUntil(() => !player.isDashing);
        player.dashingCooldown = 10f;
        Debug.Log($"Cooldown reset to: {player.dashingCooldown}!");
    }

    private void ApplyHeal()
    {
        int healAmount = 100;

        playerStats.Heal(healAmount);
        Debug.Log($"Health is now {playerStats.health}!");
    }

    private void ApplyShield()
    {
        int defenseAmount = 50;

        playerStats.IncreaseDefense(defenseAmount);
        Debug.Log($"Defense is now {playerStats.defense}!");
    }
}