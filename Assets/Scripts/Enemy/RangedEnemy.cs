using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float detectionRange = 10f;  // Start chasing at this distance
    public float stopRange = 15f;       // Stop chasing beyond this distance
    public float speed = 3f;            // Movement speed
    public float shootRange = 5f;       // Shooting range for ranged attack
    public GameObject bulletPrefab;     // Bullet prefab for shooting
    public Transform firePoint;         // Fire point for bullets
    [SerializeField] private Animator animator;

    private GameObject targetPlayer;        // Reference to the player GameObject
    private IPlayer targetPlayerScript;     // Reference to the IPlayer interface
    private bool isChasing = false;         // Are we currently chasing?

    private float shootCooldown = 0f;       // Cooldown for shooting
    public float shootRate = 1f;            // Rate of fire (bullets per second)

    private bool isBlocked = false;         // Is the enemy blocked by a wall?

    [SerializeField]
    private Collider2D wallChecker;         // Wall checker collider (set this in the Inspector)

    private float shootDelayTimer = 0f;     // Timer to track the delay before shooting

    private void Start()
    {
        FindPlayerByInterface();
    }

    private void Update()
    {
        if (targetPlayer == null)
        {
            FindPlayerByInterface(); // Try to re-find player if null
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        // Prioritize shooting if in range
        if (distanceToPlayer <= shootRange)
        {
            shootDelayTimer += Time.deltaTime;

            if (shootDelayTimer >= 1f && shootCooldown <= 0f)
            {
                ShootAtPlayer();
                shootCooldown = shootRate;
                shootDelayTimer = 0f;
            }
            else
            {
                shootCooldown -= Time.deltaTime;
            }

            return;
        }

        // Stop chasing if player is too far or blocked
        if ((distanceToPlayer > stopRange && isChasing) || isBlocked)
        {
            StopChasing();
        }

        // Start chasing if player is within detection range
        if (distanceToPlayer <= detectionRange && !isChasing && !isBlocked)
        {
            StartChasing();
        }

        // Chase if in chase mode and not blocked
        if (isChasing && !isBlocked)
        {
            ChasePlayer();
        }

        // Flip sprite to face player
        FlipEnemySpriteTowardPlayer();
    }

    private void FindPlayerByInterface()
    {
    MonoBehaviour[] allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
    foreach (MonoBehaviour behaviour in allBehaviours)
    {
        if (behaviour is IPlayer player)
        {
            targetPlayerScript = player;
            targetPlayer = behaviour.gameObject;
            return;
        }
    }

    }


    void StartChasing()
    {
        isChasing = true;
    }

    void StopChasing()
    {
        isChasing = false;
    }

    void ChasePlayer()
    {
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        direction.y = 0; // Optional: prevent vertical movement
        transform.position += direction * speed * Time.deltaTime;
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null || targetPlayer == null) return;

        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.y += 30f; // Adjust target height if needed

        Vector3 direction = targetPosition - firePoint.position;
        direction.z = 0;
        direction.Normalize();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().Initialize(direction);
    }

    private void FlipEnemySpriteTowardPlayer()
    {
        if (targetPlayer == null) return;

        float direction = targetPlayer.transform.position.x - transform.position.x;
        if (direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isBlocked = true;
            StopChasing();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isBlocked = false;
        }
    }
}
