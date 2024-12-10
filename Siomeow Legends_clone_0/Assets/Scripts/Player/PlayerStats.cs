using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    // PLAYER STATS
    public NetworkVariable<int> health = new NetworkVariable<int>(3607, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> defense = new NetworkVariable<int>(400, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] public int killCount = 0;  // Add kill count to the player

    // To track the last player who caused damage
    private ulong lastAttackerClientId;

    public float damageMultiplier = 1f;

    public delegate void StatsChangedDelegate();
    // public delegate void StatsChangedDelegate(int newHealth, int newDefense);
    public event StatsChangedDelegate OnStatsChanged;
    public static event Action<ulong> OnPlayerDied;

    // STAT LIMITS
    public const int MAX_HEALTH = 3607;
    public const int MAX_DEFENSE = 400;
    public const int DAMAGE_REDUCTION = 50;
    private bool isDead = false;

    public Animator anim;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int rawDamage, ulong attackerClientId)
    {
        rawDamage = (int)(rawDamage * damageMultiplier);

        if (rawDamage < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot take negative damage.");
        }

        // Record the last attacker
        lastAttackerClientId = attackerClientId;

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
        if (isDead) yield break;
        isDead = true;
        // When the player dies, increment the kill count of the last attacker
        foreach (var client in HostManager.Instance.ClientData)
        {
            if (client.Key == lastAttackerClientId)
            {
                client.Value.IncrementKillCount();
                Debug.Log($"ClientId {client.Value.clientId} has killed. Kill counter: {client.Value.killCount}");
            }
        }

        // Destroy the player object after death
        anim.SetBool("Dead", true); 

        OnPlayerDied?.Invoke(OwnerClientId);
        yield return new WaitForSeconds(3f); 

        health.Value = MAX_HEALTH;
        defense.Value = MAX_DEFENSE;
        // Destroy(gameObject);
        anim.SetBool("Dead", false);
        isDead = false;
    }

    public void IncrementKillCount()
    {
        killCount++;
        Debug.Log($"{gameObject.name}'s Kill Count: {killCount}");
    }
    
    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(0.5f); 
        anim.SetBool("Damage", false);
    }
}
