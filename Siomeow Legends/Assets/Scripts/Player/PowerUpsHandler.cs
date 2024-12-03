using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsHandler : MonoBehaviour
{
    private PlayerMovement player;

    public void Initialize(PlayerMovement player)
    {
        this.player = player;
    }

    public void ApplyPowerUp(PickupItem.PowerUp type)
    {
        switch (type)
        {
            case PickupItem.PowerUp.Berserk:
                Debug.LogWarning("Berserk not yet implemented!");
                break;

            case PickupItem.PowerUp.Movement:
                StartCoroutine(ApplySpeedBoost());
                break;

            case PickupItem.PowerUp.Stamina:
                StartCoroutine(ApplyStaminaBoost());
                // Debug.LogWarning("Stamina boost not yet implemented!");
                break;

            default:
                Debug.LogWarning("Power-up type not recognized!");
                break;
        }
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
        // Reset the dash cooldown immediately
        player.canDash = true;  // Allow dashing immediately after reset
        player.dashingCooldown = 0f;
        Debug.Log("Stamina replenished. Dash cooldown reset to 0, you can doshge now!");

        // Wait until the player finishes the dash
        yield return new WaitUntil(() => !player.isDashing);
        player.dashingCooldown = 10f;
        Debug.Log($"Cooldown reset to: {player.dashingCooldown}!");
    }

}