using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI; // Only needed if you want to show the score in UI

public class Scoreboard : MonoBehaviour
{
    [SerializeField]
    private int score;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TMP_Text finalScoreText;
    [SerializeField]
    private TMP_Text highScoreText;
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
        
        if (scoreText != null)
        {
            scoreText.text =  score.ToString(); // Update the UI with the new score
        }
    }

    public void HighScoreUpdate()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (score > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedHighScore", score);
        }
        finalScoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt("SavedHighScore").ToString();
    }
}
