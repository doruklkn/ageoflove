using UnityEngine;
using TMPro;

public class ScoreUI_doruk : MonoBehaviour
{
    public TMP_Text scoreText;

    void Update()
    {
        if (LetterManager.instance != null && scoreText != null)
        {
            scoreText.text = "SCORE: " + LetterManager.instance.score;
        }
    }
}