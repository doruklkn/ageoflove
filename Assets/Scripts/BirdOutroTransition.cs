using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BirdOutroTransition : MonoBehaviour
{
    public Image blackScreen;
    public TMP_Text dateText;

    public float waitBeforeFade = 2f;
    public float fadeDuration = 1.5f;
    public float blackScreenDuration = 3f;
    public float dateDuration = 3f;

    private bool started = false;

    public void StartTransition()
    {
        if (started)
            return;

        started = true;
        Debug.Log("OUTRO TRANSITION BAŞLADI!");

        StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        Color color = blackScreen.color;
        color.a = 0f;
        blackScreen.color = color;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;

            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            blackScreen.color = color;

            yield return null;
        }

        color.a = 1f;
        blackScreen.color = color;

        yield return new WaitForSecondsRealtime(blackScreenDuration);

        dateText.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(dateDuration);

        Time.timeScale = 1f;

        SceneManager.LoadScene("Scene3_berke");
    }
}