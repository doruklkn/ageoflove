using UnityEngine;
using UnityEngine.SceneManagement;

public class MeteGameManager : MonoBehaviour
{
    public static MeteGameManager Instance;

    [Header("Engeller (her biri ayrı prefab)")]
    public GameObject[] obstaclePrefabs;

    [Header("Bilgisayarlar")]
    public Transform startComputer;
    public Transform goalComputer;
    public float introDuration = 2f;
    public float endDuration = 2.5f;

    [Header("Zorluk (başlangıç -> son)")]
    public float startInterval = 0.5f;
    public float minInterval = 0.12f;
    public float startSpeed = 7f;
    public float maxSpeed = 18f;
    public float spawnRangeX = 7f;
    [Range(0f, 1f)] public float aimedChance = 0.4f;

    [Header("Oyun")]
    public float winTime = 45f;

    [Header("Kaydırma efekti")]
    public float startComputerScrollSpeed = 4f;
    public float goalApproachTime = 4f;
    public float goalStartOffset = 6f;

    [Header("Efektler")]
    public GameObject[] flyEffects;

    [Header("İlerleme barı")]
    public float barWidth = 30f;
    public float barHeightRatio = 0.6f;
    public Color barColor = Color.green;

    enum State { Intro, Playing, Ending, Over }
    State state = State.Intro;

    MetePlayerMove player;

    float timer;
    float elapsed;
    float stateTimer;

    Vector3 playerTarget;
    Vector3 endStartPos;
    Vector3 endStartScale;

    string message = "";

    Vector3 goalRestPos;
    Vector3 goalHiddenPos;

    bool startComputerGone;

    public bool IsPlaying
    {
        get { return state == State.Playing; }
    }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Uçuş efektleri giriş animasyonu bitene kadar kapalı
        foreach (GameObject fx in flyEffects)
        {
            if (fx != null)
                fx.SetActive(false);
        }

        // GoalComputer başta yukarıda bekler
        if (goalComputer != null)
        {
            goalRestPos = goalComputer.position;

            goalHiddenPos =
                goalRestPos +
                Vector3.up * goalStartOffset;

            goalComputer.position = goalHiddenPos;
        }

        player = FindAnyObjectByType<MetePlayerMove>();

        if (player == null)
            return;

        playerTarget = player.transform.position;

        player.controlEnabled = false;

        // Sinyal başlangıç bilgisayarının içinde başlar
        if (startComputer != null)
            player.transform.position = startComputer.position;

        player.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (state == State.Over)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;

