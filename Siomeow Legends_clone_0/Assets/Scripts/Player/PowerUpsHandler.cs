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

    public void ApplyPowerUp(PickupItem.PowerUp type, float amount, float duration)
    {
        switch (type)
        {
            case PickupItem.PowerUp.Berserk:
                Debug.LogWarning("Berserk not yet implemented!");
                break;

            case PickupItem.PowerUp.Movement:
                StartCoroutine(ApplySpeedBoost(amount, duration));
                break;

            case PickupItem.PowerUp.Stamina:
                Debug.LogWarning("Stamina boost not yet implemented!");
                break;

            default:
                Debug.LogWarning("Power-up type not recognized!");
                break;
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(float boostAmount, float duration)
    {
        float originalSpeed = 5f;
        player.speed = Mathf.Min(originalSpeed + boostAmount, 20f);
        Debug.Log(boostAmount + " " + 3f);
        Debug.Log($"Speed boosted to: {player.speed}");

        yield return new WaitForSeconds(duration);

        if (player != null)
        {
            player.speed = originalSpeed;
            Debug.Log($"Speed reverted to original value: {player.speed}");
        }
    }
}
