using UnityEngine;

public class RangedAttackBullet : MonoBehaviour
{
    private int minDamage = 30;
    private int maxDamage = 60;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<PlayerStats>() != null)
        {
            PlayerStats health = collider.GetComponent<PlayerStats>();

            int damage = Random.Range(minDamage, maxDamage + 1); // Add 1 to include maxDamage in the range
            health.TakeDamageServerRpc(damage);

            Destroy(gameObject);

            Debug.Log($"Normal Damage: {damage}");
        }
    }
}