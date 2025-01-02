using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameOver = false;
    public AudioClip blackgroundMusic;
   

    private void Awake()
    {
        // Singleton kontrolü
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişiminde yok olmayı engelle
        }
        else
        {
            Destroy(gameObject); // Zaten bir örnek varsa fazlasını yok et
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(blackgroundMusic);
    }

    

    public void GameOver()
    {
        isGameOver = true;

        Debug.Log("Oyun Bitti!");

        // Müziği durdur
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }
        
        

        // Oyun mekaniğini durdur
        Time.timeScale = 0;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.GameOver();
        }
        
      
    }
    
    
    public void RestartGame()
    {
        Debug.Log("Oyun Yeniden Başlatılıyor...");

        // Puanı sıfırla
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResetScore();
        }

        // Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Tüm gerekli değerleri sıfırla
        isGameOver = false;
        Time.timeScale = 1;
        AudioManager.Instance.PlayMusic(blackgroundMusic);

        // // AudioManager'da müzik tekrar başlat
        // if (AudioManager.Instance != null && gameMusic != null)
        // {
        //     AudioManager.Instance.PlayMusic(gameMusic);
        // }
    }
    
    
}