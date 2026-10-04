using UnityEngine;

public class MetePlayerMove : MonoBehaviour
{
    public float speed = 10f;
    public float padding = 0.1f;

    [Header("Zıplama")]
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpDuration = 0.5f;
    public float jumpCooldown = 0.8f;
    public float jumpScale = 0.6f;
    public Transform shadow;

    [HideInInspector] public bool controlEnabled = true; // giriş/final sırasında GameManager kapatır

    float limit;
    float jumpTimer = -1f;
    float cooldownTimer;
    Vector3 shadowBaseScale;

    public Vector3 BaseScale { get; private set; }
    public bool IsAirborne { get { return jumpTimer >= 0f; } }

    void Awake()
    {
        BaseScale = transform.localScale;
        if (shadow != null) shadowBaseScale = shadow.localScale;
    }

    void Start()
    {
        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float halfPlayer = GetComponent<SpriteRenderer>().bounds.extents.x;
        limit = halfWidth - halfPlayer - padding;
    }

    void Update()
    {
        if (!controlEnabled) return;

        float x = 0f;
        if (Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;

        Vector3 pos = transform.position;
        pos.x += x * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -limit, limit);
        transform.position = pos;

        cooldownTimer -= Time.deltaTime;
        if (!IsAirborne && cooldownTimer <= 0f && Input.GetKeyDown(jumpKey))
        {
            jumpTimer = 0f;

            // zıplama sesi çalar
            if (MeteAudio.Instance != null) MeteAudio.Instance.PlayJump();
        }

        float factor = 1f;
        if (IsAirborne)
        {
            jumpTimer += Time.deltaTime;
            float t = jumpTimer / jumpDuration;
            if (t >= 1f)
            {
                jumpTimer = -1f;
                cooldownTimer = jumpCooldown;
            }
            else
            {
                factor = 1f + Mathf.Sin(t * Mathf.PI) * jumpScale;
            }
        }

        transform.localScale = BaseScale * factor;
        if (shadow != null) shadow.localScale = shadowBaseScale / factor;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (MeteGameManager.Instance == null || !MeteGameManager.Instance.IsPlaying) return;

        MeteObstacle o = other.GetComponent<MeteObstacle>();
        if (o == null) return;

        if (IsAirborne && o.jumpable) return;

        MeteGameManager.Instance.Lose();
    }
}