using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuToDorukTransition : MonoBehaviour
{
    public Image blackScreen;
    public TMP_Text dateText;

    public float fadeDuration = 1.5f;
    public float blackScreenDuration = 1f;
    public float dateDuration = 3f;

    public string nextSceneName = "Scene1_doruk";

    private bool started = false;

    void Start()
    {
        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0f;
            blackScreen.color = color;
        }

        if (dateText != null)
            dateText.gameObject.SetActive(false);
    }

    public void StartTransition()
    {
        if (started)
            return;

        started = true;
        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        // Önce ekran kararsın
        float timer = 0f;

        Color color = blackScreen.color;
        color.a = 0f;
        blackScreen.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(
                0f,
                1f,
                timer / fadeDuration
            );

            blackScreen.color = color;

            yield return null;
        }

        color.a = 1f;
        blackScreen.color = color;

        // Biraz siyah ekran
        yield return new WaitForSecondsRealtime(
            blackScreenDuration
        );

        // Tarihi göster
        if (dateText != null)
            dateText.gameObject.SetActive(true);

        // Tarih ekranda kalsın
        yield return new WaitForSecondsRealtime(
            dateDuration
        );

        // Scene1
        SceneManager.LoadScene(
            "Scene1_doruk"
        );
    }
}