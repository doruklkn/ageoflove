using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Scene3Fade : MonoBehaviour
{
    public Image blackScreen;
    public TMP_Text dateText;

    public float fadeDuration = 1.5f;
    public float blackScreenDuration = 3f;
    public float dateDuration = 3f;

    private bool started = false;

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

    public void StartTransition()
    {
        if (started)
            return;

        started = true;

        Debug.Log("SCENE 3 FADE BAŞLADI!");

        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        // EKRANI KARART
        float timer = 0f;

        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0f;
            blackScreen.color = color;

            while (timer < fadeDuration)
            {
                timer += Time.unscaledDeltaTime;

                float progress = timer / fadeDuration;

                color.a = Mathf.Lerp(0f, 1f, progress);
                blackScreen.color = color;

                yield return null;
            }

            // Tam siyah
            color.a = 1f;
            blackScreen.color = color;
        }

        // Siyah ekranda bekle
        yield return new WaitForSecondsRealtime(blackScreenDuration);

        // TARİHİ GÖSTER
        if (dateText != null)
        {
            dateText.gameObject.SetActive(true);
        }

        // Tarihi gösterdikten sonra bekle
        yield return new WaitForSecondsRealtime(dateDuration);

        // SCENE 4
        Time.timeScale = 1f;

        SceneManager.LoadScene("Scene4_mete");
    }
}