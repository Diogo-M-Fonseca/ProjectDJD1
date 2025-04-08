using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro; // Only if using TextMeshPro

public class SceneEnterPrompt : MonoBehaviour
{
    public Tilemap sceneChangeTilemap;
    public TileBase warpTile;
    public string sceneToLoad;
    public GameObject enterPromptUI; // Drag your "Press E" UI here

    private bool isNearWarp = false;

    void Update()
    {
        Vector3Int playerCellPos = sceneChangeTilemap.WorldToCell(transform.position);
        Vector3Int aboveCellPos = new Vector3Int(playerCellPos.x, playerCellPos.y + 1, playerCellPos.z);

        TileBase currentTile = sceneChangeTilemap.GetTile(aboveCellPos);

        isNearWarp = currentTile == warpTile;

        enterPromptUI.SetActive(isNearWarp); // Show/hide prompt

        if (isNearWarp && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
