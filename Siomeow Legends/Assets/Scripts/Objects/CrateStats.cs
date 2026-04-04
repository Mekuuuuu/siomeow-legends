using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CrateStats : NetworkBehaviour
{
    // CRATE STATS
    public NetworkVariable<int> health = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // STAT LIMITS
    public const int MAX_HEALTH = 1;
    private bool isBreak = false;

    public Animator anim;

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(int rawDamage, ulong attackerClientId)
    {
        if (rawDamage < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rawDamage), "Cannot take negative damage.");
        }

        // Apply the damage to the crate's health
        health.Value -= rawDamage;

        Debug.Log($"Crate Health: {health.Value}");
        AudioManager.instance.PlayDamage();

        // Check if crate health has reached 0
        if (health.Value <= 0)
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        if (isBreak) yield break;
        isBreak = true;

        // Play death animation
        anim.SetBool("Broken", true);
        yield return new WaitForSeconds(0.5f);

        // Properly despawn the networked object
        if (IsServer)
        {
            GetComponent<LootCrate>()?.DropPotion(); 
    
            // Despawn the crate to sync destruction across all clients
            GetComponent<NetworkObject>().Despawn();
        }
    }
}
