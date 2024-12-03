using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool isAttacking = false;

    private float attackDuration = 0.25f;

    private float attackTimer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        CheckAttackTimer();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {
            attackArea.SetActive(isAttacking);
            isAttacking = true; 
            // Animator here for attack
            Debug.Log("Attacking");
        }
        
    }

    private void CheckAttackTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                attackArea.SetActive(isAttacking);
            }
        }
    }
}
