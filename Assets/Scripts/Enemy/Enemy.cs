using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;  // For scene management

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float detectionRange = 10f;   // Start chasing at this distance
    [SerializeField]
    private float stopRange = 15f;        // Stop chasing beyond this distance
    [SerializeField]
    private float speed = 3f;             // Movement speed
    [SerializeField]
    private float attackRange = 2f;       // Attack when within this distance
    [SerializeField]
    private bool isAttacking = false;    // Attack flag
    [SerializeField]
    private GameObject attackArea;        // Reference to hitbox GameObject (this includes the sprite)
    [SerializeField]
    private GameObject highscore;

    private GameObject[] players;        // All players in scene
    private GameObject targetPlayer;     // Closest player
    private bool isChasing = false;      // Are we currently chasing?
    private bool isBlocked = false;      // Is the enemy blocked by a wall?

    [SerializeField]
    private Collider2D wallChecker;     // Wall checker collider

    private void Start()
    {
        players = FindPlayers();
        ChooseTargetPlayer();
    }

    private void Update()
    {
        if (targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        // Prioritize attacking
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                StopChasing();
                StartCoroutine(AttackPlayer());
            }
            return; // Stop chasing while attacking
        }

        // Stop chasing if player is too far or if blocked by a wall
        if (distanceToPlayer > stopRange && isChasing || isBlocked)
        {
            StopChasing();
        }

        // Start chasing if player is close enough
        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            StartChasing();
        }

        // Chase if we're in chase mode
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

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        Debug.Log("Preparing to attack...");

        yield return new WaitForSeconds(1f); // Wind-up delay

        // Activate the visual or hitbox if needed
        attackArea.SetActive(true);
        Debug.Log("Attack area activated!");

        // Check for player hit using OverlapCircle
        float hitRadius = 0.5f; // Adjust to match your weapon hit size
        Collider2D hit = Physics2D.OverlapCircle(attackArea.transform.position, hitRadius, LayerMask.GetMask("Player"));

        if (hit != null && hit.gameObject == targetPlayer)
        {
            Debug.Log("Player hit by enemy attack!");
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
            Debug.Log("Attack missed or player was out of range.");
        }

        yield return new WaitForSeconds(0.3f); // Duration of visible hitbox

        attackArea.SetActive(false);
        Debug.Log("Attack area deactivated!");

        yield return new WaitForSeconds(0.5f); // Cooldown before next possible attack

        isAttacking = false;
    }

    // Flip the enemy's sprite to face the player (side-scrolling version)
    private void FlipEnemySpriteTowardPlayer()
    {
        if (targetPlayer == null) return;

        // Get direction to the player
        Vector2 directionToPlayer = targetPlayer.transform.position - transform.position;

        // Flip the enemy sprite based on the direction
        if (directionToPlayer.x > 0 && transform.localScale.x < 0)  // Player is on the right and the enemy is flipped
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (directionToPlayer.x < 0 && transform.localScale.x > 0)  // Player is on the left and the enemy is not flipped
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Detect the attack collision with the closest player and reset the scene
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the closest player
        if (other.gameObject == targetPlayer)
        {
            Debug.Log("Player hit!");
            highscore.SetActive(true);
        }
    }

    // Check if the wall checker hits an obstacle (wall or ground)
    private void OnTriggerStay2D(Collider2D other)
    {
        // If we hit something on the "Ground" layer, stop chasing
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

    // Reload the current scene
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
