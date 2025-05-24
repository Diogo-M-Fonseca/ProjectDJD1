using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure Rigidbody2D isn't sleeping (critical for consistent movement)
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    public void SetDirection(Vector2 direction)
    {
        direction = direction.normalized;
        rb.linearVelocity = direction * speed;
        Debug.Log($"Applied Velocity: {rb.linearVelocity}"); // Verify velocity is correct
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
    if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
        Destroy(gameObject);
    }
    }

}