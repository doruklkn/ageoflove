using UnityEngine;

public class LetterSpawner : MonoBehaviour
{
    public GameObject letterPrefab;

    public float spawnInterval = 3f;
    public float spawnX = 12f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

 void Start()
    {
        InvokeRepeating(nameof(SpawnLetter), 2f, spawnInterval);
    }

    public void SpawnLetter()
    {
        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);

        Instantiate(letterPrefab, spawnPosition, Quaternion.identity);
    }
}