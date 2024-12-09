using UnityEngine;
using System.Collections;
using Unity.Netcode;
using Cinemachine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    private Rigidbody2D body;

    public Animator anim;

    private bool isFacingRight = true;
    private bool moving;

    public bool canDash = true;
    public bool isDashing; 
    public float dashingCooldown = 10f;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float lastDashTime = -Mathf.Infinity; 

    [SerializeField] private TrailRenderer tr;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private AudioListener listener;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;
        }
        else
        {
            vc.Priority = 0;
        }
    }

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

        // Allow dash after 10 seconds
        if (Time.time - lastDashTime >= dashingCooldown && !canDash)
        {
            canDash = true;  
        }
        
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
        if (!canDash) yield break; 
        canDash = false;
        isDashing = true;
        lastDashTime = Time.time; 
        body.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        isDashing = false;
    }
}