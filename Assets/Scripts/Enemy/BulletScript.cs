using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;  // Bullet speed

    private Vector2 direction;
    private Rigidbody2D rb;

    [SerializeField]
    private float lifetime = 3f;  // Bullet lifetime in seconds

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep; // Optional: keep awake for smooth movement
        rb.gravityScale = 0f; // Usually bullets should not be affected by gravity
    }

    // Initialize the bullet with a direction vector
    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;

        // Set rotation so the bullet faces movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Apply velocity to Rigidbody2D
        rb.linearVelocity = direction * speed;

        // Destroy bullet after lifetime seconds
        Destroy(gameObject, lifetime);
    }

    // Optional: You can add OnTriggerEnter2D here for collision detection
    private void OnTriggerEnter2D(Collider2D other)
{
    // Check if the collided object implements IPlayer interface
    IPlayer player = other.GetComponent<IPlayer>();
    if (player != null)
    {

        ReloadScene();
    }

    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    {
        Destroy(gameObject); // Destroy bullet on ground hit
    }
}


    private void ReloadScene()
    {
        var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
