using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class SceneChangerWithInput : MonoBehaviour
{
    public Tilemap sceneChangeTilemap;
    public TileBase warpTile;
    public string sceneToLoad;

    public GameObject enterPromptUI;

    void Update()
    {
        enterPromptUI.SetActive(false); 
        
        Vector3Int playerCellPos = sceneChangeTilemap.WorldToCell(transform.position);
        Vector3Int aboveCellPos = new Vector3Int(playerCellPos.x, playerCellPos.y + 1, playerCellPos.z);

        TileBase currentTile = sceneChangeTilemap.GetTile(aboveCellPos);

        if (currentTile == warpTile)
        {
            enterPromptUI.SetActive(true);
        }

        if (currentTile == warpTile && Input.GetKeyDown(KeyCode.E))
        {
        SceneManager.LoadScene(sceneToLoad);
        }

    }
}