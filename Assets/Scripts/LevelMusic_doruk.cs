using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public AudioClip music;

    void Start()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.PlayMusic(music);
        }
    }
}