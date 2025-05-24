using UnityEngine;
using UnityEngine.UI;

public class RageBarDisplay : MonoBehaviour
{
    [SerializeField] private Image rageSlider;
    [SerializeField] private Sprite[] sprites;

    private RageBar rageBar;

    private void Start()
    {
        // Find all RageBar components in the scene
        RageBar[] allRageBars = FindObjectsByType<RageBar>(FindObjectsSortMode.None);

        foreach (var rb in allRageBars)
        {
            if (rb.gameObject.activeInHierarchy)
            {
                rageBar = rb;
                Debug.Log("Active RageBar found: " + rb.gameObject.name);
                break;
            }
        }

        if (rageBar == null)
        {
            Debug.LogWarning("No active RageBar found in the scene.");
        }
    }

    private void Update()
    {
        if (rageBar == null) return;

        float rage = rageBar.currentRage;

        if (rage <= 0f)
        {
            rageSlider.sprite = sprites[0];
        }
        else if (rage == 12.5f)
        {
            rageSlider.sprite = sprites[1];
        }
        else if (rage == 25f)
        {
            rageSlider.sprite = sprites[2];
        }
        else if (rage == 37.5f)
        {
            rageSlider.sprite = sprites[3];
        }
        else if (rage == 50f)
        {
            rageSlider.sprite = sprites[4];
        }
        else if (rage == 62.5f)
        {
            rageSlider.sprite = sprites[5];
        }
        else if (rage == 75f)
        {
            rageSlider.sprite = sprites[6];
        }
        else if (rage <= 87.5f)
        {
            rageSlider.sprite = sprites[7];
        }
        else if (rage == 100f)
        {
            rageSlider.sprite = sprites[8];
        }
    }
}
