using UnityEngine;

public class RageBar : MonoBehaviour
{
    public float currentRage = 50f;  // Starting value for testing
    public int maxRage = 100;  // Maximum rage value
    public float depletionTime = 10f;  // How long (in seconds) it should take to deplete the rage bar fully
    public float speedBoostAmount = 1.5f;  // How much faster the player gets (1 = no boost, 1.5 = 50% faster)

    private bool isDepleting = false;  // Whether the rage bar is currently depleting
    private PlayerMovement playerMovement;  // Reference to the player movement script
    private float originalSpeed;  // Store the player's original speed
    private float timePassed = 0f;  // Timer to track how much time has passed

    private void Start()
    {
        // Find the PlayerMovement script on the player GameObject
        playerMovement = Object.FindObjectOfType<PlayerMovement>();

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.speed;  // Store the original speed
        }
        else
        {
            Debug.LogError("PlayerMovement script not found!");
        }
    }

    private void Update()
    {
        // Check if the F key is pressed and start the depletion process
        if (Input.GetKeyDown(KeyCode.F) && currentRage > 0)
        {
            isDepleting = true;  // Start depleting
            Debug.Log("Depletion Started!");
        }

        // If we're depleting the rage bar, decrease it gradually over time
        if (isDepleting)
        {
            timePassed += Time.deltaTime;  // Increase the time tracker with each frame

            // Deplete the current rage evenly over time
            float depletionRate = maxRage / depletionTime;  // Calculate the rate of depletion per second
            currentRage -= depletionRate * Time.deltaTime;  // Subtract the depletion rate each frame

            // Ensure currentRage doesn't go below 0
            if (currentRage < 0)
            {
                currentRage = 0;
                isDepleting = false;  // Stop depleting when the rage bar hits 0
                Debug.Log("Rage Bar Depleted!");
            }

            BoostPlayerSpeed();  // Boost the player's speed while depleting the rage
        }

        // Debugging log to track the current rage value
        Debug.Log("Current Rage: " + currentRage);
    }

    // Method to add rage points to the bar
    public void AddRage(int amount)
    {
        currentRage += amount;
        if (currentRage > maxRage)  // Cap the rage bar at the max value
        {
            currentRage = maxRage;
        }
        Debug.Log("Rage Bar Added: " + currentRage);
    }

    // Method to boost the player's speed while the rage bar is depleting
    private void BoostPlayerSpeed()
    {
        if (playerMovement != null)
        {
            if (currentRage > 0)
            {
                // Increase the player's speed as the RageBar depletes
                playerMovement.speed = originalSpeed * speedBoostAmount;  // Boost speed
            }
            else
            {
                // Reset the player's speed when rage reaches 0
                playerMovement.speed = originalSpeed;  // Reset speed to original value
            }
        }
    }
}
