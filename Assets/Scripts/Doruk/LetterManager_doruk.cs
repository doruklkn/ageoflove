using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterManager : MonoBehaviour
{
    public static LetterManager instance;

    private bool[] letterUsed = new bool[4];

    public int score = 0;
    public float gameSpeed = 200f;

    public GameObject gameOverText;
    public GameObject youWonText;

    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f;
    }

    public int GetRandomAvailableLetter()
    {
        int[] available = new int[4];
        int count = 0;

        for (int i = 0; i < 4; i++)
        {
            if (!letterUsed[i])
            {
                available[count] = i;
                count++;
            }
        }

        int selected = available[Random.Range(0, count)];

        letterUsed[selected] = true;

        return selected;
    }

    public void ReleaseLetter(int letterIndex)
    {
        letterUsed[letterIndex] = false;
    }

    public void Correct()
    {
        if (gameEnded)
            return;

        score += 10;

        UpdateSpeed();

        Debug.Log("DOGRU! Score: " + score);

        if (score >= 300)
        {
            score = 300;
            YouWon();
        }
    }

    public void Wrong()
    {
        if (gameEnded)
            return;

        score -= 20;

        if (score < 0)
        {
            score = 0;
            GameOver();
            return;
        }

        UpdateSpeed();

        Debug.Log("YANLIS! Score: " + score);
    }

    void Update()
    {
        if (gameEnded && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;

            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex
            );
        }
    }

    void UpdateSpeed()
    {
        if (score >= 200)
        {
            gameSpeed = 370f;
        }
        else if (score >= 100)
        {
            gameSpeed = 290f;
        }
        else
        {
            gameSpeed = 200f;
        }
    }

    void GameOver()
    {
        gameEnded = true;

        Debug.Log("GAME OVER!");

        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void YouWon()
    {
        gameEnded = true;

        Debug.Log("YOU WON!");

        if (youWonText != null)
        {
            youWonText.SetActive(true);
        }

        if (SceneFade_doruk.instance != null)
        {
            SceneFade_doruk.instance.FadeToNextScene();
        }

        // Fade'in çalışması için burada timeScale'i 0 YAPMIYORUZ.
        // Fade zaten unscaledDeltaTime kullanıyor.
    }
}