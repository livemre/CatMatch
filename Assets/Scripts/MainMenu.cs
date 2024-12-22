using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements; // UI Toolkit için gerekli

public class MainMenu : MonoBehaviour
{
    private Button playButton; // UI Toolkit butonu

    void OnEnable()
    {
        // UI Document'in kök görsel ağacını alın
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Butonu bulun (UXML'de verilen adı kullanın)
        playButton = root.Q<Button>("playButton");

        // Tıklama olayını dinleyin
        playButton.clicked += OnPlayButtonClicked;
    }

    void OnPlayButtonClicked()
    {
        // GameScane sahnesine geçiş yap
        SceneManager.LoadScene("GameScane");
    }
}