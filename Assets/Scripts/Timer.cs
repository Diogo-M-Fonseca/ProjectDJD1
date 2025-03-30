using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mathf;
public class CountdownTimer : MonoBehaviour
{
    public float totalTime = 90;
    public Text timerText;

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
        }
    }
}
    