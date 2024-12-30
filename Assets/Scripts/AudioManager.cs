using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioSource;
   

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Bu script sahne değişikliklerinde yok olmaz
        }
        else
        {
            Destroy(gameObject);
        }
    }

   

    /// <summary>
    /// Ses efekti çalar.
    /// </summary>
    /// <param name="clip">Çalınacak ses klibi.</param>
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            
            audioSource.PlayOneShot(clip);
        }
    }
    
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Muziği Durdur!");

        }
    }
}