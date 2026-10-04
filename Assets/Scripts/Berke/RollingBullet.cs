using System.Collections;
using UnityEngine;

public class RollingBullet : MonoBehaviour
{
    [Header("Görsel Dilimler (4 Kare)")]
    public Sprite[] rollFrames;

    [Header("Mumla Koordinasyon")]
    public float stepDelay = 0.2f;

    [Header("Mesafe Ayarı")]
    public float stepDistance = 0.35f;
    public int totalSteps = 20;

    [Tooltip("Mermi kaçıncı adımdayken ses çalsın? (Masanın ucuna geldiği adım)")]
    public int playDropStep = 12;

    public Vector2 moveDirection = new Vector2(0.15f, -1.0f);

    [Header("Düşme Sesi Ayarları")]
    public AudioClip bulletDropClip;
    [Range(0f, 1f)] public float dropVolume = 0.9f;

    [Header("Bitiş")]
    public bool hideOnFinish = true;

    private SpriteRenderer spriteRenderer;
    private RectTransform rectTransform;
    private AudioSource audioSource;
    private bool isRolling = false;
    private bool soundPlayed = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rectTransform = GetComponent<RectTransform>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Start()
    {
        StartRolling();
    }

    public void StartRolling()
    {
        if (isRolling) return;
        isRolling = true;
        soundPlayed = false;
        StartCoroutine(SteppedRollRoutine());
    }

    private IEnumerator SteppedRollRoutine()
    {
        Vector3 stepVector = new Vector3(moveDirection.x, moveDirection.y, 0f).normalized * stepDistance;
        int frameIndex = 0;

        for (int i = 0; i < totalSteps; i++)
        {
            // Belirlenen adımda sesi ANINDA tetikle
            if (i >= playDropStep && !soundPlayed)
            {
                soundPlayed = true;
                if (bulletDropClip != null)
                {
                    audioSource.PlayOneShot(bulletDropClip, dropVolume);
                }
            }

            if (rollFrames != null && rollFrames.Length > 0 && spriteRenderer != null)
            {
                spriteRenderer.sprite = rollFrames[frameIndex];
                frameIndex = (frameIndex + 1) % rollFrames.Length;
            }

            if (rectTransform != null)
                rectTransform.anchoredPosition += (Vector2)stepVector * 100f;
            else
                transform.position += stepVector;

            yield return new WaitForSeconds(stepDelay);
        }

        isRolling = false;

        if (hideOnFinish)
        {
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            yield return new WaitForSeconds(1.5f); // Sesin son yankısı kesilmesin
            gameObject.SetActive(false);
        }
    }
}