using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // PLAYER STATS
    [SerializeField] public int health = 3607;
    [SerializeField] public int defense = 400;
    [SerializeField] public int killCount = 0;  // Add kill count to the player

    // To track the last player who caused damage
    private PlayerStats lastAttacker;

    // STAT LIMITS
    private const int MAX_HEALTH = 3607;
    private const int MAX_DEFENSE = 400;
    private const int DAMAGE_REDUCTION = 50;

    // Applies damage to the player, accounting for defense.
    public void TakeDamage(int rawDamage, PlayerStats attacker)
    {
        if (rawDamage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot take negative damage.");
        }

        // Record the last attacker
        lastAttacker = attacker;

        // Check if defense is greater than or equal to 5
        if (this.defense >= DAMAGE_REDUCTION)
        {
            // Reduce damage by 5 if defense is greater than or equal to 5
            int damageAfterDefense = rawDamage - DAMAGE_REDUCTION;

            // Apply damage after defense reduction
            this.health -= damageAfterDefense;

            // Reduce the defense value by 5
            this.defense -= DAMAGE_REDUCTION;
        }
        else
        {
            // If defense is less than 5, apply full damage
            this.health -= rawDamage;
        }

        if (this.health <= 0)
        {
            Die();
        }
    }

    public void Heal(int healValue)
    {
        if (healValue < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing.");
        }

        bool wouldBeOverMaxHealth = this.health + healValue > MAX_HEALTH;

        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += healValue;
        }
    }

    private void Die()
    {
        // When the player dies, increment the kill count of the last attacker
        if (lastAttacker != null)
        {
            lastAttacker.IncrementKillCount();
        }

        // Destroy the player object after death
        Destroy(gameObject);
    }

    public void IncrementKillCount()
    {
        killCount++;
        Debug.Log($"{gameObject.name}'s Kill Count: {killCount}");
    }
}
