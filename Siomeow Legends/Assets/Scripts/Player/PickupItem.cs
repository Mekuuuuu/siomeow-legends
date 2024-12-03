using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PowerUp { Berserk, Movement, Stamina, Heal, Shield }
    public PowerUp type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player == null)
            {
                Debug.LogWarning("PlayerMovement component not found on the player.");
                return;
            }
            PowerUpsHandler powerUpsHandler = player.GetComponent<PowerUpsHandler>();
            powerUpsHandler.Initialize(player);
            powerUpsHandler.ApplyPowerUp(type);

            Destroy(gameObject);
        }
    }
}
