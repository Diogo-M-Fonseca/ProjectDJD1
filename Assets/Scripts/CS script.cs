using UnityEngine;
using UnityEngine.SceneManagement;

public class CSscript : MonoBehaviour
{
    public bool player1 = false;
    public bool player2 = false;
    public bool player3 = false;
    public void OpenLvl(int levelID)
    {
        SceneManager.LoadSceneAsync(levelID);
    }
    public void Character(int num)
    {
        if (num == 3)
        {
            player3 = true;
        }
        else if (num == 2)
        {
            player2 = true;
        }
        else if (num == 1)
        {
            player1 = true;
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
