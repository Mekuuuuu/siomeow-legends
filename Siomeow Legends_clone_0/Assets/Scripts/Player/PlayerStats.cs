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

    public delegate void StatsChangedDelegate();
    public event StatsChangedDelegate OnStatsChanged;

    // STAT LIMITS
    public const int MAX_HEALTH = 3607;
    public const int MAX_DEFENSE = 400;
    private const int DAMAGE_REDUCTION = 50; 

    public Animator anim;

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

        this.health = Mathf.Clamp(this.health, 0, MAX_HEALTH);
        OnStatsChanged?.Invoke();
        Debug.Log($"{this.health}");
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

        this.health = Mathf.Min(this.health + healValue, MAX_HEALTH);

        OnStatsChanged?.Invoke();  
    }

    public void IncreaseDefense(int defenseAmount)
    {
        this.defense = Mathf.Min(this.defense + defenseAmount, MAX_DEFENSE);

        OnStatsChanged?.Invoke();
    }

    private IEnumerator Die()
    {
        anim.SetBool("Dead", true); 
        yield return new WaitForSeconds(3f); 
        Destroy(gameObject);
    }
    
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f); 
        anim.SetBool("Damage", false);
    }
}
