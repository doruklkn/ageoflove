using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FinalFade : MonoBehaviour
{
    public static FinalFade Instance;

    [Header("Fade")]
    public Image blackScreen;

    public float fadeDuration = 2f;
    public float blackScreenDuration = 3f;

    [Header("Next Scene")]
    public string nextSceneName = "Menu";

    private bool started = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0f;
            blackScreen.color = color;
        }
    }

    public void StartFinalFade()
    {
        if (started)
            return;

        started = true;

        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float timer = 0f;

        Color color = blackScreen.color;

        // Ekranı karart
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            color.a =
                Mathf.Lerp(
                    0f,
                    1f,
                    timer / fadeDuration
                );

            blackScreen.color = color;

            yield return null;
        }

        color.a = 1f;
        blackScreen.color = color;

        // Siyah ekranı biraz beklet
        yield return new WaitForSecondsRealtime(
            blackScreenDuration
        );

        // Sonraki sahne
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            nextSceneName
        );
    }
}