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
    [SerializeField] private GameObject confiner;
    [SerializeField] private AudioListener listener;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            listener.enabled = true;
            vc.Priority = 1;

            SetupConfinerServerRpc();
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
        if (!IsOwner) return;
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
        

        FlipServerRpc(inputDirection.x);
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

    [ServerRpc(RequireOwnership = false)]
    private void FlipServerRpc(float inputX)
    {
        FlipClientRpc(inputX);
    }

    [ClientRpc]
    private void FlipClientRpc(float inputX)
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
    [ServerRpc(RequireOwnership = false)]
    private void SetupConfinerServerRpc()
    {
        // Ensure the confiner is properly assigned on the server
        SetupConfiner();

        SetupConfinerClientRpc();
    }

    [ClientRpc]
    private void SetupConfinerClientRpc()
    {
        SetupConfiner();
    }

    private void SetupConfiner()
    {
        if (vc == null)
        {
            Debug.LogError("Virtual Camera is not assigned.");
            return;
        }

        var confiner2D = vc.GetComponent<CinemachineConfiner2D>();
        if (confiner2D == null)
        {
            Debug.LogError("CinemachineConfiner2D component not found on the Virtual Camera.");
            return;
        }

        // Dynamically find the Confiner if it's not assigned
        if (confiner == null)
        {
            confiner = GameObject.FindWithTag("Confiner");
            if (confiner == null)
            {
                Debug.LogError("Confiner GameObject is not assigned and could not be found.");
                return;
            }
        }

        // Ensure the Confiner has a Collider2D
        Collider2D collider = confiner.GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("Confiner GameObject does not have a Collider2D component.");
            return;
        }

        // Assign the Collider2D to the Confiner2D component
        confiner2D.m_BoundingShape2D = collider;

        // Invalidate the cache to apply changes
        confiner2D.InvalidateCache();

        Debug.Log("Confiner setup completed successfully.");
    }

}