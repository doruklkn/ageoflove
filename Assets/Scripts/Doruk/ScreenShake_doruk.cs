using UnityEngine;
using System.Collections;

public class ScreenShake_doruk : MonoBehaviour
{
    public float duration = 0.08f;
    public float magnitude = 5f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-magnitude, magnitude);
            float y = Random.Range(-magnitude, magnitude);

            rectTransform.anchoredPosition =
                originalPosition + new Vector2(x, y);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }
}