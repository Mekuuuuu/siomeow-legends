using UnityEngine;

public class PlayerAttackRanged : MonoBehaviour
{
    public Transform Aim;
    public GameObject bullet; 
    public float fireForce = 10f;
    private bool isAttacking = false;
    float shootCooldown = 0.15f;
    float shootTimer = 0.3f;

    private GameObject specialAttackArea = default;
    private bool isSpecialAttacking = false;
    private float specialAttackDuration = 0.65f;
    private float specialAttackCooldown = 5f;
    private float specialAttackTimer = 0f;

    private int facingDirection = 1; // 1 = right, -1 = left

    public Animator anim;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Aim = transform.GetChild(0).gameObject; 
        // specialAttackArea = transform.GetChild(1).gameObject;

        // specialAttackArea.SetActive(isSpecialAttacking);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;

        // Update facing direction based on input (replace with your movement logic)
        if (Input.GetKey(KeyCode.A)) // Moving left
        {
            facingDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D)) // Moving right
        {
            facingDirection = 1;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            OnShoot();
        }
    }

    void OnShoot()
    {
        if(shootTimer >  shootCooldown)
        {
            shootTimer = 0;
            GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
            Vector2 shootingDirection = Aim.right * facingDirection;
            intBullet.GetComponent<Rigidbody2D>().AddForce(shootingDirection * fireForce, ForceMode2D.Impulse);

            // Adjust bullet's rotation to face the correct direction
            if (facingDirection < 0) // Player is facing left
            {
                intBullet.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Flip horizontally
            }
            else // Player is facing right
            {
                intBullet.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Default rotation
            }

            isAttacking = true;
            Animate();

            Destroy(intBullet, 2f);
        }
    }

    /*
    private void SpecialAttack()
    {
        if (!isSpecialAttacking)
        {
            isSpecialAttacking = true;
            specialAttackArea.SetActive(isSpecialAttacking);
            // Animate();
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
    */

    private void Animate()
    {
        if(isAttacking)
        {
            anim.SetBool("Attack", isAttacking); 
        }
    }
    
}