using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private GameObject attackArea = default;

    private bool isAttacking = false;

    private float timeToAttack = 0.25f;

    private float timer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

        if (isAttacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                isAttacking = false;
                attackArea.SetActive(isAttacking);
            }
        }
    }

    private void Attack()
    {
        isAttacking = true; 
        attackArea.SetActive(isAttacking);
        Debug.Log("Attacking");
    }
}
