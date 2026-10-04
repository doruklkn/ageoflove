using UnityEngine;

public class MeteAudio : MonoBehaviour
{
    public static MeteAudio Instance;

    [Header("Müzik")]
    public AudioClip music;                 // metemuzik
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    [Header("Ses efektleri")]
    public AudioClip signalBaslangicSfx;    // signalbaslangic: doğma sesi
    public AudioClip signalCikisSfx;        // signalcikis: karşı bilgisayara giderken
    public AudioClip jumpSfx;               // zıplama sesi (şimdilik boş kalabilir)
    public AudioClip loseSfx;               // ölme sesi (şimdilik boş kalabilir)
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    AudioSource musicSource;
    AudioSource sfxSource;

    void Awake()
    {
        Instance = this;

        // iki "hoparlör": biri müzik için, biri efektler için
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        // 1) doğma sesi çalar
        PlaySfx(signalBaslangicSfx);

        // 2) doğma sesi bitince müzik başlar
        if (music != null)
        {
            float delay = signalBaslangicSfx != null ? signalBaslangicSfx.length : 0f;
            musicSource.PlayDelayed(delay);
        }
    }

    void PlaySfx(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayJump() { PlaySfx(jumpSfx); }

    // ölünce: müzik durur, ölme sesi çalar
    public void PlayLose()
    {
        musicSource.Stop();
        PlaySfx(loseSfx);
    }

    // karşı bilgisayara gidiş: müzik durur, çıkış sesi çalar
    public void PlayCikis()
    {
        musicSource.Stop();
        PlaySfx(signalCikisSfx);
    }
}