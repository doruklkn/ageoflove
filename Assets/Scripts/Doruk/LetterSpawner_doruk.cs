using UnityEngine;

public class LetterSpawner_doruk : MonoBehaviour
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

        LetterMovement_doruk movement =
            newLetter.GetComponent<LetterMovement_doruk>();

        movement.targetCircle =
            GameObject.Find("targetCircle").GetComponent<RectTransform>();
    }
}