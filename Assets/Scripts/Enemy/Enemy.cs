using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject playerPrefab; // The reference to the Player prefab
    public float detectionRange = 10f;  // Distance the enemy can detect the player (smaller radius to start chasing)
    public float stopRange = 15f;  // Distance the enemy will stop chasing (larger radius)
    public float speed = 3f;  // How fast the enemy moves toward the player

    private GameObject player; // The instance of the player in the scene
    private bool isChasing = false; // Flag to check if the enemy is currently chasing the player

    private void Start()
    {
        // Search for the player prefab in the scene
        player = FindPlayerPrefab();
    }

    private void Update()
    {
        // If the player is not found, exit early
        if (player == null)
        {
            Debug.Log("Player prefab not found in the scene.");
            return;
        }

        // Check distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        // If the player is within the stop range, check if we should stop chasing
        if (distanceToPlayer > stopRange && isChasing)
        {
            StopChasing();
        }

        // If the player is within the detection range, chase the player
        if (distanceToPlayer <= detectionRange && !isChasing)
        {
            StartChasing();
        }

        // If the enemy is chasing, move towards the player
        if (isChasing)
        {
            ChasePlayer();
        }
    }

    // Start chasing the player when within detection range
    void StartChasing()
    {
        isChasing = true;
        Debug.Log("Starting to chase player!");
    }

    // Stop chasing the player when outside of stop range
    void StopChasing()
    {
        isChasing = false;
        Debug.Log("Stopped chasing player!");
    }

    // Move the enemy towards the player's position
    void ChasePlayer()
    {
        // Move the enemy towards the player's position
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        transform.position += direction * speed * Time.deltaTime;
        Debug.Log("Chasing player!"); // Debugging info: Check if the enemy is moving
    }

    // Find the Player prefab in the scene
    GameObject FindPlayerPrefab()
    {
        // Search for the player prefab in the scene
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        
        // If multiple instances of the player prefab exist, return the first one found
        if (playerObjects.Length > 0)
        {
            return playerObjects[0]; // Returning the first instance found
        }
        
        return null; // Return null if no player prefab is found
    }
}