                SceneManager.LoadScene(
                    SceneManager.GetActiveScene().buildIndex
                );
            }

            return;
        }

        if (player == null)
            return;

        if (state == State.Intro)
            UpdateIntro();

        else if (state == State.Playing)
            UpdatePlaying();

        else if (state == State.Ending)
            UpdateEnding();
    }

    void UpdateIntro()
    {
        stateTimer += Time.deltaTime;

        float t =
            Mathf.Clamp01(
                stateTimer / introDuration
            );

        float e =
            Mathf.SmoothStep(
                0f,
                1f,
                t
            );

        Vector3 from =
            startComputer != null
            ? startComputer.position
            : playerTarget;

        player.transform.position =
            Vector3.Lerp(
                from,
                playerTarget,
                e
            );

        player.transform.localScale =
            player.BaseScale * e;

        if (t >= 1f)
        {
            player.transform.position =
                playerTarget;

            player.transform.localScale =
                player.BaseScale;

            player.controlEnabled = true;

            foreach (GameObject fx in flyEffects)
            {
                if (fx != null)
                    fx.SetActive(true);
            }

            state = State.Playing;
        }
    }

    void ScrollStartComputer()
    {
        if (startComputer == null ||
            startComputerGone)
            return;

        startComputer.position +=
            Vector3.down *
            startComputerScrollSpeed *
            Time.deltaTime;

        if (startComputer.position.y <
            -(Camera.main.orthographicSize + 6f))
        {
            startComputer.gameObject.SetActive(false);

            startComputerGone = true;
        }
    }

    void UpdatePlaying()
    {
        ScrollStartComputer();

        elapsed += Time.deltaTime;
        timer += Time.deltaTime;

        float approachStart =
            winTime -
            goalApproachTime;

        float t =
            Mathf.Clamp01(
                elapsed / winTime
            );

        float interval =
            Mathf.Lerp(
                startInterval,
                minInterval,
                t
            );

        float speed =
            Mathf.Lerp(
                startSpeed,
                maxSpeed,
                t
            );

        // Son saniyelerde yeni engel gelmez
        if (timer >= interval &&
            elapsed < approachStart)
        {
            timer = 0f;

            int count =
                t > 0.5f
                ? 2
                : 1;

            for (int i = 0; i < count; i++)
                Spawn(speed);
        }

        // Bitiş bilgisayarı aşağı iner
        if (goalComputer != null &&
            elapsed > approachStart)
        {
            float g =
                Mathf.Clamp01(
                    (elapsed - approachStart) /
                    goalApproachTime
                );

            goalComputer.position =
                Vector3.Lerp(
                    goalHiddenPos,
                    goalRestPos,
                    Mathf.SmoothStep(
                        0f,
                        1f,
                        g
                    )
                );
        }

        if (elapsed >= winTime)
            BeginEnding();
    }

    void BeginEnding()
    {
        state = State.Ending;

        if (goalComputer != null)
            goalComputer.position =
                goalRestPos;

        // Çıkış sesi
        if (MeteAudio.Instance != null)
            MeteAudio.Instance.PlayCikis();

        stateTimer = 0f;

        player.controlEnabled = false;

        endStartPos =
            player.transform.position;

        endStartScale =
            player.transform.localScale;

        // Engelleri temizle
        foreach (
            MeteObstacle o in
            FindObjectsByType<MeteObstacle>(
                FindObjectsSortMode.None
            )
        )
        {
            Destroy(o.gameObject);
        }
    }

    void UpdateEnding()
    {
        stateTimer += Time.deltaTime;

        float t =
            Mathf.Clamp01(
                stateTimer / endDuration
            );

        float e =
            Mathf.SmoothStep(
                0f,
                1f,
                t
            );

        Vector3 to =
            goalComputer != null
            ? goalComputer.position
            : endStartPos;

        player.transform.position =
            Vector3.Lerp(
                endStartPos,
                to,
                e
            );

        // Son %30'da küçülerek bilgisayara girer
        float shrink =
            1f -
            Mathf.Clamp01(
                (t - 0.7f) / 0.3f
            );

        player.transform.localScale =
            endStartScale * shrink;

        if (t >= 1f)
            Win();
    }

    GameObject PickPrefab()
    {
        float total = 0f;

        foreach (GameObject p in obstaclePrefabs)
        {
            MeteObstacle o =
                p.GetComponent<MeteObstacle>();

            total +=
                o != null
                ? o.weight
                : 1f;
        }

        float r =
            Random.value * total;

        foreach (GameObject p in obstaclePrefabs)
        {
            MeteObstacle o =
                p.GetComponent<MeteObstacle>();

            r -=
                o != null
                ? o.weight
                : 1f;

            if (r <= 0f)
                return p;
        }

        return obstaclePrefabs[0];
    }

    void Spawn(float speed)
    {
        if (obstaclePrefabs == null ||
            obstaclePrefabs.Length == 0)
            return;

        float x =
            Random.Range(
                -spawnRangeX,
                spawnRangeX
            );

        if (Random.value < aimedChance)
        {
            x =
                Mathf.Clamp(
                    player.transform.position.x +
                    Random.Range(-1f, 1f),
                    -spawnRangeX,
                    spawnRangeX
                );
        }

        float y =
            Camera.main.orthographicSize + 2f;

        GameObject obj =
            Instantiate(
                PickPrefab(),
                new Vector3(x, y, 0f),
                Quaternion.identity
            );

        MeteObstacle ob =
            obj.GetComponent<MeteObstacle>();

        if (ob != null)
        {
            ob.fallSpeed =
                speed *
                Random.Range(
                    0.8f,
                    1.2f
                );
        }
    }

    public void Lose()
    {
        if (state != State.Playing)
            return;

        state = State.Over;

        message =
            "SIGNAL LOST\nRESTART WITH R";

        if (MeteAudio.Instance != null)
            MeteAudio.Instance.PlayLose();

        Time.timeScale = 0f;
    }

    void Win()
    {
        state = State.Over;

        message = "";

        // Final fade başlat
        if (FinalFade.Instance != null)
        {
            FinalFade.Instance.StartFinalFade();
        }
        else
        {
            Debug.LogError(
                "FINAL FADE BULUNAMADI!"
            );
        }
    }

    void DrawProgressBar()
    {
        float progress = 0f;

        if (state == State.Playing ||
            state == State.Ending ||
            state == State.Over)
        {
            progress =
                Mathf.Clamp01(
                    elapsed / winTime
                );
        }

        float barH =
            Screen.height *
            barHeightRatio;

        float x =
            Screen.width -
            barWidth -
            30f;

        float y =
            (Screen.height - barH) / 2f;

        // Dış çerçeve
        GUI.color =
            new Color(
                1f,
                1f,
                1f,
                0.35f
            );

        GUI.DrawTexture(
            new Rect(
                x - 4f,
                y - 4f,
                barWidth + 8f,
                barH + 8f
            ),
            Texture2D.whiteTexture
        );

        // Arka plan
        GUI.color = Color.black;

        GUI.DrawTexture(
            new Rect(
                x,
                y,
                barWidth,
                barH
            ),
            Texture2D.whiteTexture
        );

        // Dolan kısım
        float fillH =
            barH * progress;

        GUI.color = barColor;

        GUI.DrawTexture(
            new Rect(
                x,
                y + barH - fillH,
                barWidth,
                fillH
            ),
            Texture2D.whiteTexture
        );

        GUI.color = Color.white;
    }

    void OnGUI()
    {
        GUIStyle small =
            new GUIStyle(
                GUI.skin.label
            )
            {
                fontSize = 28
            };

        small.normal.textColor =
            Color.white;

        DrawProgressBar();

        if (state == State.Intro)
        {
            GUI.Label(
                new Rect(
                    20,
                    20,
                    600,
                    50
                ),
                "Sending Signal...",
                small
            );
        }

        if (state == State.Over)
        {
            GUIStyle big =
                new GUIStyle(
                    GUI.skin.label
                )
                {
                    fontSize = 60,
                    alignment =
                        TextAnchor.MiddleCenter
                };

            big.normal.textColor =
                Color.red;

            GUI.Label(
                new Rect(
                    0,
                    0,
                    Screen.width,
                    Screen.height
                ),
                message,
                big
            );
        }
    }
}