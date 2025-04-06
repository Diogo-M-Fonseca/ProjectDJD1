using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 10;
    public Transform target;


    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x + 200f, target.position.y + 180, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed*Time.deltaTime);
    }
}

