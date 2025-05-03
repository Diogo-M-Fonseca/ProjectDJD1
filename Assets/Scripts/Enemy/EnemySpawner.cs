using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
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
    private bool canSpawn;
    public float spawnTime;
    public int enemyInRoom;
    public bool spawnerDone;
    [SerializeField]
    private GameObject spawnerDoneGameObject;

    private void Start()
    {
        Invoke("SpawnEnemy", 0.5f);
    }

    private void Update()
    {
        if (canSpawn)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime < 0)
            {
                spawnTime = 5;
            }
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

        Invoke("SpawnEnemy", timeBtwSpawns);
        if (spawnerDone)
        {
            spawnerDoneGameObject.SetActive(true);
        }
    }
}