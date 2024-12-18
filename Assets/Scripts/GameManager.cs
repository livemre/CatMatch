using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        // Tekil instance (Singleton)
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Sahne geçişlerinde yok olmasın
        DontDestroyOnLoad(gameObject);
    }

    // Gecikmeli obje oluşturma metodunu başlat
    public void SpawnDelayed(GameObject prefab, Vector3 position, float delay)
    {
        StartCoroutine(SpawnDelayedRoutine(prefab, position, delay));
    }

    private IEnumerator SpawnDelayedRoutine(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(prefab, position, Quaternion.identity);
    }
    
    
}


