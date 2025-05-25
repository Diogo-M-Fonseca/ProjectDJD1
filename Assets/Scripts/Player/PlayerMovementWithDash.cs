using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementWithDash : MonoBehaviour, IPlayer
{
    public float Speed => speed;
    public float JumpForce => jumpForce;
    public float MaxJumpTime => maxJumpTime;
    public float JumpTime
    {
        get => jumpTime;
        set => jumpTime = value;
    }
    [SerializeField]
    private Animator animator;
    private bool isAlive = true;
    private bool isRunning = false;
    public float speed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float jumpTime = 0;
    public AudioClip dashSound;          // Reference to the dash sound clip
    private AudioSource dashAudioSource;  // AudioSource for the dash sound
    private Rigidbody2D rb;
    private bool isGrounded;
    public Transform GroundCheck; // The GroundCheck GameObject reference
    public float groundCheckRadius = 0.2f; // The radius of the GroundCheck circle (adjust as needed)
    public LayerMask groundLayer; // The layer mask for ground objects
    private Quaternion initialRotation; // To store the player's initial rotation
    internal bool canDash = true; // Variable to manage dash cooldown
    private bool isDashing = false;
    
    [SerializeField]
    private float dashingPower = 24f; // The force of the dash
    [SerializeField]
    private float dashingTime = 0.2f; // The duration of the dash
    [SerializeField]
    private float dashingCooldown = 1f; // The cooldown time for dashing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation; // Store the initial rotation

        // Dynamically find the AudioSource on the same GameObject
        dashAudioSource = GetComponent<AudioSource>();

        // If no AudioSource is found, log a warning
        if (dashAudioSource == null)
        {
            Debug.LogWarning("No AudioSource component found on this GameObject. Dash sound will not play.");
        }
    }

    void Update()
    {
        // Check if the player is grounded using the GroundCheck collider
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("OnGround", isGrounded);
        // Movement_X (Left and Right)
        float moveX = Input.GetAxis("Horizontal");
        if (!isDashing) // Only apply movement when not dashing
        {
            rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y); // Apply movement
        }
        animator.SetBool("IsRunning", isRunning);
        // Flip logic using transform.right to check the direction
        if (moveX < 0 && transform.right.x > 0) // If moving left but facing right
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 180f, 0f); // Flip character
            isRunning = true;
        }
        else if (moveX > 0 && transform.right.x < 0) // If moving right but facing left
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 0f); // Reset rotation to original
            isRunning = true;
        }
        else if (moveX == 0)
        {
            isRunning = false;
        }

        // Movement_Jump (Upward)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Jump force applied only on the Y-axis
        }
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Stop upward movement when jump button is released
        }

        // Trigger the dash when right mouse button (or another input) is pressed
        if (Input.GetMouseButtonDown(1) && canDash && !isDashing)
        {
            StartCoroutine(Dash()); // Start the dash coroutine

        }
    }

    private IEnumerator Dash()
    {
        // Prevent dashing while already dashing or on cooldown
        canDash = false;
        isDashing = true;
        animator.SetBool("IsDashing", isDashing);

        // Play the dash sound when dash happens
        if (dashAudioSource != null && dashSound != null)
        {
            dashAudioSource.PlayOneShot(dashSound);  // Play the dash sound
        }

        // Save the player's gravity scale and set it to 0 to ignore gravity during dash
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Get the world position of the mouse cursor
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate the direction from the player to the mouse position
        Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized;

        // Apply dash velocity in the direction of the mouse
        rb.linearVelocity = dashDirection * dashingPower;

        // Wait for the duration of the dash
        yield return new WaitForSeconds(dashingTime);

        // Restore gravity after dash
        rb.gravityScale = originalGravity;

        // Stop the dash by setting velocity to zero
        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        // Wait for cooldown time before allowing another dash
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        animator.SetBool("IsDashing", isDashing);
    }
}
