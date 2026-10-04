using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1_doruk");
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