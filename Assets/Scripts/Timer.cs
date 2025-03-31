using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Mathf;
public class CountdownTimer : MonoBehaviour
{
    [SerializeField]
    private float totalTime = 90;
    [SerializeField]
    private Text timerText;

    void Update()
    {
        if (totalTime > 0)
        {
            totalTime -= Time.deltaTime;

            float minutes = Mathf.FloorToInt(totalTime / 60);
            float seconds = Mathf.FloorToInt(totalTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else if (totalTime < 0)
        {
            timerText.text = "Times up!";
            totalTime = 0;
            SceneManager.LoadScene(0, LoadSceneMode.Additive);
        }
    }
}
    