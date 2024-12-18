using Unity.Netcode;
using UnityEngine;

public class RangedSpecialAttackBullet : MonoBehaviour
{
    private int minDamage = 160;
    private int maxDamage = 240;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the collider has the PlayerStats component
        if (collider.GetComponent<PlayerStats>() != null)
        {
            // Get the target PlayerStats (the player being hit)
            PlayerStats target = collider.GetComponent<PlayerStats>();

            // Get the PlayerStats of the attacker (the player who launched the attack)
            PlayerStats attacker = GetComponentInParent<PlayerAttackRanged>().Aim.GetComponentInParent<PlayerStats>();
            ulong attackerClientId = attacker.GetComponent<NetworkObject>().OwnerClientId;

            if (attacker != target)
            {
                // Apply random damage to the target and pass the attacker reference
                int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
                target.TakeDamageServerRpc(damage, attackerClientId);  // Pass the attacker to the TakeDamage method

                Debug.Log($"Attacker: {attacker.name} Target: {target.name}.");

                // Log the damage dealt and the attacker
                Debug.Log($"Special Damage: {damage} dealt by {attacker.name}.");
            }
        }
    }
}