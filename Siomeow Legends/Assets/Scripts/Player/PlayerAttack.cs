using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;
    private GameObject specialAttackArea = default;

    private bool isAttacking = false;
    private float attackDuration = 0.35f;
    private float attackTimer = 0f;

    private bool isSpecialAttacking = false;
    private float specialAttackDuration = 1.0f;
    private float specialAttackCooldown = 5f;
    private float specialAttackTimer = 0f;

    public Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        specialAttackArea = transform.GetChild(1).gameObject;

        attackArea.SetActive(isAttacking);
        specialAttackArea.SetActive(isSpecialAttacking);
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttackTimer();
        CheckSpecialAttackCooldown();

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.K) && !isSpecialAttacking && specialAttackTimer <= 0)
        {
            SpecialAttack();
        }
    }

    private void Attack()
    {
        if (!isAttacking)
        {            
            isAttacking = true; 
            attackArea.SetActive(isAttacking);
            Animate();
        }
        
    }

    private void SpecialAttack()
    {
        if (!isSpecialAttacking)
        {
            isSpecialAttacking = true;
            specialAttackArea.SetActive(isSpecialAttacking);
            Animate();
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
                anim.SetBool("Attack", isAttacking); 
            }
        }
    }

    private void CheckSpecialAttackCooldown()
    {
        if (isSpecialAttacking)
        {
            specialAttackTimer += Time.deltaTime;

            if (specialAttackTimer >= specialAttackDuration)
            {
                isSpecialAttacking = false;
                specialAttackArea.SetActive(isSpecialAttacking);
                anim.SetBool("Special", isSpecialAttacking);
            }
        }

        // Reset the cooldown timer when cooldown is complete
        if (specialAttackTimer > 0 && specialAttackTimer < specialAttackCooldown)
        {
            specialAttackTimer += Time.deltaTime;
        }
        else if (specialAttackTimer >= specialAttackCooldown)
        {
            specialAttackTimer = 0; // Cooldown complete
        }
    }

    private void Animate()
    {
        if(isAttacking)
        {
            anim.SetBool("Attack", isAttacking); 
        }
        else if(isSpecialAttacking)
        {
            anim.SetBool("Special", isSpecialAttacking); 
        }
    }
}
