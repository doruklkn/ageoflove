using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Level3Manager : MonoBehaviour
{
    [Header("UI Metin Alanları")]
    public TextMeshProUGUI targetWordText;
    public TextMeshProUGUI playerMorseInputText;
    public TextMeshProUGUI feedbackText;

    [Header("Kitap / İpucu Engeli")]
    public GameObject bookHintPanel;

    [Header("Bölüm Bitiş Ekranı")]
    public GameObject levelCompletedPanel;
    public float panelFadeDuration = 1.0f;
    public float delayBeforeNextScene = 2.5f;
    public string nextSceneName = "Scene4";

    [Header("Görsel Efektler")]
    public Image screenFlashImage;
    public RectTransform shakeTarget;
    public float typeSpeed = 0.06f;

    [Header("Telgraf Zamanlama & Ses")]
    public float dashThreshold = 0.22f;
    public AudioClip customBeepClip;
    [Range(200, 1200)] public float morseFrequency = 750f;
    [Range(0f, 1f)] public float beepVolume = 0.4f;

    [Header("Mermi Referansı")]
    public RollingBullet rollingBullet;

    [System.Serializable]
    public struct MorseMission
    {
        public string word;
        public string morseCode;
    }

    [Header("Görev Havuzu")]
    public List<MorseMission> missions = new List<MorseMission>();

    private AudioSource audioSource;
    private int currentMissionIndex = 0;
    private string currentEnteredMorse = "";
    private float keyDownTime = 0f;
    private bool isKeyHeld = false;
    private bool isLevelFinished = false;
    private bool isTypingTargetWord = false;

    private double sampleRate;
    private double phase = 0.0;
    private bool isGeneratingSound = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        sampleRate = AudioSettings.outputSampleRate;
    }

    void Start()
    {
        if (levelCompletedPanel != null)
        {
            CanvasGroup cg = levelCompletedPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = levelCompletedPanel.AddComponent<CanvasGroup>();
            cg.alpha = 0f;
            levelCompletedPanel.SetActive(false);
        }

        if (feedbackText != null) feedbackText.text = "";

        if (screenFlashImage != null)
        {
            Color c = screenFlashImage.color;
            c.a = 0f;
            screenFlashImage.color = c;
        }

        missions.Clear();
        missions.Add(new MorseMission { word = "AGE OF LOVE", morseCode = ".- --. . --- ..-. .-.. --- ...- ." });
        missions.Add(new MorseMission { word = "CONNECTION", morseCode = "-.-. --- -. -. . -.-. - .. --- -." });
        missions.Add(new MorseMission { word = "ACROSS AGES", morseCode = ".- -.-. .-. --- ... ... .- --. . ..." });
        missions.Add(new MorseMission { word = "TIMELESS", morseCode = "- .. -- . .-.. . ... ..." });
        missions.Add(new MorseMission { word = "FOREVER", morseCode = "..-. --- .-. . ...- . .-." });
        missions.Add(new MorseMission { word = "DEVOTION", morseCode = "-.. . ...- --- - .. --- -." });

        LoadMission(0);
    }

    void Update()
    {
        if (isLevelFinished) return;

        if ((bookHintPanel != null && bookHintPanel.activeSelf) || isTypingTargetWord)
        {
            if (isKeyHeld) StopBeep();
            return;
        }

        // M Tuşu: Mors Girişi
        if (Input.GetKeyDown(KeyCode.M))
        {
            keyDownTime = Time.time;
            isKeyHeld = true;
            StartBeep();
        }

        if (Input.GetKeyUp(KeyCode.M) && isKeyHeld)
        {
            float pressDuration = Time.time - keyDownTime;
            isKeyHeld = false;
            StopBeep();

            if (pressDuration < dashThreshold)
                currentEnteredMorse += ".";
            else
                currentEnteredMorse += "-";

            UpdateInputDisplay();
        }

        // Space veya B: Boşluk Bırakma
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.B))
        {
            if (currentEnteredMorse.Length > 0 && !currentEnteredMorse.EndsWith(" "))
            {
                currentEnteredMorse += " ";
                UpdateInputDisplay();
            }
        }

        // Backspace: Silme
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentEnteredMorse.Length > 0)
            {
                currentEnteredMorse = currentEnteredMorse.Substring(0, currentEnteredMorse.Length - 1);
                UpdateInputDisplay();
            }
        }

        // Enter: Gönderme
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SubmitMorse();
        }
    }

    void LoadMission(int index)
    {
        if (index >= missions.Count)
        {
            TriggerLevelComplete();
            return;
        }

        currentMissionIndex = index;
        currentEnteredMorse = "";
        UpdateInputDisplay();

        if (targetWordText != null)
        {
            StopCoroutine(nameof(TypewriterWordRoutine));
            StartCoroutine(TypewriterWordRoutine(missions[currentMissionIndex].word));
        }

        if (feedbackText != null) feedbackText.text = "";
    }

    private IEnumerator TypewriterWordRoutine(string fullWord)
    {
        isTypingTargetWord = true;
        targetWordText.text = "";

        foreach (char c in fullWord)
        {
            targetWordText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTypingTargetWord = false;
    }

    void UpdateInputDisplay()
    {
        if (playerMorseInputText != null)
        {
            playerMorseInputText.text = currentEnteredMorse;
        }
    }

    void SubmitMorse()
    {
        // Boşlukları temizleyip tek tip hale getirerek karşılaştırıyoruz
        string cleanExpected = CleanMorse(missions[currentMissionIndex].morseCode);
        string cleanPlayer = CleanMorse(currentEnteredMorse);

        Debug.Log($"[KONTROL] Beklenen: '{cleanExpected}' | Girilen: '{cleanPlayer}'");

        if (cleanPlayer == cleanExpected)
        {
            Debug.Log("<color=green>[BAŞARILI] Kod doğru! Panel tetikleniyor...</color>");
            OnSuccess();
        }
        else
        {
            Debug.Log("<color=red>[HATALI] Kod uyuşmadı!</color>");
            OnFail();
        }
    }

    // Fazladan girilen veya eksik kalan boşlukları toparlayan temizleyici
    string CleanMorse(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        string[] parts = input.Trim().Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", parts);
    }

    void OnSuccess()
    {
        StartCoroutine(FlashRoutine(Color.green));
        TriggerLevelComplete();
    }

    void OnFail()
    {
        if (feedbackText != null) feedbackText.text = "";

        StartCoroutine(FlashRoutine(Color.red));
        if (shakeTarget != null)
            StartCoroutine(ShakeRoutine(shakeTarget, 0.25f, 15f));

        currentEnteredMorse = "";
        UpdateInputDisplay();

        if (currentMissionIndex + 1 < missions.Count)
            LoadMission(currentMissionIndex + 1);
        else
            LoadMission(0);
    }

    public void TriggerLevelComplete()
    {
        isLevelFinished = true;
        if (feedbackText != null) feedbackText.text = "";

        StartCoroutine(ShowCompletedPanelRoutine());
    }

    private IEnumerator ShowCompletedPanelRoutine()
    {
        if (levelCompletedPanel != null)
        {
            // Paneli en öne taşı ve aç
            levelCompletedPanel.transform.SetAsLastSibling();
            levelCompletedPanel.SetActive(true);

            CanvasGroup cg = levelCompletedPanel.GetComponent<CanvasGroup>();
            if (cg == null) cg = levelCompletedPanel.AddComponent<CanvasGroup>();

            cg.alpha = 0f;
            float elapsed = 0f;

            // Yavaşça görünür yap (Fade-In)
            while (elapsed < panelFadeDuration)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Clamp01(elapsed / panelFadeDuration);
                yield return null;
            }
            cg.alpha = 1f;
        }
        else
        {
            Debug.LogError("DİKKAT: GameManager üzerindeki 'Level Completed Panel' yuvası BOŞ!");
        }

        yield return new WaitForSeconds(delayBeforeNextScene);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning($"'{nextSceneName}' Build Settings içinde ekli değil!");
            }
        }
    }

    private IEnumerator FlashRoutine(Color flashColor)
    {
        if (screenFlashImage == null) yield break;

        screenFlashImage.color = flashColor;
        float elapsed = 0f;
        float duration = 0.35f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsed / duration);
            Color c = screenFlashImage.color;
            c.a = alpha;
            screenFlashImage.color = c;
            yield return null;
        }

        Color finalC = screenFlashImage.color;
        finalC.a = 0f;
        screenFlashImage.color = finalC;
    }

    private IEnumerator ShakeRoutine(RectTransform target, float duration, float magnitude)
    {
        Vector2 originalPos = target.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            target.anchoredPosition = originalPos + new Vector2(x, y);

            elapsed += Time.deltaTime;
            yield return null;
        }

        target.anchoredPosition = originalPos;
    }

    void StartBeep()
    {
        if (customBeepClip != null)
        {
            audioSource.clip = customBeepClip;
            audioSource.volume = beepVolume;
            audioSource.Play();
        }
        else
        {
            isGeneratingSound = true;
            if (!audioSource.isPlaying) audioSource.Play();
        }
    }

    void StopBeep()
    {
        if (customBeepClip != null)
            audioSource.Stop();
        else
            isGeneratingSound = false;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!isGeneratingSound) return;

        double increment = morseFrequency * 2.0 * System.Math.PI / sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            float sample = (float)(System.Math.Sin(phase) * beepVolume);

            for (int c = 0; c < channels; c++)
                data[i + c] += sample;

            if (phase > 2.0 * System.Math.PI) phase -= 2.0 * System.Math.PI;
        }
    }
}