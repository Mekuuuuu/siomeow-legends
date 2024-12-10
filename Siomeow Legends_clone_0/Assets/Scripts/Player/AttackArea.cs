using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int minDamage = 90;
    private int maxDamage = 120;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats target = collider.GetComponent<PlayerStats>();

            // Get the player who initiated the special attack (the one triggering the attack)
            PlayerStats attacker = GetComponentInParent<PlayerStats>();  

            // Apply random damage to the target and pass the attacker reference
            int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
            target.TakeDamage(damage, attacker);

            Debug.Log($"Normal Damage: {damage} dealt by {attacker.name}.");
        }
    }
}
