using UnityEngine;
using UnityEngine.UI;  // For accessing UI components like Slider

public class RageBarDisplay : MonoBehaviour
{
    public Slider rageSlider;  // Reference to the Slider component
    public RageBar rageBar;    // Reference to the RageBar script

    void Start()
    {
        if (rageSlider == null)
        {
            Debug.LogError("RageBarUI: RageSlider is not assigned!");
        }

        if (rageBar == null)
        {
            Debug.LogError("RageBarUI: RageBar script is not assigned!");
        }

        // Initialize the Slider's max value based on the RageBar's max rage
        if (rageBar != null && rageSlider != null)
        {
            rageSlider.maxValue = rageBar.maxRage;
        }
    }

    void Update()
    {
        // Update the Slider value based on the current rage
        if (rageBar != null && rageSlider != null)
        {
            rageSlider.value = rageBar.currentRage;
        }
    }
}
