using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float totalTime = 90;
    [SerializeField]
    private TextMeshProUGUI timerText;  // Changed to TextMeshProUGUI
    [SerializeField]
    private GameObject highScoreScreen;
    [SerializeField]
    private Scoreboard Scoreboard;

    void Start()
    {
        timerText.text = totalTime.ToString("F2");  // Convert float to string
    }

    void Update()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(totalTime / 60);
            float seconds = Mathf.FloorToInt(totalTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timerText.text = "Time's up!";
            totalTime = 0;
            GameObject playerObject = GameObject.FindWithTag("Player");
            playerObject.SetActive(false);
            highScoreScreen.SetActive(true);
            Scoreboard.HighScoreUpdate();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Restart scene
        }
    }
}