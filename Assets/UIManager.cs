using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour

{
    public static UIManager Instance;
    //public GameObject[] catPrefabs; // Tüm kedi prefab'leri

    private VisualElement nextCatPreview; // Bir sonraki kedinin önizlemesini gösterecek alan
    private Label scoreLabel; // Skoru gösterecek Label
    private VisualElement gameOver;
    private Dictionary<GameObject, Texture2D> prefabImageMap; // Eşleştirme
    private Button restartButton;
    
    // Prefab ve görselleri eşleştirmek için bir Dictionary
    public List<GameObject> catPrefabs; // Kedilere ait prefab listesi
    public List<Texture2D> catImages; // Kedilere ait resimler

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
        
      

        // Prefab ve görselleri eşleştir
        prefabImageMap = new Dictionary<GameObject, Texture2D>();

        for (int i = 0; i < catPrefabs.Count; i++)
        {
            if (i < catImages.Count)
            {
                prefabImageMap[catPrefabs[i]] = catImages[i];
            }
            else
            {
                Debug.LogWarning($"Prefab '{catPrefabs[i].name}' için bir görsel tanımlanmamış.");
            }


            // UIDocument içindeki VisualElement kökünü alın
            var uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;

            // UI'deki elemanları bulun
            nextCatPreview = root.Q<VisualElement>("nextCatPreview");
            scoreLabel = root.Q<Label>("scoreLabel");
            gameOver = root.Q<VisualElement>("gameOver");
            restartButton = root.Q<Button>("restartButton");

            // Debug: Kontrol amaçlı log ekleyin
            if (nextCatPreview == null)
                Debug.LogError("nextCatPreview adında bir VisualElement bulunamadı. UXML dosyasını kontrol edin.");

            if (scoreLabel == null)
                Debug.LogError("scoreLabel adında bir Label bulunamadı. UXML dosyasını kontrol edin.");

            // Başlangıçta skor UI'yi güncelle
            UpdateScoreUI();
            
            restartButton.clicked += () =>
            {
                gameOver.style.display = DisplayStyle.None;
                GameManager.Instance.RestartGame();};
        }
    }

    public void UpdateNextCatUI(GameObject nextCatPrefab)
    {
        // Null kontrolü
        if (nextCatPrefab == null)
        {
            Debug.LogWarning("Güncellenecek kedi prefab'i null.");
            nextCatPreview.style.backgroundImage = null;
            return;
        }

        // Prefab'e bağlı görseli kontrol et
        if (prefabImageMap.TryGetValue(nextCatPrefab, out Texture2D image))
        {
            // Görseli UI'ye ekle
            nextCatPreview.style.backgroundImage = new StyleBackground(image);
        }
        else
        {
            // Görsel bulunamazsa uyarı ver
            Debug.LogWarning($"Prefab '{nextCatPrefab.name}' için bir görsel bulunamadı.");
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
            scoreLabel.text = score.ToString();
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Oyun Bitti yarrrak burası UI!");
        
        gameOver.style.display = DisplayStyle.Flex;
        
       

       //  // Oyunu durdur
       // // Time.timeScale = 0f;
       //
       //  // UI üzerinde Game Over mesajını göster
       //  var uiDocument = FindObjectOfType<UIDocument>();
       //  var root = uiDocument.rootVisualElement;
       //  var gameOverLabel = root.Q<Label>("gameOverLabel");
       //
       //  if (gameOverLabel != null)
       //  {
       //      gameOverLabel.text = "Game Over!";
       //      gameOverLabel.style.display = DisplayStyle.Flex;
       //      gameOverPanel.style.display = DisplayStyle.Flex;
       //      gameOverPanel.style.backgroundColor = Color.black;
       //  }
    }

    public void ResetScore()
    {
        score = 0;
    }
}