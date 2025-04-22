using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float detectionRange = 10f;  // Distance the enemy can detect the player (start chasing)
    public float stopRange = 15f;        // Distance the enemy will stop chasing
    public float speed = 3f;             // Enemy movement speed

    private GameObject[] players;        // Array to store all players found in the scene
    private GameObject targetPlayer;     // The player the enemy is currently targeting
    private bool isChasing = false;       // Is the enemy currently chasing?

    private void Start()
    {
        players = FindPlayers();          // Find all players
        ChooseTargetPlayer();             // Pick one to target
    }

    private void Update()
    {
        if (targetPlayer == null)
        {
            Debug.Log("No target player selected.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        if (distanceToPlayer > stopRange && isChasing)
        {
            StopChasing();
        }

        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            StartChasing();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    // Start chasing the player
    void StartChasing()
    {
        isChasing = true;
        Debug.Log("Starting to chase player!");
    }

    // Stop chasing the player
    void StopChasing()
    {
        isChasing = false;
        Debug.Log("Stopped chasing player!");
    }

    // Move toward the targeted player
    void ChasePlayer()
    {
        Vector3 direction = (targetPlayer.transform.position - transform.position).normalized;
        direction.y = 0; // Optional: ignore vertical movement
        transform.position += direction * speed * Time.deltaTime;
        Debug.Log("Chasing player!");
    }

    // Find all player objects in the scene
    GameObject[] FindPlayers()
    {
        return GameObject.FindGameObjectsWithTag("Player");
    }

    // Choose the closest player to target
    void ChooseTargetPlayer()
    {
        if (players.Length == 0)
        {
            Debug.LogWarning("No players found in the scene!");
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
}
