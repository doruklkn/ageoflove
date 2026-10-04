using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LetterMovement_doruk : MonoBehaviour
{
    public RectTransform targetCircle;
    public float spawnY = 300f;

    public GameObject hitParticles;
    public Transform particleParent;

    // SMOKE EFFECT
    public SmokeEffect_doruk smokeEffect;

    // SPRITELAR
    public Sprite lSprite;
    public Sprite oSprite;
    public Sprite vSprite;
    public Sprite eSprite;

    private KeyCode correctKey;
    private int currentLetterIndex;

    private Image letterImage;
    private Image targetImage;
    private Color originalColor;

    void Start()
    {
        letterImage = GetComponent<Image>();

        targetImage = targetCircle.GetComponent<Image>();
        originalColor = targetImage.color;

        SetRandomLetter();
    }

    void Update()
    {
        transform.Translate(
            Vector3.down *
            LetterManager.instance.gameSpeed *
            Time.deltaTime
        );

        if (Input.GetKeyDown(correctKey))
        {
            RectTransform letterRect = GetComponent<RectTransform>();

            float distance = Vector2.Distance(
                letterRect.anchoredPosition,
                targetCircle.anchoredPosition
            );

            if (distance < 60f)
            {
                LetterManager.instance.Correct();

                targetImage.color = Color.green;

                ScreenShake_doruk shake =
                    FindFirstObjectByType<ScreenShake_doruk>();

                if (shake != null)
                    shake.Shake();

                // SMOKE
                if (smokeEffect != null)
                {
                    smokeEffect.PlaySmoke();
                }

                // PARTIKUL
                if (hitParticles != null)
                {
                    GameObject particles = Instantiate(
                        hitParticles,
                        targetCircle.position,
                        Quaternion.identity,
                        particleParent
                    );

                    Destroy(particles, 1f);
                }

                Debug.Log("HIT! DOGRU!");

                StartCoroutine(ResetColor());

                ResetLetter();
            }
            else
            {
                // YANLIS ZAMAN
                LetterManager.instance.Wrong();

                targetImage.color = Color.red;

                Debug.Log("MISS!");

                StartCoroutine(ResetColor());
            }
        }

        // HARF HEDEFI GECTI
        if (GetComponent<RectTransform>().anchoredPosition.y <
            targetCircle.anchoredPosition.y - 100f)
        {
            LetterManager.instance.Wrong();

            targetImage.color = Color.red;

            Debug.Log("MISS! HARFI KACIRDIN!");

            StartCoroutine(ResetColor());

            ResetLetter();
        }
    }

    void SetRandomLetter()
    {
        currentLetterIndex =
            LetterManager.instance.GetRandomAvailableLetter();

        if (currentLetterIndex == 0)
        {
            letterImage.sprite = lSprite;
            correctKey = KeyCode.L;
        }
        else if (currentLetterIndex == 1)
        {
            letterImage.sprite = oSprite;
            correctKey = KeyCode.O;
        }
        else if (currentLetterIndex == 2)
        {
            letterImage.sprite = vSprite;
            correctKey = KeyCode.V;
        }
        else
        {
            letterImage.sprite = eSprite;
            correctKey = KeyCode.E;
        }
    }

    void ResetLetter()
    {
        LetterManager.instance.ReleaseLetter(currentLetterIndex);

        RectTransform letterRect = GetComponent<RectTransform>();

        Vector2 position = letterRect.anchoredPosition;

        position.y = spawnY;

        letterRect.anchoredPosition = position;

        SetRandomLetter();
    }

    IEnumerator ResetColor()
    {
        yield return new WaitForSecondsRealtime(0.12f);

        targetImage.color = originalColor;
    }
}