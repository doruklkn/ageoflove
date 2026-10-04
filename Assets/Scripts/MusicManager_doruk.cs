using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    public AudioClip menuMusic;

    private AudioSource audioSource;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.pitch = 1f;
        audioSource.panStereo = 0f;
        audioSource.spatialBlend = 0f;
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
    }

    void Start()
    {
        PlayMusic(menuMusic);
    }

    public void PlayMusic(AudioClip newMusic)
    {
        if (newMusic == null)
        {
            Debug.LogError("MusicManager: Music clip NULL!");
            return;
        }

        // Aynı müzik zaten çalıyorsa baştan başlatma
        if (audioSource.clip == newMusic && audioSource.isPlaying)
            return;

        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();

        Debug.Log("Playing music: " + newMusic.name);
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}