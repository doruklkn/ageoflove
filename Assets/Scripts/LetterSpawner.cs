using UnityEngine;

public class LetterSpawner : MonoBehaviour
{
    public GameObject letterPrefab;

    void Start()
    {
        SpawnLetter();
    }

    public void SpawnLetter()
    {
        GameObject newLetter = Instantiate(
            letterPrefab,
            transform.position,
            Quaternion.identity,
            transform.parent
        );

        LetterMovement movement = newLetter.GetComponent<LetterMovement>();
        movement.targetCircle = GameObject.Find("targetCircle").GetComponent<RectTransform>();
    }
}