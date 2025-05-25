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
    private IPlayer targetPlayerScript;
    [SerializeField] private Animator animator;

    private GameObject[] players;        // All players in scene
    private GameObject targetPlayer;     // Closest player
    private bool isChasing = false;      // Are we currently chasing?
    private bool isBlocked = false;      // Is the enemy blocked by a wall?

    [SerializeField]
    private Collider2D wallChecker;     // Wall checker collider

    private void Start()
    {
        FindPlayerByInterface();
    }

    private void Update()
    {
        if (targetPlayer == null)
        {
            FindPlayerByInterface();
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);

        // Prioritize attacking
        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking)
            {
                StopChasing();
                animator.SetBool("Moving", false);
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
        animator.SetBool("Moving", isChasing);
    }

    void StopChasing()
    {
        isChasing = false;
        animator.SetBool("Moving", isChasing);
    }

    void ChasePlayer()
    {
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        direction.y = 0; // Optional: remove vertical movement
        transform.position += direction * speed * Time.deltaTime;
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


    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetBool("Attacking", isAttacking);

        yield return new WaitForSeconds(1f); // Wind-up delay

        // Activate the visual or hitbox if needed
        attackArea.SetActive(true);

        // Check for player hit using OverlapCircle
        float hitRadius = 10f; // Adjust to match your weapon hit size
        Collider2D hit = Physics2D.OverlapCircle(attackArea.transform.position, hitRadius, LayerMask.GetMask("Player"));

        if (hit != null && hit.gameObject == targetPlayer)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        yield return new WaitForSeconds(0.3f); // Duration of visible hitbox

        attackArea.SetActive(false);

        yield return new WaitForSeconds(0.5f); // Cooldown before next possible attack

        isAttacking = false;
        animator.SetBool("Attacking", isAttacking);
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
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isBlocked = false;
        }
    }

    // Reload the current scene
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }
}
