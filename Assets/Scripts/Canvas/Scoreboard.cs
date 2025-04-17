using TMPro;
using UnityEngine;
using UnityEngine.UI; // Only needed if you want to show the score in UI

public class Scoreboard : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText; // Assign this UI Text in the Inspector if you want

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Update the UI with the new score
        }
    }
}
