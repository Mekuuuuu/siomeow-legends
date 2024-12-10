using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    // PLAYER STATS
    // [SerializeField] public int health = 3607;
    // [SerializeField] public int defense = 400;
    // [SerializeField] private int mana = 0;

    public NetworkVariable<int> health = new NetworkVariable<int>(3607, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> defense = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public float damageMultiplier = 1f;

    public delegate void StatsChangedDelegate();
    // public delegate void StatsChangedDelegate(int newHealth, int newDefense);
    public event StatsChangedDelegate OnStatsChanged;

    // STAT LIMITS
    public const int MAX_HEALTH = 3607;
    public const int MAX_DEFENSE = 400;
    private const int DAMAGE_REDUCTION = 50; 

    public Animator anim;
    
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int rawDamage)
    {
        rawDamage = (int)(rawDamage * damageMultiplier);

        if (rawDamage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot take negative damage.");
        }

        // Check if defense is greater than or equal to 5
        if (defense.Value >= DAMAGE_REDUCTION)
        {
            // Reduce damage by 5 if defense is greater than or equal to 5
            int damageAfterDefense = rawDamage - DAMAGE_REDUCTION;

            // Apply damage after defense reduction
            health.Value -= damageAfterDefense;

            // Reduce the defense value by 5
            defense.Value -= DAMAGE_REDUCTION;
        }
        else
        {
            // If defense is less than 5, apply full damage
            health.Value -= rawDamage;
        }

        health.Value = Mathf.Clamp(health.Value, 0, MAX_HEALTH);
        OnStatsChanged?.Invoke();
        Debug.Log($"{health.Value}");
        anim.SetBool("Damage", true); 
        StartCoroutine(ResetDamageAnimation());

        if (health.Value <= 0)
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

        health.Value = Mathf.Min(health.Value + healValue, MAX_HEALTH);

        OnStatsChanged?.Invoke();  
    }

    public void IncreaseDefense(int defenseAmount)
    {
        defense.Value = Mathf.Min(defense.Value + defenseAmount, MAX_DEFENSE);

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
