using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroTimer : MonoBehaviour
{
    public float duration = 5f;

    void Start()
    {
        Invoke(nameof(LoadNextScene), duration);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Scene2_ece");
    }
}