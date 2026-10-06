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

    [Header("Date")]
    public GameObject dateText;
    public float blackScreenDuration = 3f;

    private bool isFading = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;

        if (dateText != null)
            dateText.SetActive(false);
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

        color.a = 1f;
        fadeImage.color = color;

        yield return new WaitForSecondsRealtime(blackScreenDuration);

        if (dateText != null)
            dateText.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
