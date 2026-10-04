using UnityEngine;
using TMPro;

public class LetterMovement : MonoBehaviour
{
    public float speed = 200f;
    public RectTransform targetCircle;

    private KeyCode correctKey;
    private TMP_Text letterText;

    void Start()
    {
        letterText = GetComponent<TMP_Text>();

        int randomNumber = Random.Range(0, 4);

        if (randomNumber == 0)
        {
            letterText.text = "W";
            correctKey = KeyCode.W;
        }
        else if (randomNumber == 1)
        {
            letterText.text = "A";
            correctKey = KeyCode.A;
        }
        else if (randomNumber == 2)
        {
            letterText.text = "S";
            correctKey = KeyCode.S;
        }
        else
        {
            letterText.text = "D";
            correctKey = KeyCode.D;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (Input.GetKeyDown(correctKey))
        {
            RectTransform letterRect = GetComponent<RectTransform>();

            float distance = Vector2.Distance(
                letterRect.anchoredPosition,
                targetCircle.anchoredPosition
            );

            if (distance < 80f)
            {
                Debug.Log("HIT! DOGRU!");

                LetterSpawner spawner = FindFirstObjectByType<LetterSpawner>();
                spawner.SpawnLetter();

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("MISS! YANLIS ZAMANDA!");
            }
        }
    }
}