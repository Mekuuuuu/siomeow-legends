using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    private const float MOVEMENT_SPEED = 5f;
    private const float INITIAL_SLIDING_SPEED = 7f;

    public Rigidbody2D rb;

    public Animator animator;

    private Vector2 movement;
    private Vector3 slideDirection;
    private float dodgeRollSpeed;

    private State state;
    private enum State {
        Normal, 
        DodgeRollSliding,
    }

    

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }

    private void Awake(){
        state = State.Normal;
        dodgeRollSpeed = INITIAL_SLIDING_SPEED;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) 
        {
            case State.Normal:
                HandleMovement();
                HandleDodgeRoll();
                break;

            case State.DodgeRollSliding:
                HandleDodgeRollSliding();
                break;
        }
        
    }

    private void HandleMovement() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        if (movement.sqrMagnitude > 1)
        {
            movement.Normalize();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void HandleDodgeRoll() {
        if (Input.GetKey(KeyCode.Mouse1)) {
            state = State.DodgeRollSliding;
            // Go to direction of mouse click 
            // slideDir =(UtilsClass.GetMouseWorldPosition() - transform.position).Normalize;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Ensure z-axis is zero to avoid unintended movement
            slideDirection = (mouseWorldPosition - transform.position).normalized;
            
            dodgeRollSpeed = INITIAL_SLIDING_SPEED;

            // Trigger dodge roll animation
            animator.SetTrigger("DodgeRoll");
            
        }
    }

    /*
    private void HandleDodgeRollSliding() {
        transform.position += slideDir * SLIDING_SPEED * Time.fixedDeltaTime;
        SLIDING_SPEED -= SLIDING_SPEED * 10f * Time.fixedDeltaTime; 
        if (SLIDING_SPEED < 5f) {
            state = State.Normal;
        }
    }
    */

    private void HandleDodgeRollSliding()
    {
        transform.position += slideDirection * dodgeRollSpeed * Time.fixedDeltaTime;
        dodgeRollSpeed -= dodgeRollSpeed * 10f * Time.fixedDeltaTime;

        if (dodgeRollSpeed < MOVEMENT_SPEED)
        {
            state = State.Normal;

            // Reset or return to normal animation if needed
            animator.SetFloat("Speed", movement.sqrMagnitude); // Resume walking/idle animations
        }
    }

    void FixedUpdate()
    {
        if (state == State.Normal)
        {
            rb.MovePosition(rb.position + movement * MOVEMENT_SPEED * Time.fixedDeltaTime);
        }
    }
}
