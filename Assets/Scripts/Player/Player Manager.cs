using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CSscript cSscript;
    [SerializeField]
    private GameObject playerknife;
    [SerializeField]
    private GameObject playerBat;
    [SerializeField]
    private GameObject playerShotgun;
    void Update()
    {
        if (cSscript.player1 == true)
        {
            playerBat.SetActive(false);
            playerknife.SetActive(true);
            playerShotgun.SetActive(false);
            Debug.Log("tryed to set active");
        }
        else if (cSscript.player2 == true)
        {
            playerBat.SetActive(true);
            playerknife.SetActive(false); 
            playerShotgun.SetActive(false);
            Debug.Log("tryed to set active");
        }
        else if (cSscript.player3 == true)
        {
            playerBat.SetActive(false);
            playerknife.SetActive(false);
            playerShotgun.SetActive(true);
            Debug.Log("tryed to set active");
        }
        else
        {
            Debug.Log("none were true");
        }
    }
}
