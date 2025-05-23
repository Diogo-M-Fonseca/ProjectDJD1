using UnityEngine;

public class RageBar : MonoBehaviour
{
    public float currentRage = 50f;  // Starting value for testing
    public int maxRage = 100;  // Maximum rage value
    public float depletionTime = 10f;  // How long (in seconds) it should take to deplete the rage bar fully
    public float speedBoostAmount = 1.5f;  // How much faster the player gets (1 = no boost, 1.5 = 50% faster)

    private bool isDepleting = false;  // Whether the rage bar is currently depleting
    private PlayerMovement playerMovement;  // Reference to the player movement script
    private PlayerMovementWithDash playerMovementWithDash;
    private float originalSpeed;  // Store the player's original speed
    private float originalSpeedwithDash;
    private float timePassed = 0f;  // Timer to track how much time has passed
    public bool isInvul = false;

    private void Start()
    {
        // Find the PlayerMovement script on the player GameObject
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        playerMovementWithDash = Object.FindFirstObjectByType<PlayerMovementWithDash>();

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.speed;  // Store the original speed
        }
        else
        {
            Debug.LogError("PlayerMovement script not found!");
        }
        if (playerMovementWithDash != null)
        {
            originalSpeedwithDash = playerMovementWithDash.speed;
        }
        else
        {
            Debug.LogError("PlayerMovementWithDash script not found!");
        }
    }

    private void Update()
    {
        // Check if the F key is pressed and start the depletion process
        if (Input.GetKeyDown(KeyCode.F) && currentRage > 0)
        {
            isDepleting = true;  // Start depleting
            isInvul = true;

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
                isInvul = false;

            }

            BoostPlayerSpeed();  // Boost the player's speed while depleting the rage
        }

    }

    // Method to add rage points to the bar
    public void AddRage(float amount)
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
        if (playerMovementWithDash != null)
        {
            if (currentRage > 0)
            {
                // Increase the player's speed as the RageBar depletes
                playerMovementWithDash.speed = originalSpeedwithDash * speedBoostAmount;  // Boost speed
            }
            else
            {
                // Reset the player's speed when rage reaches 0
                playerMovementWithDash.speed = originalSpeedwithDash;  // Reset speed to original value
            }
        }
    }
}
