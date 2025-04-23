using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float jumpTime = 0;
    private Rigidbody2D rigidbody;
    public bool isGrounded;
    public Transform GroundCheck; // The GroundCheck GameObject reference
    public float groundCheckRadius = 0.2f; // The radius of the GroundCheck circle (adjust as needed)
    public LayerMask groundLayer; // The layer mask for ground objects
    private Quaternion initialRotation; // To store the player's initial rotation


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
        rigidbody.linearVelocity = new Vector2(moveX * speed, rigidbody.linearVelocity.y); // Apply movement

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
 

    }

}
