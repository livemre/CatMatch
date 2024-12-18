using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour

{
    public static UIManager Instance;
    public GameObject[] catPrefabs; // Tüm kedi prefab'leri

    private VisualElement nextCatPreview; // Bir sonraki kedinin önizlemesini gösterecek alan
    private Label scoreLabel; // Skoru gösterecek Label
    private VisualElement gameOverPanel;

    private int score = 0; // Oyuncunun puanı

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // UIManager'ın sahneler arasında yok olmamasını sağlar
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        // UIDocument içindeki VisualElement kökünü alın
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // UI'deki elemanları bulun
        nextCatPreview = root.Q<VisualElement>("nextCatPreview");
        scoreLabel = root.Q<Label>("scoreLabel");
        gameOverPanel = root.Q<VisualElement>("gameOverPanel");

        // Debug: Kontrol amaçlı log ekleyin
        if (nextCatPreview == null)
            Debug.LogError("nextCatPreview adında bir VisualElement bulunamadı. UXML dosyasını kontrol edin.");
        
        if (scoreLabel == null)
            Debug.LogError("scoreLabel adında bir Label bulunamadı. UXML dosyasını kontrol edin.");

        // Başlangıçta skor UI'yi güncelle
        UpdateScoreUI();
    }

    /// <summary>
    /// Bir sonraki kedi önizlemesini günceller.
    /// </summary>
    /// <param name="nextCatPrefab">Bir sonraki kedinin prefab'i.</param>
    public void UpdateNextCatUI(GameObject nextCatPrefab)
    {
        // Null kontrolü
        if (nextCatPrefab == null)
        {
            Debug.LogWarning("Güncellenecek kedi prefab'i null.");
            nextCatPreview.style.backgroundImage = null;
            return;
        }

        // Kedinin SpriteRenderer bileşenini kontrol et
        var spriteRenderer = nextCatPrefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Kedinin sprite'ını arka plan olarak UI'de göster
            nextCatPreview.style.backgroundImage = new StyleBackground(spriteRenderer.sprite.texture);
        }
        else
        {
            // SpriteRenderer bulunamazsa uyarı ver
            Debug.LogWarning($"Prefab '{nextCatPrefab.name}' için SpriteRenderer bulunamadı.");
            nextCatPreview.style.backgroundImage = null;
        }
    }

    /// <summary>
    /// Skoru artırır ve UI'yi günceller.
    /// </summary>
    /// <param name="points">Eklenecek puan.</param>
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    /// <summary>
    /// Skor değerini UI'de günceller.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreLabel != null)
        {
            scoreLabel.text = $"Score: {score}";
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Oyun Bitti!");

        // Oyunu durdur
       // Time.timeScale = 0f;

        // UI üzerinde Game Over mesajını göster
        var uiDocument = FindObjectOfType<UIDocument>();
        var root = uiDocument.rootVisualElement;
        var gameOverLabel = root.Q<Label>("gameOverLabel");

        if (gameOverLabel != null)
        {
            gameOverLabel.text = "Game Over!";
            gameOverLabel.style.display = DisplayStyle.Flex;
            gameOverPanel.style.display = DisplayStyle.Flex;
            gameOverPanel.style.backgroundColor = Color.black;
        }
    }
}