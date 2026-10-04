using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    public float spawnInterval = 2.5f;
    public float spawnX = 10f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), 1f, spawnInterval);
    }

    void SpawnObstacle()
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Length);

        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);

        Instantiate(obstaclePrefabs[randomIndex], spawnPosition, Quaternion.identity);
    }
}