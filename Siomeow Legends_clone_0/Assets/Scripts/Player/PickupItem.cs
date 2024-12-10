using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickupItem : NetworkBehaviour
{
    public enum PowerUp { Berserk, Movement, Stamina, Heal, Shield }
    public PowerUp type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (!IsServer) return; // Only the server handles the pickup logic

        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (player == null)
            {
                Debug.LogWarning("PlayerMovement component not found on the player.");
                return;
            }
            PowerUpsHandler powerUpsHandler = player.GetComponent<PowerUpsHandler>();
            powerUpsHandler.Initialize(player, playerStats);
            powerUpsHandler.ApplyPowerUp(type);

            // Despawn the item on the network
            DespawnItemServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnItemServerRpc()
    {
        // Ensure this logic runs only on the server
        if (IsSpawned)
        {
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
