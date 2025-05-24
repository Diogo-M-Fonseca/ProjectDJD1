using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayer
{
    public float Speed => speed;
    public float JumpForce => jumpForce;
    public float MaxJumpTime => maxJumpTime;
    public float JumpTime
    {
        get => jumpTime;
        set => jumpTime = value;
    }
    public float speed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float jumpTime = 0;

    private Rigidbody2D rigidbody;
    public bool isGrounded;
    public Transform GroundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Quaternion initialRotation;

    private float recoilLockTimer = 0f; // Timer to prevent input during recoil
    public float recoilLockDuration = 0.1f; // Duration to pause movement after recoil

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);

        // Reduce recoil lock timer
        if (recoilLockTimer > 0)
        {
            recoilLockTimer -= Time.deltaTime;
        }

        float moveX = Input.GetAxis("Horizontal");

        // Only apply movement if not locked by recoil
        if (recoilLockTimer <= 0)
        {
            rigidbody.linearVelocity = new Vector2(moveX * speed, rigidbody.linearVelocity.y);
        }

        // Flip logic
        if (moveX < 0 && transform.right.x > 0)
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 180f, 0f);
        }
        else if (moveX > 0 && transform.right.x < 0)
        {
            transform.rotation = initialRotation * Quaternion.Euler(0f, 0f, 0f);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, jumpForce);
        }
        if (Input.GetButtonUp("Jump") && rigidbody.linearVelocity.y > 0)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, 0);
        }
    }

    // Call this from recoil script after applying recoil
    public void LockMovementTemporarily()
    {
        recoilLockTimer = recoilLockDuration;
    }
}
