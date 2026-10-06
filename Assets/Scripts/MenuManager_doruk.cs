using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Transition")]
    public Image blackScreen;
    public TMP_Text dateText;

    public float fadeDuration = 1.5f;
    public float blackScreenDuration = 1f;
    public float dateDuration = 3f;

    [Header("Next Scene")]
    public string nextSceneName = "Scene1_doruk";

    private bool transitionStarted = false;


    void Start()
    {
        // BlackScreen başlangıçta görünmez
        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0f;
            blackScreen.color = color;
        }

        // Tarih başlangıçta görünmez
        if (dateText != null)
        {
            dateText.gameObject.SetActive(false);
        }
    }


    // PLAY BUTONU BUNA BASACAK
    public void PlayGame()
    {
        if (transitionStarted)
            return;

        transitionStarted = true;

        StartCoroutine(PlayGameTransition());
    }


    IEnumerator PlayGameTransition()
    {
        // =========================
        // 1. EKRANI KARART
        // =========================

        float timer = 0f;

        Color color = blackScreen.color;
        color.a = 0f;
        blackScreen.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float progress =
                Mathf.Clamp01(
                    timer / fadeDuration
                );

            color.a =
                Mathf.Lerp(
                    0f,
                    1f,
                    progress
                );

            blackScreen.color = color;

            yield return null;
        }

        // Tamamen siyah
        color.a = 1f;
        blackScreen.color = color;


        // =========================
        // 2. SİYAH EKRAN BEKLESİN
        // =========================

        yield return new WaitForSecondsRealtime(
            blackScreenDuration
        );


        // =========================
        // 3. TARİHİ GÖSTER
        // =========================

        if (dateText != null)
        {
            dateText.gameObject.SetActive(true);
        }


        // =========================
        // 4. TARİH BEKLESİN
        // =========================

        yield return new WaitForSecondsRealtime(
            dateDuration
        );


        // =========================
        // 5. SCENE 1'E GEÇ
        // =========================

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            nextSceneName
        );
    }


    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }


    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");

        Debug.Log("basildi");
    }


    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Game Quit");
    }
}