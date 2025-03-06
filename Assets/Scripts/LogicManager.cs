using UnityEngine;
using UnityEngine.UI;
public class LogicManager : MonoBehaviour
{
    private int playerscore;
    public Text scoretext;

    private void addScore()
    {
        playerscore++;
        scoretext.text = playerscore.ToString();
    }

}
