using UnityEngine;
using UnityEngine.UI;  // For accessing UI components like Slider

public class RageBarDisplay : MonoBehaviour
{
    [SerializeField]
    private Image rageSlider;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private RageBar rageBar;    // Reference to the RageBar script

    private void Update()
    {
        if (rageBar.currentRage <= 0)
        {
            rageSlider.sprite = sprites[0];
        }
        else if (rageBar.currentRage == 12.5)
        {
            rageSlider.sprite = sprites[1];
        }
        else if (rageBar.currentRage == 25)
        {
            rageSlider.sprite = sprites[2];
        }
        else if (rageBar.currentRage == 37.5)
        {
            rageSlider.sprite = sprites[3];
        }
        else if (rageBar.currentRage == 50)
        {
            rageSlider.sprite = sprites[4];
        }
        else if (rageBar.currentRage == 62.5)
        {
            rageSlider.sprite = sprites[5];
        }
        else if (rageBar.currentRage == 75)
        {
            rageSlider.sprite = sprites[6];
        }
        else if (rageBar.currentRage <= 87.5)
        {
            rageSlider.sprite = sprites[7];
        }
        else if (rageBar.currentRage == 100)
        {
            rageSlider.sprite = sprites[8];
        }


    }
}
