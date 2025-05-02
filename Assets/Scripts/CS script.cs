using UnityEngine;
using UnityEngine.SceneManagement;

public class CSscript : MonoBehaviour
{
    public void OpenLvl(int levelID)
    {
        SceneManager.LoadSceneAsync(levelID);
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
