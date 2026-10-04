using UnityEngine;

public class MenuMusic : MonoBehaviour
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