using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // PLAYER STATS
    [SerializeField] private int health = 100;
    [SerializeField] private int defense = 50;
    // [SerializeField] private int mana = 0;

    // STAT LIMITS
    private const int MAX_HEALTH = 100;
    private const int MAX_DEFENSE = 50;
    private const int MAX_MANA = 80;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
    }

    // Applies damage to the player, accounting for defense.
    public void TakeDamage(int rawDamage)
    {
        if (rawDamage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot take negative damage.");
        }

        // Check if defense is greater than or equal to 5
        if (this.defense >= 5)
        {
            // Reduce damage by 5 if defense is greater than or equal to 5
            int damageAfterDefense = rawDamage - 5;

            // Apply damage after defense reduction
            this.health -= damageAfterDefense;

            // Reduce the defense value by 5
            this.defense -= 5;

            Debug.Log($"Damage after defense reduction: {damageAfterDefense}. Health: {this.health}, Defense: {this.defense}");
        }
        else
        {
            // If defense is less than 5, apply full damage
            this.health -= rawDamage;

            Debug.Log($"No damage reduction. Full damage: {rawDamage}. Health: {this.health}");
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
        // Temporary 
        Debug.Log("I am Dead!");
        Destroy(gameObject);
    }
}
