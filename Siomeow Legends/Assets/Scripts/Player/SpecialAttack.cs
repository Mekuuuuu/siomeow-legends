using Unity.Netcode;
using UnityEngine;

public class SpecialAttackArea : NetworkBehaviour
{
    private int minDamage = 120;
    private int maxDamage = 150;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            
            int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
            // health.TakeDamage(damage);

            Debug.Log($"Special Damage: {damage}");

            NetworkObject targetNetworkObject = playerStats.GetComponent<NetworkObject>();

            // ApplyDamageServerRpc(damage, targetNetworkObject);
        }
    }

    // [ServerRpc]
    // private void ApplyDamageServerRpc(int damage, NetworkObject targetNetworkObject)
    // {
    //     // Ensure the target player stats is valid
    //     if (targetNetworkObject != null && targetNetworkObject.TryGetComponent<PlayerStats>(out var targetPlayerStats))
    //     {
    //         // Apply damage to the target player
    //         targetPlayerStats.TakeDamage(damage);
    //     }
    // }
}
