using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lettersCollected = 0;
    public int totalLetters = 5;

    public TextMeshProUGUI letterCounter;
    public GameObject gameOverText;
    public GameObject restartButton;
    public GameObject winText;

    public void CollectLetter()
    {
        lettersCollected++;

        letterCounter.text = lettersCollected + "/" + totalLetters;

        Debug.Log("Letters: " + lettersCollected + "/" + totalLetters);

        if (lettersCollected >= totalLetters)
        {
            WinGame();
        }
    }

   
   public void GameOver()
{
    Debug.Log("GAME OVER");

    gameOverText.SetActive(true);
    restartButton.SetActive(true);

    Time.timeScale = 0f;
}
public void RestartGame()
{
Time.timeScale= 1f;
SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
}

    private void WinGame()
    {
        winText.SetActive(true);
        Time.timeScale = 0f;
    }
}