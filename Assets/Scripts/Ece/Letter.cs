using UnityEngine;

public class Letter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();

            gameManager.CollectLetter();

            Destroy(gameObject);
        }
    }
}