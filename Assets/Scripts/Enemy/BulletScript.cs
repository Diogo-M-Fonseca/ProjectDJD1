using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;  // Bullet speed
    private Vector3 direction;
    [SerializeField]
    private float lifetime = 3f;  // Bullet lifetime in seconds

    // Initialize the bullet with a direction
    public void Initialize(Vector3 dir)
    {
        direction = dir.normalized; // Normalize direction for consistent movement speed
        Debug.Log("Bullet initialized with direction: " + direction); // Debug log to confirm direction

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);  // Destroy this bullet after 'lifetime' seconds
    }

    void Update()
    {
        if (direction != Vector3.zero)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            Debug.Log("Bullet moving: " + transform.position); // Debug log to confirm movement
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            // You can apply damage or any other logic here
            ReloadScene();
        }
    }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
