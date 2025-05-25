using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 10f;
    private Transform target;
    [SerializeField]
    private float cameraX = 200f;
    [SerializeField]
    private float cameraY = 180f;

    private void Start()
    {
        FindPlayerByInterface();
    }

    void Update()
    {
        if (target == null)
        {
            FindPlayerByInterface(); // Try to re-find player if null
            return;
        }

        Vector3 newPos = new Vector3(target.position.x + cameraX, target.position.y + cameraY, -10f);
        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
    void FindPlayerByInterface()
    {
        MonoBehaviour[] allMonoBehaviours = FindObjectsByType<MonoBehaviour>(
        FindObjectsInactive.Include,
        FindObjectsSortMode.None);

        foreach (MonoBehaviour mb in allMonoBehaviours)
        {
            if (mb is IPlayer)
            {
                target = mb.transform;
                break;
            }
        }
    }
}