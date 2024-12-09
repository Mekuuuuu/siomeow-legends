using Unity.Netcode;
using UnityEngine;

public class AttackArea : NetworkBehaviour
{
    private int minDamage = 90;
    private int maxDamage = 120;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats playerStats = collider.GetComponent<PlayerStats>();

            int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
            // health.TakeDamage(damage);

            Debug.Log($"Normal Damage: {damage}");

            NetworkObject targetNetworkObject = playerStats.GetComponent<NetworkObject>();

            NetworkObjectReference targetNetworkObjectRef = targetNetworkObject;


            ApplyDamageServerRpc(damage, targetNetworkObjectRef);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ApplyDamageServerRpc(int damage, NetworkObjectReference targetNetworkObjectRef)
    {
        // Ensure the target player stats is valid
        if (targetNetworkObjectRef.TryGet(out NetworkObject targetNetworkObject) && targetNetworkObject != null)
        {
            // Get the PlayerStats component on the target NetworkObject
            if (targetNetworkObject.TryGetComponent<PlayerStats>(out var targetPlayerStats))
            {
                // Apply damage to the target player
                targetPlayerStats.TakeDamage(damage);
            }
        }
    }
}
