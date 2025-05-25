using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayer
{
    [SerializeField]
    private Animator animator;
    private bool isRunning = false;
    public float Speed => speed;
    public float JumpForce => jumpForce;
    public float MaxJumpTime => maxJumpTime;
    private bool jump = false;
    public float JumpTime
    {
        get => jumpTime;
        set => jumpTime = value;
    }
    public float speed = 5f;
    public float jumpForce = 10f;
    public float maxJumpTime = 0.5f;
    public float jumpTime = 0;

    private Rigidbody2D rb;
    public bool isGrounded;
    public Transform GroundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Quaternion initialRotation;

    private float recoilLockTimer = 0f; // Timer to prevent input during recoil
    public float recoilLockDuration = 0.1f; // Duration to pause movement after recoil

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialRotation = transform.rotation;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("OnGround", isGrounded);
        // Reduce recoil lock timer
        if (recoilLockTimer > 0)
        {
            recoilLockTimer -= Time.deltaTime;
        }

        float moveX = Input.GetAxis("Horizontal");
        animator.SetFloat("IsRunning", moveX);
        // Only apply movement if not locked by recoil
        if (recoilLockTimer <= 0)
        {
            rb.linearVelocity = new Vector2(moveX * speed, rb.linearVelocity.y);
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jump = true;
            animator.SetBool("Jump", jump);
        }
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            jump = false;
            animator.SetBool("Jump", jump);
        }
    }

    // Call this from recoil script after applying recoil
    public void LockMovementTemporarily()
    {
        recoilLockTimer = recoilLockDuration;
    }
}
