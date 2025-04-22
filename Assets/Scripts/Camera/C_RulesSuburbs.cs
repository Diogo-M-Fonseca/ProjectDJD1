using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 10f;
    private Transform target;  // Only one player

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogError("No player found with tag 'Player'!");
        }
    }

    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x + 200f, target.position.y + 180, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed*Time.deltaTime);
    }
}


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

