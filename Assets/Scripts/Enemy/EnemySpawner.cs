using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Spawner;
    [SerializeField]
    private GameObject[] spawnPoints;
    [SerializeField] 
    private GameObject[] enemies;
    [SerializeField]
    private GameObject currentPoint;
    [SerializeField]
    private int index;
    [SerializeField]
    private float minTimeToSpawn;
    [SerializeField]
    private float maxTimeToSpawn;
    [SerializeField]
    private bool canSpawn = false;
    public float spawnTime;
    public int enemyInRoom;
    public bool spawnerDone;
    [SerializeField]
    private GameObject spawnerDoneGameObject;

    private void Start()
    {
    }

    private void Update()
    {
        if (canSpawn)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime < 0)
            {
                spawnTime = 5;
                SpawnEnemy();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canSpawn = false;
        }
    }
    void SpawnEnemy()
    {
        index = Random.Range(0, spawnPoints.Length);
        currentPoint = spawnPoints[index];
        float timeBtwSpawns = Random.Range(minTimeToSpawn, maxTimeToSpawn);


        if (canSpawn)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], currentPoint.transform.position, Quaternion.identity);
            enemyInRoom++;
        }

        if (spawnerDone)
        {
            spawnerDoneGameObject.SetActive(true);
        }
    }
}