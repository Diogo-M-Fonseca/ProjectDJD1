using UnityEngine;
using TMPro;

public class BulletDisplay : MonoBehaviour
{
    // Reference to the UI TextMeshProUGUI element
    public TextMeshProUGUI bulletCountText;

    // This method will update the bullet count UI text
    public void UpdateBulletCount(float bullets)
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + bullets.ToString();
        }
    }
}
