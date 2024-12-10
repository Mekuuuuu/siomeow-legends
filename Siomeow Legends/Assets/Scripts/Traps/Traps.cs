using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public Animator anim;
    public bool trapTriggered = false;
    
    private int minDamage = 10;
    private int maxDamage = 50;

    private int playerCount = 0; // Tracks the number of players in the trigger zone
    private Coroutine damageCoroutine; // Reference to the running coroutine

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {        
        PlayerStats health = other.GetComponent<PlayerStats>();

        if (other.CompareTag("Player"))
        {
            playerCount++;
            trapTriggered = true;

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(InflictDamageOverTime(health));
            }

            Debug.Log($"Trap Triggered");
        }
        
        anim.SetBool("Triggered", trapTriggered);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerCount = Mathf.Max(0, playerCount - 1);
            trapTriggered = playerCount > 0;
            
            if (!trapTriggered && damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
                Debug.Log($"{name} Trap Reset");
            }
        }
        
        anim.SetBool("Triggered", trapTriggered);
    }
    
    private IEnumerator InflictDamageOverTime(PlayerStats health)
    {
        int damage = Random.Range(minDamage, maxDamage + 1); 
        
        while (trapTriggered)
        {
            health.TakeDamageServerRpc(damage);
            Debug.Log("Inflicting damage!");

            yield return new WaitForSeconds(1f);
        }
    }
}
