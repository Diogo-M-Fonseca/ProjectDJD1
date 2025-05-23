using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int scoreValue = 1; // How much this enemy is worth
    public float RageBarValue; // How much the enemy adds to the RageBar when killed

    private Scoreboard scoreboard;
    private RageBar rageBar;  // Separate RageBar script for the rage points
    private EnemySpawner enemySpawner;

    private void Start()
    {
        // Find the Scoreboard and RageBar in the scene
        scoreboard = FindFirstObjectByType<Scoreboard>(); // Changed to FindObjectOfType
        rageBar = FindFirstObjectByType<RageBar>();  // Changed to FindObjectOfType

        if (scoreboard == null)
        {
            Debug.LogError("No Scoreboard found in the scene!");
        }
        if (rageBar == null)
        {
            Debug.LogError("No RageBar found in the scene!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider has the AttackArea script (note correct case)
        if (collision.GetComponent<attackArea>() != null)  // Corrected case
        {
            Debug.Log("Enemy hit by attack!");

            // Increase the score
            if (scoreboard != null)
            {
                scoreboard.AddScore(scoreValue);
            }

            // Increase the RageBar
            if (rageBar != null)
            {
                rageBar.AddRage(RageBarValue);
            }

            // Destroy this enemy
            Destroy(gameObject);
            enemySpawner = FindObjectOfType<EnemySpawner>();
            enemySpawner.enemyInRoom--;
            if (enemySpawner.spawnTime <= 0 && enemySpawner.enemyInRoom <= 0);
            {
                enemySpawner.spawnerDone = true;
            }

        }
    }
}
