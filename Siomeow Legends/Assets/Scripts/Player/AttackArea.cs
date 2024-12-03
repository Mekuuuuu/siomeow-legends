using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private int damage = 10;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats health = collider.GetComponent<PlayerStats>();
            health.TakeDamage(damage);
            Debug.Log($"Damage: {damage}");
        }
    }
}
