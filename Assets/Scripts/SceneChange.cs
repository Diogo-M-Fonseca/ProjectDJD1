using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad; // Scene name as a string

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the colliding object has the "Player" tag
        {
            Debug.Log("Player has entered the trigger zone. Loading scene: " + sceneToLoad);
            SceneManager.LoadScene(sceneToLoad); // Load the scene by name (string)
        }
    }
}
