using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int minDamage = 90;
    private int maxDamage = 120;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats health = collider.GetComponent<PlayerStats>();
            
            int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
            health.TakeDamage(damage);

            Debug.Log($"Damage: {damage}");
        }
    }
}
