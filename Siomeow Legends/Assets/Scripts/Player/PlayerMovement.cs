using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D body;

    public Animator anim;

    private bool isFacingRight = true;
    private bool moving;

    private bool canDash = true;
    private bool isDashing; 
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private void Awake() 
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if(isDashing)
        {
            return;
        }

        // Get input axis values
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Normalize the direction and apply speed
        Vector2 velocity = inputDirection.normalized * speed;

        // Animate running 
        Animate(inputDirection);

        // Set the Rigidbody2D velocity
        body.linearVelocity = velocity;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) 
        {
            StartCoroutine(Dash());
        }

        Flip(inputDirection.x);
    }

    private void Animate(Vector2 input)
    {
        if(input.magnitude > 0.1f || input.magnitude < -0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }



        if(moving)
        {
            anim.SetFloat("Horizontal", input.x);
            anim.SetFloat("Vertical", input.y);
        }

        anim.SetBool("Moving", moving);
    }

    private void Flip(float inputX)
    {
        if (isFacingRight && inputX < 0f || !isFacingRight && inputX > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        body.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}