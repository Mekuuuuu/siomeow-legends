using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // PLAYER STATS
    [SerializeField] public int health = 3607;
    [SerializeField] public int defense = 400;
    // [SerializeField] private int mana = 0;

    public float damageMultiplier = 1f;

    // STAT LIMITS
    private const int MAX_HEALTH = 3607;
    private const int MAX_DEFENSE = 400;
    private const int DAMAGE_REDUCTION = 50; 

    public Animator anim;

    // Applies damage to the player, accounting for defense.
    public void TakeDamage(int rawDamage)
    {
        rawDamage = (int)(rawDamage * damageMultiplier);

        if (rawDamage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot take negative damage.");
        }

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

        anim.SetBool("Damage", true); 
        StartCoroutine(ResetDamageAnimation());

        if (this.health <= 0)
        {
            StartCoroutine(Die());
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

    private IEnumerator Die()
    {
        anim.SetBool("Dead", true); 
        yield return new WaitForSeconds(4f); 
        Destroy(gameObject);
    }
    
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Adjust time as per your animation length
        anim.SetBool("Damage", false);
    }
}
