using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public float detectionRange = 10f;  // Start chasing at this distance
    public float stopRange = 15f;       // Stop chasing beyond this distance
    public float speed = 3f;            // Movement speed
    public float shootRange = 5f;       // Shooting range for ranged attack
    public GameObject bulletPrefab;     // Bullet prefab for shooting
    public Transform firePoint;         // Fire point for bullets

    private GameObject[] players;       // All players in scene
    private GameObject targetPlayer;    // Closest player
    private bool isChasing = false;     // Are we currently chasing?

    private float shootCooldown = 0f;   // Cooldown for shooting
    public float shootRate = 1f;        // Rate of fire (bullets per second)

    private bool isBlocked = false;     // Is the enemy blocked by a wall?
    
    [SerializeField]
    private Collider2D wallChecker;     // Wall checker collider (set this in the Inspector)

    private float shootDelayTimer = 0f; // Timer to track the delay before shooting

    private void Start()
    {
        players = FindPlayers();
        ChooseTargetPlayer();
    }

    private void Update()
    {
        if (targetPlayer == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        // Prioritize shooting if in range
        if (distanceToPlayer <= shootRange)
        {
            // Increase the timer by the time passed since the last frame
            shootDelayTimer += Time.deltaTime;

            // Check if the 1 second delay has passed and the cooldown is finished
            if (shootDelayTimer >= 1f && shootCooldown <= 0f)
            {
                ShootAtPlayer();
                shootCooldown = shootRate;  // Reset cooldown based on shootRate
                shootDelayTimer = 0f;  // Reset the delay timer after shooting
            }
            else
            {
                // If the delay hasn't passed yet, decrease the cooldown as usual
                shootCooldown -= Time.deltaTime;  
            }

            return; // Don't chase or attack while shooting
        }

        // Stop chasing if player is too far or if blocked by a wall
        if (distanceToPlayer > stopRange && isChasing || isBlocked)
        {
            StopChasing();
        }

        // Start chasing if player is close enough and not blocked by wall
        if (distanceToPlayer <= detectionRange && !isChasing && !isBlocked)
        {
            StartChasing();
        }

        // Chase if we're in chase mode and not blocked by wall
        if (isChasing && !isBlocked)
        {
            ChasePlayer();
        }

        // Flip the enemy sprite to face the player (no rotation)
        FlipEnemySpriteTowardPlayer();
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
        direction.y = 0; // Optional: remove vertical movement
        transform.position += direction * speed * Time.deltaTime;
    }

    GameObject[] FindPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }

    void ChooseTargetPlayer()
    {
        if (players.Length == 0)
        {
            return;
        }

        float closestDistance = Mathf.Infinity;
        GameObject closestPlayer = null;

        foreach (var p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestPlayer = p;
            }
        }

        targetPlayer = closestPlayer;
        if (targetPlayer != null)
        {
            Debug.Log("Targeting player: " + targetPlayer.name);
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null || targetPlayer == null) return;

        // Adjust the player's target height (raise it by 1 unit on the Y-axis)
        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.y += 35f;  // Optional: raise target to eye level or adjust as necessary

        // Calculate the direction from the fire point to the adjusted target position
        Vector3 direction = targetPosition - firePoint.position;
        direction.z = 0;  // Ensure the bullet moves on the X-Y plane

        // Normalize the direction vector to get a consistent speed
        direction.Normalize();

        // Instantiate the bullet at the fire point and set its direction
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Set the bullet's direction through BulletScript (or your custom bullet logic)
        bullet.GetComponent<BulletScript>().Initialize(direction);

        Debug.Log("Ranged enemy shot a bullet!");
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

    // Check if the wall checker hits an obstacle (wall or ground)
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isBlocked = true;
            StopChasing(); // Stop chasing if blocked
            Debug.Log("Wall detected. Stopping chase.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isBlocked = false;
            Debug.Log("Wall no longer blocking.");
        }
    }
}
