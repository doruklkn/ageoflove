using UnityEngine;
using TMPro;
using System.Collections;

public class Countdown_ece : MonoBehaviour
{
    public TMP_Text countdownText;

    void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }
}