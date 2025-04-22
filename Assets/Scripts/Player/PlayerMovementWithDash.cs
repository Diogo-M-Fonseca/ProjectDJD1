using System.Collections;
using UnityEngine;

public class PlayerMovementWithDash : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float jumpTime = 0;
    private Rigidbody2D rigidbody;
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
        rigidbody = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation; // Store the initial rotation
    }

    void Update()
    {
        // Check if the player is grounded using the GroundCheck collider
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);

        // Movement_X (Left and Right)
        float moveX = Input.GetAxis("Horizontal");
        if (!isDashing) // Only apply movement when not dashing
        {
            rigidbody.linearVelocity = new Vector2(moveX * speed, rigidbody.linearVelocity.y); // Apply movement
        }

        // Flip logic using transform.right to check the direction
        if (moveX < 0 && transform.right.x > 0) // If moving left but facing right
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 180f, 0f); // Flip character
        }
        else if (moveX > 0 && transform.right.x < 0) // If moving right but facing left
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 0f); // Reset rotation to original
        }

        // Movement_Jump (Upward)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce); // Jump force applied only on the Y-axis
        }
        if (Input.GetButtonUp("Jump") && rigidbody.linearVelocity.y > 0)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0); // Stop upward movement when jump button is released
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

    // Save the player's gravity scale and set it to 0 to ignore gravity during dash
    float originalGravity = rigidbody.gravityScale;
    rigidbody.gravityScale = 0f;

    // Get the world position of the mouse cursor
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // Calculate the direction from the player to the mouse position
    Vector2 dashDirection = (mousePosition - (Vector2)transform.position).normalized;

    // Apply dash velocity in the direction of the mouse
    rigidbody.linearVelocity = dashDirection * dashingPower;

    // Wait for the duration of the dash
    yield return new WaitForSeconds(dashingTime);

    // Restore gravity after dash
    rigidbody.gravityScale = originalGravity;

    // Stop the dash by setting velocity to zero
    rigidbody.linearVelocity = Vector2.zero;
    isDashing = false;

    // Wait for cooldown time before allowing another dash
    yield return new WaitForSeconds(dashingCooldown);
    canDash = true;
    }

}
