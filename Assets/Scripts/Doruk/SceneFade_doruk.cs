using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFade_doruk : MonoBehaviour
{
    public static SceneFade_doruk instance;

    public Image fadeImage;
    public float fadeDuration = 1.5f;
    public string nextSceneName;

    private bool isFading = false;

    void Awake()
    {
        instance = this;
    }

    public void FadeToNextScene()
    {
        if (isFading)
            return;

        isFading = true;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timer = 0f;

        // Başlangıçta tamamen şeffaf
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress = timer / fadeDuration;

            color.a = Mathf.Lerp(0f, 1f, progress);

            fadeImage.color = color;

            yield return null;
        }

        // Tamamen siyah
        color.a = 1f;
        fadeImage.color = color;

        // Yeni scene normal hızda başlasın
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}