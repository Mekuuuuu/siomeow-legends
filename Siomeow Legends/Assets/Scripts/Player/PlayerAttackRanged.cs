using Unity.Netcode;
using UnityEngine;

public class PlayerAttackRanged : NetworkBehaviour
{
    // Projectile Aim 
    public Transform Aim;

    // Objects that shoot out 
    public GameObject bullet;
    public GameObject specialBullet; 
    public float fireForce = 10f; 

    // Normal Attack
    private bool isAttacking = false;
    float shootCooldown = 0.25f;
    float shootTimer = 0.5f;

    // Special Attack 
    private bool isSpecialAttacking = false;
    private float specialAttackCooldown = 5f;
    private float specialAttackTimer = 0f;
    private float specialAttackDuration = 0.65f;

    // Reference direction for the projectile. Primarily faces right. 
    private int facingDirection = 1; // 1 = right, -1 = left

    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        CheckShootingTimer();
        CheckSpecialAttackCooldown(); 

        // Update facing direction based on input (replace with your movement logic)
        if (Input.GetKey(KeyCode.A)) // Moving left
        {
            facingDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D)) // Moving right
        {
            facingDirection = 1;
        }

        // Shooot
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnShoot();
        }
        else if (Input.GetKeyDown(KeyCode.K) && !isSpecialAttacking && specialAttackTimer <= 0)
        {
            SpecialAttack();
        }
    }

    void OnShoot()
    {
        if(!isAttacking)
        {
            // Instantiate bullet
            GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation, Aim);

            // Adjust bullet's shooting direction based on where the character is facing 
            Vector2 shootingDirection = Aim.right * facingDirection;
            intBullet.GetComponent<Rigidbody2D>().AddForce(shootingDirection * fireForce, ForceMode2D.Impulse);

            // Adjust bullet's rotation to face the correct direction
            if (facingDirection < 0) 
            {
                intBullet.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Flip horizontally
            }
            else 
            {
                intBullet.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Default rotation
            }

            // Trigger animations
            isAttacking = true; 
            anim.SetBool("Attack", isAttacking); 

            // Removes bullet after 2 seconds
            Destroy(intBullet, 2f);
        }
    }

    private void CheckShootingTimer()
    {
        if (isAttacking)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >=  shootCooldown)
            {
                shootTimer = 0;
                isAttacking = false;
                anim.SetBool("Attack", isAttacking); 
            }
        }
    }

    private void SpecialAttack()
    {
        if(!isSpecialAttacking)
        {
            // Instantiate bullet
            GameObject intSpecialBullet = Instantiate(specialBullet, Aim.position, Aim.rotation, Aim);

            // Adjust bullet's shooting direction based on where the character is facing 
            Vector2 shootingDirection = Aim.right * facingDirection;
            intSpecialBullet.GetComponent<Rigidbody2D>().AddForce(shootingDirection * fireForce, ForceMode2D.Impulse);

            // Adjust bullet's rotation to face the correct direction
            if (facingDirection < 0) 
            {
                intSpecialBullet.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Flip horizontally
            }
            else 
            {
                intSpecialBullet.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Default rotation
            }

            // Trigger animations
            isSpecialAttacking = true; 
            anim.SetBool("SpecialAttack", isSpecialAttacking); 

            // Removes bullet after 2 seconds
            Destroy(intSpecialBullet, 2f);
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
                anim.SetBool("SpecialAttack", isSpecialAttacking);
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
}
